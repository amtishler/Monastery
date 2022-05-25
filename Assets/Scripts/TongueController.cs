using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    private CircleCollider2D tongueCollider;
    private Vector3 direction = Vector3.zero;
    private float speed = 0.0f;
    private float maxDistance;
    private float currentDistance = 0;
    private bool returned = false;

    public PlayerController player;


    // Start method, called once per frame.
    void Start() {
        tongueCollider = gameObject.GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    // Update method, called once per frame.
    void Update() {
        Vector3 deltaDist = direction*speed*Time.deltaTime;
        currentDistance = currentDistance+deltaDist.magnitude;
        transform.position = transform.position + deltaDist;
        if (currentDistance >= maxDistance) {
            currentDistance = 0;
            direction = -direction;
            if (!returned) {
                returned = true;
            } else {
                player.ReturnTongue();
                Destroy(gameObject);
            }
        }

    }


    // Method called by the PlayerController to set the tongue's fields.
    // direction: Vector3 tells us where we're going.
    // speed: float tells us how fast tongue shoots.
    // distance: float tells us how far tongue goes.
    public void setFields(Vector3 direction, float speed, float distance) {
        this.direction = direction;
        this.speed = speed;
        this.maxDistance = distance;
    }
}
