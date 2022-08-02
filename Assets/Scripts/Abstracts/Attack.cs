using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Fields
    [SerializeField] protected CharacterConfig playerconf;
    [SerializeField] protected float damage, knockback, stun;
    [SerializeField] protected float knockbacknormalization = 0.5f;
    [SerializeField] protected float friction = 0.5f;
    protected bool done = false;

    // Getters & Setters
    public bool Done { get { return done; } set { done = value; } }
    public float Friction { get { return friction; } }

    // Gets direction
    protected Vector3 GetAxis(int dir)
    {
        switch (dir)
        {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.up;
            case 2:
                return Vector3.left;
            case 3:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }

    // Trigger enter, when attack hits something else
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //LIVING THINGS
        CharacterConfig d = collision.transform.GetComponent<CharacterConfig>();
        if (d != null)
        {
            // put knockback calculations here or something
            Vector3 knockbackdir = collision.transform.position - playerconf.transform.position;
            Vector3 facingdir = GetAxis(playerconf.currentdir);
            knockbackdir.Normalize();
            facingdir.Normalize();

            Vector3 finaldir = (knockbackdir * (1 - knockbacknormalization)) + (facingdir * knockbacknormalization);


            GameManager.Instance.DamageCharacter(d, damage, stun, finaldir, knockback);

            PlayerConfig p = playerconf.GetComponent<PlayerConfig>();

            if (p != null)
            {
                //p.
            }
        }

        //INANIMATE OBJECTS
        InteractableObject obj = collision.transform.GetComponent<InteractableObject>();
        if (obj != null)
        {
            Vector3 knockbackdir = collision.transform.position - playerconf.transform.position;
            Vector3 facingdir = GetAxis(playerconf.currentdir);
            knockbackdir.Normalize();
            facingdir.Normalize();

            Vector3 finaldir = (knockbackdir * (1 - knockbacknormalization)) + (facingdir * knockbacknormalization);
            obj.OnHit(knockbackdir, knockback);
        }
    }

    // Called when object is disabled
    private void OnDisable()
    {
        done = false;
    }
}
