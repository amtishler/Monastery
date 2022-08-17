using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceUnloader : MonoBehaviour
{
    const float threshold = 20f;
    GameObject player;
    float distanceToPlayer = 0;

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        player = GameObject.Find("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) {
            distanceToPlayer = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));

        if(distanceToPlayer > threshold) {
            sR.enabled = false;
        }
        else {
            sR.enabled = true;
        }
        }
        
    }
}
