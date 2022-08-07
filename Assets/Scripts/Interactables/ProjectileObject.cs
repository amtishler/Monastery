using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : InteractableObject
{

    public float damage, knockback, stun;
    public float knockbackreflected = 0.5f;


    public float projectilespeed = 10.0f;
    public bool isProjectile;
    private float duration = 0f;
    public float gravity;

    private static FMOD.Studio.EventInstance getHitSound = new FMOD.Studio.EventInstance();

    private void Awake()
    {
        getHitSound = FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Features/Rock/Get Hit");
        obj = this.gameObject;
        resetPosition = obj.transform.position;
    }

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
        if (this.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0) {
            duration += Time.deltaTime;
            if (duration >= 5f) {
                this.gameObject.SetActive(false);
                Debug.Log("Object Destroyed");
            }
        }
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        ApplyKnockback(dir, mag);
        getHitSound.start();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("hit");
        if(isProjectile)
        {
            CharacterConfig d = collision.transform.GetComponent<CharacterConfig>();
            //Debug.Log(d);
            if (d != null)
            {
                Debug.Log(d);
                // put knockback calculations here or something
                Vector3 knockbackdir = collision.transform.position - gameObject.transform.position;
                knockbackdir.Normalize();

                ApplyKnockback(-knockbackdir, knockback * knockbackreflected);
                GameManager.Instance.DamageCharacter(d, damage, stun, knockbackdir, knockback);
            }
        }
    }
}
