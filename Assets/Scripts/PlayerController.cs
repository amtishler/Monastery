using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour {
    
    // Base variables
    private CircleCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private Dictionary<int, string> spriteDict;
    [SerializeField] private Sprite[] spriteList = new Sprite[4];
    private Camera mainCamera;

    // Movement variables
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    private Vector3 moveDelta;
    private Vector3 currentDir = Vector3.zero;
    private Vector3 currentDirVelocity = Vector3.zero;

    // Tongue variables
    public TongueController tongue;
    [SerializeField] float tongueTime = 1.0f;
    [SerializeField] float tongueDist = 1.0f;
    private Vector3 currentTongueTarget;
    private bool tongueOut = false;

    // Start method, called before the first frame update.
    void Start() {   
        playerCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteDict = new Dictionary<int, string>();
        mainCamera = Camera.main;
    }


    // Update method, called once per frame.
    void Update() {
        if (!tongueOut) {
            UpdateMovement();
            if (Input.GetMouseButtonDown(1)) ShootTongue();
        }
    }


    // Updates movement.
    void UpdateMovement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        if (targetDir == Vector3.zero) return;
        targetDir.Normalize();

        // moving
        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        Vector3 velocity = currentDir * walkSpeed;
        transform.Translate(velocity * Time.deltaTime);

        // update sprite
        int angle = (int)Vector3.Angle(targetDir, Vector3.right)/90;
        if (angle == 1 && targetDir.y < 0) angle = 3;
        spriteRenderer.sprite = spriteList[angle];
        
    }


    // Deploys tongue.
    void ShootTongue() {

        tongueOut = true;
        Vector3 mouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        Vector3 direction = mouse - transform.position;
        direction.Normalize();

        TongueController deployedTongue = Instantiate(tongue, transform.position, Quaternion.identity);
        deployedTongue.setFields(direction, tongueTime, tongueDist);
    }


    // Alerts us when the tongue is back in.
    public void ReturnTongue() {
        tongueOut = false;
    }
}
