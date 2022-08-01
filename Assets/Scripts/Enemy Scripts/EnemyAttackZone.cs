using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    public bool playerentered;
    public GameObject playergameobj;

    void Start()
    {
        playerentered = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<PlayerConfig>() != null)
        {
            playerentered = true;
            playergameobj = col.gameObject;
        }
    }
}
