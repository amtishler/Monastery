using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    private CircleCollider2D tongueCollider;
    public PlayerController player;
    public GameObject tongueBody;

    [SerializeField] float travelTime = 1.0f;
    [SerializeField] float tongueLength = 5.0f;
    private Vector3 direction;
    private Vector3 tongueSpawnPoint;
    private float amplitude;
    private float period;
    private float extendPeriod;
    private float extendAmplitude;
    private float retractPeriod;
    private float retractAmplitude;
    private float totalTime = 0f;
    private float maxLength;


    // Start method, called once per frame.
    void Start()
    {
        tongueCollider = gameObject.GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        tongueBody = GameObject.Find("TongueBody");
    }


    // Enable method, called every time the tongue is enabled.
    public void OnEnable() 
    {
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
        period = Mathf.PI/(travelTime);
        amplitude = tongueLength / ( (1/period)*Mathf.Sin( travelTime*period/2) );


        // Tongue body
        tongueBody.transform.rotation = Quaternion.identity;
        float angle = Vector3.Angle(direction, Vector3.right);
        if (direction.y < 0) angle = -angle;
        tongueBody.transform.Rotate(0f,0f,angle);
        tongueBody.SetActive(true);
    }


    // Update method. Moves tongue.
    void Update()
    {
        float cos = amplitude * Mathf.Cos(period*totalTime);
        float sin = amplitude * Mathf.Sin(period*totalTime);
        Vector3 deltaDist = direction*cos*Time.deltaTime;
        totalTime += Time.deltaTime;

        if (cos > 0 && !Input.GetMouseButton(1))
        {
            totalTime = travelTime - totalTime;
            Debug.Log(totalTime);
        }
        if (sin < 0 && cos < 0) player.ReturnTongue();

        transform.position = transform.position + deltaDist;
        ResizeTongueBody();
    }


    // Disable method, called every time the tongue is disabled.
    public void OnDisable()
    {
        tongueBody.SetActive(false);
    }


    // Resizes the tongue's width and everything.
    public void ResizeTongueBody()
    {
        tongueBody.transform.position = (transform.position + tongueSpawnPoint) / 2;
        float currentLength = Vector3.Distance(transform.position, tongueSpawnPoint);
        tongueBody.transform.localScale = new Vector3(currentLength*5, 5*(.12f - .06f * (tongueBody.transform.localScale.x / (5*tongueLength))), transform.localScale.z);
    }

}
