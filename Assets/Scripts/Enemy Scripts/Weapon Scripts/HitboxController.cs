using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : Attack
{
    private void OnTriggerStay2D(Collider2D collision)
    {
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
    }
}
