using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KickController : Attack {
    
    [SerializeField] BoxCollider2D kickCollider;

    [SerializeField] float kickTime;
    [SerializeField] float kickWidth;
    [SerializeField] float kickHeight;
    private Vector3 direction;
    private int[][] directionList = new int[4][];
    private float timeUp = 0f;


    // Awake is called before the first frame update
    void Awake() {
        directionList[0] = new int[] {1,0};
        directionList[1] = new int[] {0,1};
        directionList[2] = new int[] {-1,0};
        directionList[3] = new int[] {0,-1};
        transform.localScale = new Vector3(kickHeight, kickWidth, transform.localScale.z);
    }

    // Enable method, called every time player swings staff.
    void OnEnable() {
        // Mouse direction calculation
        transform.position = playerconf.transform.position;
        direction = GetComponentInParent<PlayerConfig>().Input.Aim;

        // picking direction
        int angle = (int)Vector3.Angle(direction, Vector3.right);
        if (direction.y < 0)
            angle = 180 + (int)Vector3.Angle(direction, Vector3.left);
        angle = (angle+45)/90;
        if (angle > 3) angle = 0;

        // Resetting staff position/rotation
        int[] key = directionList[angle];
        float x = transform.position.x + key[0] + (kickHeight-1)/2 * key[0];
        float y = transform.position.y + key[1] + (kickHeight-1)/2 * key[1];
        if (angle == 3) y = y-0.5f;
        transform.Rotate(0f,0f,(angle)*90);
        transform.position = new Vector3(x,y,0f);
        kickCollider.enabled = true;
    }

    // Disable method, called when staff is disabled.
    void OnDisable() {
        timeUp = 0f;
        transform.rotation = Quaternion.identity;
        kickCollider.enabled = false;
    }


    // Update is called once per frame
    public void UpdateKick() {
        timeUp = timeUp + Time.deltaTime;
    }

    // Whether or not staff is done
    public bool Done() {
        return timeUp >= kickTime;
    }
}
