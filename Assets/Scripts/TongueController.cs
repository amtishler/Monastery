using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    private CircleCollider2D tongueCollider;
    private Vector3 direction = Vector3.zero;
    // private float speed = 0.0f;
    // private float maxDistance;
    private float amplitude;
    private float period;
    private float startTime;
    private float currentDistance = 0;
    private bool returned = false;

    public PlayerController player;
    


    // Start method, called once per frame.
    void Start() {
        tongueCollider = gameObject.GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        startTime = Time.time;
    }

    // Update method. Moves tongue.
    void Update() {

        float cos = amplitude * Mathf.Cos(period*(Time.time - startTime));
        float sin = amplitude * Mathf.Sin(period*(Time.time - startTime));
        Vector3 deltaDist = direction*cos*Time.deltaTime;

        if (!returned) {
            transform.position = transform.position - deltaDist;
            if (cos < 0 || sin > 0) {
                returned = true;
            }
        } else {
            transform.position = transform.position + deltaDist;
            if (sin < 0) {
                player.ReturnTongue();
                Debug.Log(Time.time - startTime);
                Destroy(gameObject);
            }
        }
    }


    // Method called by the PlayerController to set the tongue's fields.
    // direction: Vector3 tells us where we're going.
    // speed: float tells us how fast tongue shoots.
    // distance: float tells us how far tongue goes.
    // public void setFields(Vector3 direction, float speed, float distance) {
    //     this.direction = direction;
    //     this.speed = speed;
    //     this.maxDistance = distance;
    // }

    public void setFields(Vector3 tongueDirection, float tongueTime, float tongueDist) {
        direction = tongueDirection;
        period = Mathf.PI/tongueTime;
        amplitude = tongueDist / ( (1/period)*Mathf.Sin( tongueTime*period/2 ) );
        Debug.Log(amplitude);
    }
}
