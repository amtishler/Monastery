using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropShadow : MonoBehaviour
{
    public Material shadowMat;
    public Vector3 offset;
    public GameObject original;
    public GameObject grid;
    public GameObject plantsGO;

    GameObject shadowGO;

    void Start()
    {
        shadowGO = GameObject.Instantiate(original);
        shadowGO.transform.SetParent(grid.transform, false);
        shadowGO.transform.position = shadowGO.transform.position + offset;
        shadowGO.GetComponent<TilemapRenderer>().material = shadowMat;
        shadowGO.GetComponent<TilemapRenderer>().sortingOrder = original.GetComponent<TilemapRenderer>().sortingOrder - 1;
        shadowGO.name = original.name + "_shadow";
        plantsGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
