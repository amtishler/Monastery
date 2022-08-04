using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCollisionHandler : MonoBehaviour
{
    public GameObject hitbox;
    private HitboxController hitboxsettings;
    public float wallknockback = 5f;

    void Start()
    {
        hitboxsettings = hitbox.GetComponent<HitboxController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 knockbackdir = collision.transform.position - gameObject.transform.position;
        knockbackdir.Normalize();

        if(hitbox.layer == LayerMask.NameToLayer("Spit Projectile Hitbox"))
        {
            GameManager.Instance.DamageCharacter(this.gameObject.GetComponentInParent<CharacterConfig>(), hitboxsettings.selfdamage, hitboxsettings.selfstun, knockbackdir, wallknockback);
        }
    }
}
