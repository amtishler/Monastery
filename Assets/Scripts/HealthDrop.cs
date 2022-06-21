using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{

    public float healthgain = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterConfig d = collision.transform.GetComponent<PlayerConfig>();
        if (d != null)
        {
            Debug.Log("Here");
            d.Heal(healthgain);
            Destroy(this.gameObject);
        }
    }
}
