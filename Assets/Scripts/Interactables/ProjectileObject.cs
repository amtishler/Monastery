using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : InteractableObject
{

    public float damage, knockback, stun;
    public float knockbackreflected = 0.5f;


    public float projectilespeed = 10.0f;
    private bool isProjectile;
    // private float duration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0)
        {
            SlowDown(deacceleration);
            if(!isProjectile && speed > projectilespeed)
            {
                isProjectile = true;
                gameObject.layer = LayerMask.NameToLayer("Spit Projectile Hitbox");
            }
            else if(isProjectile && speed < projectilespeed)
            {
                isProjectile = false;
                gameObject.layer = LayerMask.NameToLayer("Object Hurtbox");
            }
        }
        // if (this.gameObject.GetComponent<Rigidbody2D>().gravityScale == 1f) {
        //     duration += Time.deltaTime;
        //     if (duration >= 5f) Destroy(this);
        // }
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        ApplyKnockback(dir, mag);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isProjectile)
        {
            CharacterConfig d = collision.transform.root.GetComponent<CharacterConfig>();
            if (d != null)
            {
                // put knockback calculations here or something
                Vector3 knockbackdir = collision.transform.position - gameObject.transform.position;
                knockbackdir.Normalize();

                ApplyKnockback(-knockbackdir, knockback * knockbackreflected);
                GameManager.Instance.DamageCharacter(d, damage, stun, knockbackdir, knockback);
            }
        }
    }
}
