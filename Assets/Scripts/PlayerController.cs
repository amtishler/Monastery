using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    //Footstep variables
    private RaycastHit foot;
    [SerializeField] private float rayDistance = 3.0f;
    [SerializeField] private float stepDistance = 3.0f;
    [SerializeField] private EventReference footstepsEventPath;
    [SerializeField] private string materialParameterName;
    public string[] materialTypes;
    private int F_materialValue;
    [HideInInspector] public int defaultMaterialValue;
    private float stepRandom;
    private Vector3 prevPos;
    private float distanceTravelled;

    //Movement variables
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {   
        controller = GetComponent<CharacterController>();
        stepRandom = Random.Range(0f, 0.5f);
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        //timeTakenSinceStep = Time.deltaTime;
        distanceTravelled += (transform.position - prevPos).magnitude;
        if(distanceTravelled >= stepDistance + stepRandom)
        {
            MaterialCheck();
            PlayFootsteps();
            stepRandom = Random.Range(0f, 0.5f);
            distanceTravelled = 0f;
        }
        prevPos = transform.position;
     
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.up * currentDir.y + transform.right * currentDir.x) * walkSpeed;
        
        controller.Move(velocity * Time.deltaTime);
    }

    void MaterialCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out foot, rayDistance))
        {
            Debug.Log(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue);
            if(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue == 1)
            {
                F_materialValue = foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue;
            }
            else if(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue == 2)
            {
                F_materialValue = foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue;
            }
            else
            {
                F_materialValue = defaultMaterialValue;
            }
        }
        else
        {
            F_materialValue = defaultMaterialValue;
        }
    }

    void PlayFootsteps()
    {
        EventInstance footsteps = RuntimeManager.CreateInstance(footstepsEventPath);
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform, GetComponent<Rigidbody>());
        footsteps.setParameterByName(materialParameterName, F_materialValue);
        footsteps.start();
        footsteps.release();
    }
}
