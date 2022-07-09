using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmbienceCollider : MonoBehaviour
{
    [SerializeField]
    private AreaAmbience toPlay;

    [SerializeField]
    private Vector2 boxSize;

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
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hi");

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ambience Trigger");

            AmbienceManager.Instance.HandleTrigger(toPlay);
        }
    }


}
