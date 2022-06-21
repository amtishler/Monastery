using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected CharacterConfig playerconf;
    [SerializeField] protected float damage, knockback;
    [SerializeField] protected float knockbacknormalization = 0.5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //LIVING THINGS
        CharacterConfig d = collision.transform.root.GetComponent<CharacterConfig>();
        if (d != null)
        {
            // put knockback calculations here or something
            Vector3 knockbackdir = collision.transform.position - playerconf.transform.position;
            Vector3 facingdir = GetAxis(playerconf.currentdir);
            knockbackdir.Normalize();
            facingdir.Normalize();

            Vector3 finaldir = (knockbackdir * (1 - knockbacknormalization)) + (facingdir * knockbacknormalization);


            GameManager.Instance.DamageCharacter(d, damage, finaldir, knockback);
        }

        //INANIMATE OBJECTS
        HittableObject obj = collision.transform.root.GetComponent<HittableObject>();
        if (obj != null)
        {
            Vector3 knockbackdir = collision.transform.position - playerconf.transform.position;
            Vector3 facingdir = GetAxis(playerconf.currentdir);
            knockbackdir.Normalize();
            facingdir.Normalize();

            Vector3 finaldir = (knockbackdir * (1 - knockbacknormalization)) + (facingdir * knockbacknormalization);
            obj.ApplyKnockback(knockbackdir, knockback);
        }
    }

    protected Vector3 GetAxis(int dir)
    {
        switch(dir)
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
}
