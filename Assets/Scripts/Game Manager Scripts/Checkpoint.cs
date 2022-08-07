using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public GameObject[] objectsToRespawn;
    [SerializeField] public GameObject[] zonesToReset;
    public Transform location;
    public bool activeCheckpoint;
    private bool oldCheckpoint;
    BoxCollider2D box;
    Rigidbody2D rig;


    private void Awake() {
        box = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        box.isTrigger = true;
        rig.isKinematic = true;
        location = this.gameObject.transform;
        oldCheckpoint = false;
    }

    private void OnTriggerEnter2D(Collider2D player) {
        if (player.gameObject.CompareTag("Player")) {
            foreach (var c in GameManager.Instance.checkpoints) {
                Checkpoint cp = c.GetComponent<Checkpoint>();
                if (cp.activeCheckpoint == true) {
                    cp.activeCheckpoint = false;
                    cp.gameObject.SetActive(false);
                    break;
                }
            }
            activeCheckpoint = true;
            box.enabled = false;
            player.gameObject.GetComponent<CharacterConfig>().respawnPoint = location.position;
        }
    }

    public void ResetZones() {
        CameraController zone;
        foreach (var z in zonesToReset) {
            zone = z.GetComponent<CameraController>();
            if (zone == null) Debug.LogError("Not A Combat Zone");
            zone.ResetZone();
        }
    }

    public void ResetObjects() {
        foreach (var o in objectsToRespawn) {
            if (o.GetComponent<ProjectileObject>() != null) o.GetComponent<ProjectileObject>().ResetPosition();
            if (o.GetComponent<CharacterConfig>() != null) o.GetComponent<CharacterConfig>().Reset();
        }
    }
}
