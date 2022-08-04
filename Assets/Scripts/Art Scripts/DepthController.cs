using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour
{
    GameObject playerGO;
    [SerializeField]
    GameObject[] characters;
    Vector3 midpoint;
    [SerializeField]
    bool debugMidpoint = false;

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        characters = GameObject.FindGameObjectsWithTag("Small Object");
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
        foreach (var c in characters) {
            if(c.transform.position.y >= transform.position.y + midpoint.y) {
                c.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            } else
            {
                c.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
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
