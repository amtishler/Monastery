using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCollider : MonoBehaviour
{
    [SerializeField] 
    private Vector2 boxSize;

    [SerializeField]
    private MusicTrigger trigger;

    private BoxCollider2D box;
    private Rigidbody2D rig;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        box.isTrigger = true;
        box.size = boxSize;
        rig.isKinematic = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Music Trigger");

            MusicManager.Instance.HandleTrigger(trigger);
        }
    }



}
