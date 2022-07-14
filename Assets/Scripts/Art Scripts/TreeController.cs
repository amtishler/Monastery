using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    GameObject playerGO;
    [SerializeField]
    Vector3 treeMidpoint;

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerGO.transform.position.y >= transform.position.y + treeMidpoint.y) {
            sR.sortingLayerName = "Foreground Trees";
        } else
        {
            sR.sortingLayerName = "Background Trees";
        }
    }
}
