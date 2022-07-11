using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour {

    public GameObject TongueBody;

    private CircleCollider2D tongueCollider;
    private Vector3 direction = Vector3.zero;
    private float amplitude;
    private float period;
    private float startTime;
    private float maxLength;
    private bool returned = false;

    public PlayerController player;


    // Start method, called once per frame.
    void Start() {
        tongueCollider = gameObject.GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        TongueBody = GameObject.Find("TongueBody");
        startTime = Time.time;
    }


    // Update method. Moves tongue.
    void Update() {

        float cos = amplitude * Mathf.Cos(period*(Time.time - startTime));
        float sin = amplitude * Mathf.Sin(period*(Time.time - startTime));
        Vector3 deltaDist = direction*cos*Time.deltaTime;

        if (!returned) {
            if (cos < 0 || sin > 0) {
                returned = true;
            }
            transform.position = transform.position - deltaDist;
        } else {
            if (sin < 0) {
                player.ReturnTongue();
                Destroy(gameObject);
            }
            transform.position = transform.position + deltaDist;
        }

        resizeTongueBody();
    }


    // Sets distance, amplitude, period of tongue tip motion. Should be called only once.
    public void setFields(Vector3 tongueDirection, float tongueTime, float tongueDist) {
        direction = tongueDirection;
        period = Mathf.PI/tongueTime;
        amplitude = tongueDist / ( (1/period)*Mathf.Sin( tongueTime*period/2 ) );
        maxLength = tongueDist;
    }

    public void resizeTongueBody()
    {
        TongueBody.transform.position = (transform.position + player.tongueSpawnPoint.transform.position) / 2;
        TongueBody.transform.localScale = new Vector3(Vector3.Distance(transform.position, player.tongueSpawnPoint.transform.position), .12f - .06f * (TongueBody.transform.localScale.x / maxLength), transform.localScale.z);
    }

}