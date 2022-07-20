using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeGenerator : MonoBehaviour
{
    void Start()
    {
        foreach (PolygonCollider2D c in this.gameObject.GetComponentsInChildren<PolygonCollider2D>()) c.usedByComposite = true;
    }
}
