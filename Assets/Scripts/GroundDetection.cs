using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CompositeCollider2D))]

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D ground;
    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private Rigidbody2D projectile;
    private bool isGrounded;

    private void Awake() {
        ground = GetComponent<CompositeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D hitbox) {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D hitbox) {
        CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
        if (character != null && !isGrounded) {
            character.grounded = false;
            Debug.Log("**Exit Detected**");
        } else {
            // projectile = hitbox.gameObject.GetComponent<Rigidbody2D>();
            // projectile.gravityScale += 1f;
            // projectile.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Default";
            Debug.Log("**character is null**");
        }
    }
}
