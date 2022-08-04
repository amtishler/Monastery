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
    // private bool isGrounded;

    private void Awake() {
        ground = GetComponent<CompositeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // private void FixedUpdate() {
    //     isGrounded = false;
    // }

    // private void OnTriggerEnter2D(Collider2D hitbox) {
    //     CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
    //     if (character != null) character.grounded = true;
    // }

    private void OnTriggerStay2D(Collider2D hitbox) {
        CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
        if (character != null) character.grounded = true;
    }

    private void OnTriggerExit2D(Collider2D hitbox) {
        CharacterConfig character = hitbox.gameObject.GetComponentInParent<CharacterConfig>();
        ProjectileObject projectile = hitbox.gameObject.GetComponentInParent<ProjectileObject>();
        if (character != null && !character.grabbed) {
            character.grounded = false;
            Debug.Log(character);
        } else if (projectile != null && projectile.isProjectile) {
            Debug.Log(projectile);
            projectile.gameObject.layer = 0;
            projectile.gameObject.GetComponent<Rigidbody2D>().gravityScale += projectile.gravity;
            projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            Debug.Log(projectile);
            //Debug.Log("Projectile fell");
        }
        //Debug.Log("**Projectile didn't fall**");
    }
}
