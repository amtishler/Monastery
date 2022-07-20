using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CompositeCollider2D))]

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D ground;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ProjectileObject projectile;
    private bool isGrounded;

    private void Awake() {
        ground = GetComponent<CompositeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D hitbox) {
        CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
        if (character != null) character.grounded = true;
    }

    private void OnTriggerStay2D(Collider2D hitbox) {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D hitbox) {
        CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
        ProjectileObject projectile = hitbox.gameObject.GetComponentInParent<ProjectileObject>();
        if (character != null && !isGrounded) {
            character.grounded = false;
            Debug.Log("**Exit Detected**");
        } else if (projectile != null && projectile.isProjectile) {
            projectile.gameObject.layer = 0;
            projectile.gameObject.GetComponent<Rigidbody2D>().gravityScale += projectile.gravity;
            projectile.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Default";
            //Debug.Log("Projectile fell");
        }
        //Debug.Log("**Projectile didn't fall**");
    }
}
