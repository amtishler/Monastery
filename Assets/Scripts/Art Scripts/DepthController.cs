using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour
{
    GameObject playerGO;
    [SerializeField]
    Vector3 midpoint;

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
    }
}