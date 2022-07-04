using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractableObject
{

    public GameObject drop;
    public int maxdrops;
    public bool dropused;

    public override void OnHit(Vector3 dir, float mag)
    {
        Debug.Log("Tree: Yeeeowch!");
    }
}
