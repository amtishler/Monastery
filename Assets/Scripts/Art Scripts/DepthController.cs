using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour
{
    GameObject playerGO;
    [SerializeField]
    Vector3 midpoint;
    [SerializeField]
    bool debugMidpoint = false;
    public bool affectChildren = false;

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerGO.transform.position.y >= transform.position.y + midpoint.y) {
            sR.sortingLayerName = "Foreground";
        } else
        {
            sR.sortingLayerName = "Background";
        }

        if(affectChildren == true) {
            foreach (Transform child in transform) {
                child.GetComponent<SpriteRenderer>().sortingLayerName = sR.sortingLayerName;
                child.GetComponent<SpriteRenderer>().sortingOrder = sR.sortingOrder - 1;
            }
        }
    }

    void OnDrawGizmos()
    {
        if(debugMidpoint)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + midpoint, 0.1f);
        }
    }
}
