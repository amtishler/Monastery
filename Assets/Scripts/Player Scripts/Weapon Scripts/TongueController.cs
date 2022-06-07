using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    // Serialized Fields
    [Header("Refernces")]
    [SerializeField] private CircleCollider2D tongueCollider;
    [SerializeField] PlayerConfig player;
    [SerializeField] GameObject tongueBody;
    [Header("Tongue Variables")]
    [SerializeField] float timeToExtend = 0.5f;
    [SerializeField] float tongueLength = 8f;
    [SerializeField] float retractionAccelerateFactor = 1f;
    [SerializeField] float autoRetractionAngle = 90f;
    [Header("Object Interactions")]
    [SerializeField] float objectSpeedReduction = 0.8f;
    [SerializeField] float buttonSpeedReduction = 0.8f;
    [SerializeField] float playerMoveSpeed = 3f;
    [SerializeField] float playerMoveSpeedHeavy = 2f;

    private Vector3 tongueSpawnPoint;
    private float deacceleration;
    private float speed;
    private Vector3 velocity;
    private Vector3 direction;
    private float distTraveled;
    private float totalTime = 0f;
    private bool extending;


    public void UpdateTongue() {
        // Checking if tongue is still going out
        if (extending) {
            if (!Input.GetMouseButton(1)) {
                speed = deacceleration*Time.deltaTime;
                extending = false;
                deacceleration = deacceleration*retractionAccelerateFactor*buttonSpeedReduction;
                speed = deacceleration*Time.deltaTime;
            }
            if (speed <= 0) {
                extending = false;
                deacceleration = deacceleration*retractionAccelerateFactor;
            }
        }

        velocity = direction*speed;
        distTraveled = distTraveled+speed*Time.deltaTime;
        speed = speed - deacceleration*Time.deltaTime;

        // if (distTraveled <= 0) player.ReturnTongue();
        transform.position = transform.position + velocity*Time.deltaTime;
        ResizeTongueBody();
    }


    // Enable method, called every time the tongue is enabled.
    public void OnEnable() {
        // Direction of tongue
        tongueSpawnPoint = player.transform.position;
        tongueSpawnPoint.y = tongueSpawnPoint.y + 0.11f;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        direction = mouse - tongueSpawnPoint;
        direction.Normalize();

        // Internal settings
        transform.position = tongueSpawnPoint;
        transform.rotation = Quaternion.identity;
        totalTime = 0f;
        extending = true;
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
        player.RotateSprite(direction);
    }


    // Disable method, called every time the tongue is disabled.
    void OnDisable() {
        tongueBody.SetActive(false);
        tongueCollider.enabled = false;
    }


    // Resizes the tongue's width and everything.
    private void ResizeTongueBody() {
        tongueBody.transform.position = (transform.position + tongueSpawnPoint) / 2;
        float currentLength = Vector3.Distance(transform.position, tongueSpawnPoint);
        tongueBody.transform.localScale = new Vector3(currentLength*5, 5*(.12f - .06f * (tongueBody.transform.localScale.x / (5*tongueLength))), transform.localScale.z);
    }


    // Checks whether or not tongue is finished.
    public bool Done() {
        return distTraveled <= 0 && !extending;
    }
}