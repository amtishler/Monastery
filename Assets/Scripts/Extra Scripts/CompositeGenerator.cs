using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeGenerator : MonoBehaviour
{
    private PolygonCollider2D[] colliders;
    void Start()
    {
        colliders = this.gameObject.GetComponentsInChildren<PolygonCollider2D>();
        foreach (PolygonCollider2D c in colliders) c.usedByComposite = true;
    }
}
