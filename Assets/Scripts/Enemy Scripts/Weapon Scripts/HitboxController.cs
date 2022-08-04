using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : Attack
{
    public float knockbackreflected = 0.25f;
    public float selfdamage = 10f;
    public float selfstun = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterConfig d = collision.transform.GetComponent<CharacterConfig>();
        //Debug.Log(d);
        if (d != null)
        {
            // put knockback calculations here or something
            Vector3 knockbackdir = collision.transform.position - gameObject.transform.position;
            knockbackdir.Normalize();

            GameManager.Instance.DamageCharacter(playerconf, selfdamage, selfstun, -knockbackdir, knockback * knockbackreflected);
            GameManager.Instance.DamageCharacter(d, damage, stun, knockbackdir, knockback);
        }
    }
}
