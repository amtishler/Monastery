using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float damage, knockback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterConfig d = collision.transform.root.GetComponent<CharacterConfig>();
        if (d != null)
        {
            // put knockback calculations here or something

            GameManager.Instance.DamageCharacter(d, damage, Vector2.right * knockback);
        }
    }
}
