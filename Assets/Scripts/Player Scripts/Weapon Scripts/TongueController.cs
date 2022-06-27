using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    // Serialized Fields
    [Header("References")]
    [SerializeField] private CircleCollider2D tongueCollider;
    [SerializeField] PlayerConfig config;
    [SerializeField] GameObject tongueBody;
    [Header("Tongue Variables")]
    [SerializeField] float chargeTime = 1f;
    [SerializeField] float timeToExtend = 0.5f;
    [SerializeField] float tongueLength = 8f;
    [SerializeField] float retractAccelFactor = 1f;
    [SerializeField] float autoRetractionAngle = 90f;
    [Header("Object Interactions")]
    [SerializeField] float objectResidualSpeed = 0.6f;
    [SerializeField] float buttonSpeedReduction = 0.8f;
    [SerializeField] float playerMoveSpeed = 3f;
    [SerializeField] float playerMoveSpeedHeavy = 2f;
    [SerializeField] int criticalFrameWindow = 10;
    [SerializeField] float criticalMultiplier = 2f;
    [SerializeField] float spitknockback = 20f;

    private float deacceleration;
    private float speed;
    private Vector3 velocity;
    private Vector3 direction;
    private Vector3 tongueAxis;
    private Vector3 tongueOrigin;
    private Vector3 finalPos;
    private float distTraveled;
    private float lengthReached;
    private float totalTime = 0f;
    private bool extending;
    private bool paused;

    //"grabbed" signifies sticking onto a large object
    public bool grabbed;
    public bool autoRetract;
    public bool holdingObject;

    // Need bool to check that the tongue can only grab once per instance.
    private bool grabUsed;

    // GameObject containing grabbed object
    public GameObject heldObject;
    private CharacterConfig heldconf;

    //Timer for critical window
    private int criticalTimer;
    private bool criticalUsed;

    // getters & setters
    public float PlayerMoveSpeed {get {return playerMoveSpeed;}}
    public float ChargeTime {get {return chargeTime;}}


    // Updates the tongue (called by state update)
    public void UpdateTongue() {

        // Set spawn
        Vector3 pos = config.transform.position;
        SetSpawn(pos);

        // Checking if tongue is still going out
        if (extending) {
            if (!config.Input.TongueHeld) {
                speed = deacceleration*Time.deltaTime;
                StopExtending();
                // retractAccelFactor
                deacceleration = deacceleration*retractAccelFactor*buttonSpeedReduction;
                speed = deacceleration*Time.deltaTime;
            }
            if (speed <= 0) {
                StopExtending();
                deacceleration = deacceleration*retractAccelFactor;
            }
        }

        //Fix this eventually if we even decide to use it
        /*
        criticalTimer+= Time.delaTime;
        if(!criticalUsed && criticalTimer < criticalFrameWindow && InputHandler.TongueRelease())
        {
            Debug.Log("Critical");
            criticalUsed = true;
            deacceleration = deacceleration * criticalMultiplier;
        };
        */

        velocity = direction*speed;
        distTraveled = distTraveled+speed*Time.deltaTime;
        speed = speed - deacceleration*Time.deltaTime;

        Vector3 nextpos = transform.position + velocity*Time.deltaTime;

        if(!grabbed && !grabUsed) CheckCollision(transform.position, nextpos);

        // if (distTraveled <= 0) player.ReturnTongue();
        transform.position = nextpos;
        ResizeTongueBody();
    }


    // Spits an object out
    public void spitObject() {
        //Should have object to spit
        if(!holdingObject || heldObject == null)
        {
            return;
        }
        heldObject.transform.SetParent(null);
        heldObject.SetActive(true);

        direction = config.Input.Aim;

        if(heldconf != null)
        {
            heldconf.ApplyKnockback(direction, spitknockback);
            heldconf.stunned = true;
        }

        HittableObject inanimateobj = heldObject.transform.root.GetComponent<HittableObject>();
        if(inanimateobj != null){
            inanimateobj.ApplyKnockback(direction, spitknockback);
        }

        heldObject = null;
        holdingObject = false;
    }


    // Checks to see if the tongue hits something
    private void CheckCollision(Vector3 start, Vector3 end) {
        LayerMask mask = LayerMask.GetMask("Object Hurtbox");

        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, mask);
        if (hits.Length != 0) {

            string objectType = hits[0].transform.gameObject.tag;

            if(objectType == "Large Object") {
                grabbed = true;
                grabUsed = true;
                StopExtending();

                //Freeze tongue movement. Set to be child of object it is grabbed to so it moves with it
                speed = 0f;
                velocity = Vector3.zero;
                gameObject.transform.SetParent(hits[0].transform.gameObject.transform);
            }
            if(objectType == "Small Object") {
                grabUsed = true;
                StopExtending();

                //Let object it has grabbed become a child so it follows.
                heldObject = hits[0].transform.gameObject;
                heldconf = heldObject.transform.GetComponentInParent<CharacterConfig>();

                transform.position = heldObject.transform.position;
                heldObject.transform.SetParent(gameObject.transform);

                if(heldconf != null)
                {
                    heldconf.invincible = true;
                    heldconf.grabbed = true;
                }

                //Lose some of the speed when hitting object
                speed = speed*objectResidualSpeed;

                //Start timer
                //criticalTimer = 0;
                //criticalUsed = false;
            }
        }
    }


    // Continually have to change "spawn point" if the player is moving while tongue is out
    public void SetSpawn(Vector3 position) {
        position.y = position.y + 0.11f;
        tongueOrigin = position;
    }

    
    // Also will continually have to change the endpoint
    public void SetEndpoint() {
        finalPos = transform.position;
    }


    // If player lets go tongue must retract
    public void UnGrab() {
        grabbed = false;

        // Change parent back to player
        gameObject.transform.SetParent(config.gameObject.transform);
    }


    // Enable method, called every time the tongue is enabled.
    public void OnEnable() {

        // Direction & position of tongue
        SetSpawn(config.transform.position);
        direction = config.Input.Aim;

        // Save axis to base rotatin on
        tongueAxis = GetAxis(config.GetAngle(direction));

        // Internal settings
        transform.position = tongueOrigin;
        transform.rotation = Quaternion.identity;
        totalTime = 0f;
        distTraveled = 0f;     // DistTraveled is reset once at enable and once when the tongue reaches it's max
        lengthReached = 0f;
        extending = true;
        grabUsed = false;
        deacceleration = 2*tongueLength/Mathf.Pow(timeToExtend,2);
        speed = deacceleration*timeToExtend;

        // Tongue bodies
        tongueBody.transform.rotation = Quaternion.identity;
        float angle = Vector3.Angle(direction, Vector3.right);
        if (direction.y < 0) angle = -angle;
        tongueBody.transform.Rotate(0f,0f,angle);
        tongueBody.SetActive(true);
        tongueCollider.enabled = true;

        // Rotate player
        config.RotateSprite(direction);

        //
        autoRetract = false;
    }


    // Disable method, called every time the tongue is disabled.
    void OnDisable() {
        tongueBody.SetActive(false);
        grabUsed = false;
        tongueCollider.enabled = false;

        if(heldObject != null) holdingObject = true;
    }


    // Resizes the tongue's width and everything.
    public void ResizeTongueBody() {
        tongueBody.transform.position = (transform.position + tongueOrigin) / 2;

        if(grabbed){
            tongueBody.transform.rotation = Quaternion.identity;
            direction = transform.position - tongueOrigin;
            direction.Normalize();
            float angle = Vector3.Angle(direction, Vector3.right);
            if (direction.y < 0) angle = -angle;
            tongueBody.transform.Rotate(0f,0f,angle);

            if(Vector3.Angle(direction, tongueAxis) > autoRetractionAngle)
            {
                autoRetract = true;
            };
        }

        float currentLength = Vector3.Distance(transform.position, tongueOrigin);
        tongueBody.transform.localScale = new Vector3(currentLength*5, 5*(.12f - .06f * (tongueBody.transform.localScale.x / (5*tongueLength))), transform.localScale.z);
    }


    // Stops tongue extending
    private void StopExtending() {
        extending = false;
        lengthReached = distTraveled;
        SetEndpoint();
        distTraveled = 0f;
    }


    // Checks if tongue is finished
    public bool CheckIfFinished() {
        bool finished = false;
        if (!extending) finished = Vector3.Distance(finalPos, tongueOrigin) + distTraveled < 0;
        return finished;
    }


    // Gets axis of tongue for angle purposes (+x, -x, +y, -y)
    private Vector3 GetAxis(int dir) {
        switch(dir) {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.up;
            case 2:
                return Vector3.left;
            case 3:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }
}