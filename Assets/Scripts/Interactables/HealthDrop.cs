using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{

    public float healthgain = 10f;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterConfig d = collision.transform.GetComponent<PlayerConfig>();
        CharacterConfig parent = this.transform.parent.transform.GetComponent<PlayerConfig>();
        if (d != null && parent != null)
        {
            d.Heal(healthgain);
            Destroy(this.gameObject);
        }
    }*/

    public void Eat(PlayerConfig player) {
        player.Heal(healthgain);
        Destroy(this.gameObject);
    }
}
