using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    // Serialized Fields
    [Header("References")]
    [SerializeField] CircleCollider2D tongueCollider;
    [SerializeField] PlayerConfig config;
    [SerializeField] GameObject tongueBody;
    [Header("Tongue Variables")]
    [SerializeField] float aimAssistRadius = 0.25f;
    [SerializeField] float timeToExtend = 0.5f;
    [SerializeField] float tongueLength = 8f;
    [SerializeField] float retractAccelFactor = 1f;
    [SerializeField] float grappleDamp = 1f;
    [SerializeField] float friction = 0.5f;
    [SerializeField] Vector3 originAdjustment = new(0,0.11f,0);
    //[SerializeField] float playerMoveSpeed = 3f;
    //[SerializeField] float playerMoveSpeedHeavy = 2f;
    //[SerializeField] int criticalFrameWindow = 10;
    //[SerializeField] float criticalMultiplier = 2f;
    [Header("Small Object Interactions")]
    [SerializeField] float objectResidualSpeed = 0.6f;
    [SerializeField] float buttonSpeedReduction = 0.8f;
    [SerializeField] float spitknockback = 20f;
    [Header("Large Object Interactions")]
    [SerializeField] float pullAccelFactor = 1f;
    [SerializeField] float inputMaxSpeed = 5f;
    [SerializeField] float inputAccel = 10f;
    [SerializeField] float flyingMaxSpeed = 20f;
    [SerializeField] float flyingDeaccel = 0.1f;


    float deacceleration;
    float pullSpeed;
    bool pullInOppositeDir;
    Vector3 velocity;
    Vector3 direction;
    Vector3 inputVelocity;
    Vector3 TongueOrigin { get { return config.transform.position + originAdjustment; } }
    Vector3 prevPos;
    bool extending;
    bool paused;
    bool startedSwinging;

    //"grabbed" signifies sticking onto a large object
    bool grabbed;
    bool holdingObject;

    // Need bool to check that the tongue can only grab once per instance.
    bool grabUsed;

    // GameObject containing grabbed object
    public GameObject heldObject;
    CharacterConfig heldconf;

    //Timer for critical window
    int criticalTimer;
    bool criticalUsed;

    // getters & setters
    public float Friction { get { return friction; } }
    public Vector3 Velocity { get { return velocity; } }
    public float Speed { get { return velocity.magnitude; } }
    public Vector3 Direction { get { return direction; } }
    public bool Grabbed { get { return grabbed; } }
    public bool HoldingObject { get { return holdingObject; } }
    public float FlyingDeaccel { get { return flyingDeaccel; } }
    public float FlyingMaxSpeed { get { return flyingMaxSpeed; } }
    public bool IsFinished { get; protected set; }
    //public float PlayerMoveSpeed {get {return playerMoveSpeed;}}


    // Updates the tongue (called by state update)
    public void UpdateTongue()
    {
        // Checking if tongue is still going out
        if (extending) 
        {
            if (!InputManager.Instance.TongueHeld)
            {
                StopExtending();
                // retractAccelFactor
                deacceleration *= retractAccelFactor*buttonSpeedReduction;
                //speed = deacceleration*Time.deltaTime;
                velocity = direction * deacceleration * Time.deltaTime;
            }
            if (Vector3.Dot(velocity, direction) <= 0)
            {
                StopExtending();
                deacceleration *= retractAccelFactor;
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

        //velocity = direction*speed;
        //speed -= deacceleration*Time.deltaTime;
        velocity -= deacceleration * Time.deltaTime * direction;

        //Vector3 nextPos = transform.position + velocity*Time.deltaTime;


        bool shouldUpdate = true;

        if (!grabbed && !grabUsed)
        {
            CheckCollision();
            if (grabUsed)
            {
                prevPos = transform.position;
                shouldUpdate = false;
            }
        }

        if (shouldUpdate) transform.position += velocity * Time.deltaTime;
        ResizeTongueBody();
        prevPos = transform.position;
        if (!IsFinished) IsFinished = CheckIfFinished();
    }


    // Moves the player towards the tongue's end (used when grappling)
    public void PullPlayer()
    {
        float extraSpeed = deacceleration * pullAccelFactor * Time.deltaTime;
        pullSpeed += extraSpeed;
        float newSpeed = config.Speed + extraSpeed;

        Vector3 inverseAxis = (new Vector3(-direction.y, direction.x, 0f)).normalized;
        Vector3 inputDirection = (Vector3.Dot(InputManager.Instance.Move, inverseAxis) * inverseAxis).normalized;
        Vector3 currentDirection = (Vector3.Dot(config.Velocity, inverseAxis) * inverseAxis).normalized;


        if (!startedSwinging)
        {
            inputVelocity = inputDirection.normalized * inputMaxSpeed;
            if (InputManager.Instance.Move != Vector3.zero) startedSwinging = true;
        }
        else
        {
            float magnitude = inputVelocity.magnitude;
            float dotProduct = Vector3.Dot(config.Velocity, inputDirection);

            if (dotProduct > 0f)
            {
                magnitude += inputAccel * Time.deltaTime;
                if (magnitude > inputMaxSpeed) magnitude = inputMaxSpeed;
            }
            else
            {
                inputDirection = -inputDirection;
                if (dotProduct == 0f) inputDirection = currentDirection;
                magnitude -= inputAccel * Time.deltaTime;
            }
            inputVelocity = inputDirection * magnitude;
        }

        config.Velocity = (direction + inputVelocity).normalized * newSpeed;
        if (config.Speed > flyingMaxSpeed) config.Velocity = config.Velocity.normalized * flyingMaxSpeed;

        // tongue velocity
        velocity = -config.Velocity;
        prevPos = transform.position;
        if (!IsFinished) IsFinished = CheckIfFinished();

        // Resetting tongue's variables
        SetEndpoint();
        ResizeTongueBody();
    }


    // Spits an object out
    public void spitObject()
    {
        //Should have object to spit
        if(!holdingObject || heldObject == null)
        {
            return;
        }
        heldObject.transform.position = TongueOrigin;
        heldObject.transform.SetParent(null);
        heldObject.SetActive(true);

        direction = InputManager.Instance.Aim;

        if(heldconf != null)
        {
            heldconf.ApplyKnockback(direction, spitknockback);
            heldconf.projectile = true;
        }

        InteractableObject inanimateobj = heldObject.transform.root.GetComponent<InteractableObject>();
        if(inanimateobj != null)
        {
            inanimateobj.OnHit(direction, spitknockback);
        }

        heldObject = null;
        holdingObject = false;
    }


    // Checks to see if the tongue hits something
    private void CheckCollision()
    {
        Vector3 start = transform.position;
        Vector3 end = start + velocity*Time.deltaTime;

        LayerMask mask = LayerMask.GetMask("Object Hurtbox") | LayerMask.GetMask("Collectible Hitbox");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(start, aimAssistRadius, direction, 0f, layerMask: mask);

        if (hits.Length == 0)
        {
            hits = Physics2D.LinecastAll(start, end, mask);
        }

        if (hits.Length != 0)
        {
            string objectType = hits[0].collider.transform.gameObject.tag;

            if(objectType == "Large Object" || (objectType == "Untagged" && hits[0].transform.gameObject.tag == "Large Object")) 
            {
                grabbed = true;
                grabUsed = true;
                StopExtending();
                SetEndpoint();

                // Set to be child of object it is grabbed to so it moves with it
                transform.position = hits[0].point;
                gameObject.transform.SetParent(hits[0].transform);

                // Velocity is reset here, but it now will start pulling the player to its location.
                velocity = deacceleration * Time.deltaTime * direction;
                deacceleration *= grappleDamp;
            }

            if(objectType == "Small Object" || (objectType == "Untagged" && hits[0].transform.gameObject.tag == "Small Object"))
            {
                heldObject = hits[0].collider.transform.gameObject;
                heldconf = heldObject.transform.GetComponentInParent<CharacterConfig>();
                grabUsed = true;

                if(heldconf != null && !heldconf.grabbable)
                {
                    heldObject = null;
                    velocity = Vector3.zero;
                    StopExtending();
                    return;
                }
                else if(heldconf != null)
                {
                    heldconf.invincible = true;
                    heldconf.grabbed = true;
                }
                // StopExtending();
                SetEndpoint();

                // Tongue moves to object position, object becomes a child of tongue.
                transform.position = heldObject.transform.position;
                ResizeTongueBody();
                heldObject.transform.SetParent(gameObject.transform);

                if(heldconf != null)
                {
                    heldconf.invincible = true;
                    heldconf.grabbed = true;
                }

                //Lose some momentum when hitting object
                velocity *= objectResidualSpeed;

                //Start timer
                //criticalTimer = 0;
                //criticalUsed = false;
            }
        }
    }

    //also will continually have to change the endpoint
    public void SetEndpoint()
    {
        //finalpos = transform.position;
    }


    // If player lets go tongue must retract
    public void UnGrab()
    {
        grabbed = false;

        // Change parent back to player
        gameObject.transform.SetParent(config.gameObject.transform);
    }


    // Enable method, called every time the tongue is enabled.
    public void OnEnable()
    {
        // Direction & position of tongue
        direction = InputManager.Instance.Aim;
        
        // Internal settings
        transform.SetPositionAndRotation(TongueOrigin, Quaternion.identity);
        prevPos = transform.position;
        extending = true;
        IsFinished = false;
        grabUsed = false;
        deacceleration = 2 * tongueLength / Mathf.Pow(timeToExtend, 2);
        velocity = direction * deacceleration * timeToExtend;

        // Specifically for tongue pulling
        startedSwinging = false;
        pullSpeed = 0f;
        inputVelocity = Vector3.zero;

        // Tongue bodies
        tongueBody.transform.rotation = Quaternion.identity;
        float angle = Vector3.Angle(direction, Vector3.right);
        if (direction.y < 0) angle = -angle;
        tongueBody.transform.Rotate(0f, 0f, angle);
        tongueCollider.enabled = true;

        // Rotate player (and put sprite in correct place
        config.RotateSprite(direction);
        config.playerAnimator.UpdateIdleAnimation();
        if (config.currentdir != 3)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            tongueBody.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        } else
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "PlayerFeatures";
            tongueBody.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerFeatures";

        }
        // Finishing
        ResizeTongueBody();
        tongueBody.GetComponent<SpriteRenderer>().enabled = true;
    }


    // Disable method, called every time the tongue is disabled.
    void OnDisable()
    {
        tongueBody.GetComponent<SpriteRenderer>().enabled = false;
        grabUsed = false;
        tongueCollider.enabled = false;

        if(heldObject != null)
        {
            HealthDrop holdinghealth = heldObject.transform.GetComponent<HealthDrop>();
            if(holdinghealth != null)
            {
                holdinghealth.Eat(config);
                holdingObject = false;
            }
            else
            {
                holdingObject = true;
            }
        }
    }


    // Resizes the tongue's width and everything.
    public void ResizeTongueBody()
    {
        tongueBody.transform.position = (transform.position + TongueOrigin) / 2;
        //direction = transform.position - TongueOrigin;
        //direction.Normalize();

        if (grabUsed)
        {
            tongueBody.transform.rotation = Quaternion.identity;
            direction = transform.position - TongueOrigin;
            direction.Normalize();
            float angle = Vector3.Angle(direction, Vector3.right);
            if (direction.y < 0) angle = -angle;
            tongueBody.transform.Rotate(0f,0f,angle);
        }

        float currentLength = Vector3.Distance(transform.position, TongueOrigin);
        //tongueBody.transform.localScale = new Vector3(currentLength, (.12f - .06f * (tongueBody.transform.localScale.x / (5*tongueLength))), transform.localScale.z);
        tongueBody.transform.localScale = new Vector3(currentLength, (.12f - .025f * (tongueBody.transform.localScale.x / (5*tongueLength))), transform.localScale.z);

    }


    // Stops tongue extending
    private void StopExtending()
    {
        extending = false;
        SetEndpoint();
    }

    // Removes player's input velocity. Called when we enter flying state.
    public void RemovePlayerInputForce()
    {
        velocity = velocity.magnitude * -direction;
    }


    // Checks if tongue is finished
    public bool CheckIfFinished()
    {
        //bool finished = false;
        //if (!extending) finished = (config.transform.position - transform.position).magnitude - distTraveled < 0;
        //return finished;

        bool finished = false;

        if (!extending)
        {
            Vector3 start = transform.position;
            Vector3 end = prevPos;
            LayerMask mask = LayerMask.GetMask("Player Hurtbox");

            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, mask);
            if (hits.Length == 0) return finished;
            if (hits[0].collider.transform.gameObject.CompareTag("Player")) finished = true;
        }
        return finished;
    }


    // Gets axis of tongue for angle purposes (+x, -x, +y, -y)
    private Vector3 GetAxis(int dir) 
    {
        switch(dir) 
        {
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