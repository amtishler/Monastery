using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropShadowNoGrid : MonoBehaviour
{
    public Material shadowMat;
    public Vector3 offset;
    public GameObject original;

    GameObject shadowGO;

    void Start()
    {
        shadowGO = GameObject.Instantiate(original);
        shadowGO.transform.SetParent(original.transform, false);
        shadowGO.transform.position = shadowGO.transform.position + offset;
        shadowGO.GetComponent<Renderer>().material = shadowMat;
        shadowGO.name = original.name + "_shadow";
        shadowGO.GetComponent<SpriteRenderer>().sortingOrder = original.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
