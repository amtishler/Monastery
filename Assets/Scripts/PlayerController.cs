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
    public GameObject tongueBody;
    public GameObject tongueSpawnPoint;
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
        targetDir.Normalize();

        // moving
        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        Vector3 velocity = currentDir * walkSpeed;
        transform.Translate(velocity * Time.deltaTime);

        // update sprite
        if (targetDir == Vector3.zero) return;
        int angle = (int)Vector3.Angle(targetDir, Vector3.right)/90;
        if (angle == 1 && targetDir.y < 0) angle = 3;
        spriteRenderer.sprite = spriteList[angle];
        
    }


    // Deploys tongue.
    void ShootTongue() {


        tongueOut = true;
        currentDir = Vector3.zero;

        Vector3 mouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        Vector3 direction = mouse - transform.position;
        Vector3 Unnormalized = direction;
        direction.Normalize();

        TongueController deployedTongue = Instantiate(tongue, tongueSpawnPoint.transform.position, Quaternion.identity);
        deployedTongue.setFields(direction, tongueTime, tongueDist);

        tongueBody.SetActive(true);

        int multiplier = 1;
        if (direction.y < tongueSpawnPoint.transform.position.y)
            multiplier = -1;

        Debug.Log("Tongue Spawn Point: " + tongueSpawnPoint.transform.position.ToString());
        Debug.Log("Unnormalized Mouse Vector: " + Unnormalized.ToString());
        Debug.Log("Normalized Mouse Vector: " + direction.ToString());
        Debug.Log("Unadjusted Angle: " + Vector3.Angle(tongueSpawnPoint.transform.position, Unnormalized));
        //Vector3 point = tongueSpawnPoint.transform.position - Unnormalized;
        //tongueBody.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg);
        //tongueBody.transform.rotation = Quaternion.Euler(0f, 0f, (multiplier * (180 - Mathf.Abs(Vector3.Angle(tongueSpawnPoint.transform.position, Unnormalized)))) - 12);
        Debug.Log("Adjusted Angle: " + tongueBody.transform.rotation.eulerAngles.z);
    }


    // Alerts us when the tongue is back in.
    public void ReturnTongue() {
        tongueOut = false;
        tongueBody.SetActive(false);
    }

    public float GetTongueDist()
    {
        return tongueDist;
    }
}
