using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour {
    
    public PlayerController player;
    private BoxCollider2D staffCollider;

    [SerializeField] float staffSwingTime;
    [SerializeField] float staffWidth;
    [SerializeField] float staffHeight;
    private Vector3 direction;
    private int[][] directionList = new int[4][];
    private float timeUp = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        staffCollider = gameObject.GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        directionList[0] = new int[] {1,0};
        directionList[1] = new int[] {0,1};
        directionList[2] = new int[] {-1,0};
        directionList[3] = new int[] {0,-1};
        transform.localScale = new Vector3(staffHeight, staffWidth, transform.localScale.z);

    }


    // Enable method, called every time player swings staff.
    void OnEnable()
    {
        // Mouse direction calculation
        transform.position = player.transform.position;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        direction = mouse - transform.position;
        direction.Normalize();

        // picking direction
        int angle = (int)Vector3.Angle(direction, Vector3.right);
        if (direction.y < 0)
            angle = 180 + (int)Vector3.Angle(direction, Vector3.left);
        angle = (angle+45)/90;
        if (angle > 3) angle = 0;

        // Resetting staff position/rotation
        int[] key = directionList[angle];
        float x = transform.position.x + key[0] + (staffHeight-1)/2 * key[0];
        float y = transform.position.y + key[1] + (staffHeight-1)/2 * key[1];
        if (angle == 3) y = y-0.5f;
        transform.Rotate(0f,0f,(angle)*90);
        transform.position = new Vector3(x,y,0f);
    }

    void OnDisable()
    {
        timeUp = 0f;
        transform.rotation = Quaternion.identity;
    }


    // Update is called once per frame
    void Update()
    {
        if (timeUp < staffSwingTime) timeUp = timeUp + Time.deltaTime;
        else player.ReturnStaff();
    }
}
