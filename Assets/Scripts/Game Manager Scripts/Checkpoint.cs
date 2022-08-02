using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public GameObject[] enemiesToRespawn;
    public Transform location;
    public bool activeCheckpoint;
    BoxCollider2D box;
    Rigidbody2D rig;


    private void Awake() {
        box = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        box.isTrigger = true;
        rig.isKinematic = true;
        location = this.gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D player) {
        if (player.gameObject.CompareTag("Player")) {
            foreach (var c in GameManager.Instance.checkpoints) {
                c.GetComponent<Checkpoint>().activeCheckpoint = false;
            }
            activeCheckpoint = true;
            player.gameObject.GetComponent<CharacterConfig>().respawnPoint = location.position;
        }
    }
}
