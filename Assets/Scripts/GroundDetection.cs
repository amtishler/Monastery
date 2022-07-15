using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CompositeCollider2D))]

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D ground;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isGrounded;


    private void Awake() {
        ground = GetComponent<CompositeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
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
            Debug.Log("**character is null**");
        }
    }

    // private void OnTriggerStay2D(Collider2D hitbox) {
    //     CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
    //     if (character != null) {
    //         onGround = true;
    //     }
    // }

        // private void OnTriggerExit2D(Collider2D hitbox) {
        //     CharacterConfig character = hitbox.gameobject.GetComponent<CharacterConfig>();
        //     if (character != null) {
        //         character.grounded = false;
        //         Debug.Log("**Exit Detected**");
        //     } else {
        //         Debug.Log("**character is null**");
        //     }
        // }
}
