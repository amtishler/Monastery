using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Telescope : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circle;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        PlayerConfig player = other.gameObject.GetComponentInParent<PlayerConfig>();
        if (player != null) player.inTelescopeRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        PlayerConfig player = other.gameObject.GetComponentInParent<PlayerConfig>();
        if (player != null) player.inTelescopeRange = false;
    }
    
}
