using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void BeginFalling()
    {
        boxCollider.enabled = false;
    }

    public void StopFalling()
    {
        boxCollider.enabled = true;
    }
}

