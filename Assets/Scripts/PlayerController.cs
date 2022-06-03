using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour {

    // Base variables
    private CircleCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    [SerializeField] private Sprite[] spriteList = new Sprite[4];

    // Movement variables
    [SerializeField] float maximumSpeed = 6.0f;
    [SerializeField] float minimumSpeed = 3.0f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float deacceleration = 0.1f;
    private Vector3 velocity = Vector3.zero;
    private float speed = 0f;

    // Weapon variables
    private bool busy = false;
    public GameObject tongue;
    public GameObject staff;


    // Start method, called before the first frame update.
    void Start()
    {   
        playerCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }


    // Update method, called once per frame.
    void Update() 
    {
        if (!busy)
        {
            UpdateMovement();
            if (Input.GetMouseButtonDown(1)) ShootTongue();
            if (Input.GetMouseButtonDown(0)) SwingStaff();
        }
    }


    // Updates movement.
    void UpdateMovement() 
    {
        // finding direction
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        targetDir.Normalize();

        // moving
        if (targetDir == Vector3.zero) 
        {
            if (speed < minimumSpeed) speed = 0;
            speed = speed - deacceleration;
            velocity.Normalize();
            velocity = velocity*speed;
        }
        else 
        {
            if (speed < minimumSpeed) speed = minimumSpeed;
            speed = speed + acceleration;
            if (speed > maximumSpeed) speed = maximumSpeed;
            velocity = targetDir*speed;
        }
        transform.Translate(velocity * Time.deltaTime);

        // update sprite
        if (targetDir == Vector3.zero) return;
        int angle = (int)Vector3.Angle(targetDir, Vector3.right);
        if (targetDir.y < 0)
            angle = 180 + (int)Vector3.Angle(targetDir, Vector3.left);
        angle = angle/90;
        spriteRenderer.sprite = spriteList[angle];
    }


    // Deploys tongue.
    void ShootTongue() 
    {
        busy = true;
        tongue.SetActive(true);
        velocity = Vector3.zero;
    }


    // Alerts us when the tongue is back in.
    public void ReturnTongue() 
    {
        busy = false;
        tongue.SetActive(false);
    }

    
    // Swings staff.
    void SwingStaff() 
    {
        busy = true;
        staff.SetActive(true);
    }


    // Returns staff.
    public void ReturnStaff()
    {
        busy = false;
        staff.SetActive(false);
        velocity = Vector3.zero;
    }
}
