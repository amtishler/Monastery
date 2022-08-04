using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeacefulWildlife : InteractableObject
{

    public float damage, knockback, stun;
    public float knockbackreflected = 0.5f;

    public float detectionradius = 1f;
    public float projectilespeed = 5.0f;
    public bool isProjectile;
    private float duration = 0f;
    public float gravity;

    private bool running = false;
    private bool hit = false;
    public float runningspeed = 10f;
    public float disappearrate = 0.1f;

    private Vector3 runningvector;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public Vector3 Velocity {get {return rigidBody.velocity;} set {rigidBody.velocity = value;}}
    public float Speed {get {return Velocity.magnitude;}}

    private static FMOD.Studio.EventInstance getHitSound = new FMOD.Studio.EventInstance();

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectionradius, LayerMask.GetMask("Player Hurtbox"));
        if(collider != null){
            running = true;
        }

        if(!running && speed != 0)
        {
            if(!hit) hit = true;
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
        if(hit && speed == 0 && !running) running = true;
        if(running)
        {
            animator.Play("Walking");
            RotateAway(player.transform.position);
            runningvector = -(player.transform.position - this.transform.position).normalized * runningspeed;
            Velocity = runningvector;
            spriteRenderer.color -= new Color(0, 0, 0, disappearrate * Time.deltaTime);
            if(spriteRenderer.color.a < 0) Destroy(this.gameObject);
        }
    }

    public void RotateAway(Vector3 targetDir) {
        Vector3 playervector = this.transform.position - targetDir;
        switch(Mathf.Sign(playervector.x)){
            case -1:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 1:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        ApplyKnockback(dir, mag);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("hit");
        if(isProjectile)
        {
            CharacterConfig d = collision.transform.GetComponent<CharacterConfig>();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
    }
}
