using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    // Serialized Fields
    // [SerializeField] Vector2 playerPosition;
    [SerializeField] List<CutsceneEvent> cutsceneEvents = new List<CutsceneEvent>();

    // Private Fields
    private PlayerConfig config;
    private bool running = false;
    private bool completed = false;


    void Awake() {
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (completed) return;
        if (other.gameObject.CompareTag("Player") && !running) {
            // Beginning
            running = true;
            config.GetComponentInParent<PlayerStateMachine>().BeginCutscene();
            StartCoroutine(MovePlayer());
            StartCoroutine(StartCutscene());
        }
    }


    IEnumerator MovePlayer() {
        yield return 0;
    }


    IEnumerator StartCutscene() {
        foreach(CutsceneEvent cutsceneEvent in cutsceneEvents) {
            yield return new WaitForSeconds(cutsceneEvent.StartDelay);
            yield return cutsceneEvent.Play(this);
            yield return new WaitForSeconds(cutsceneEvent.EndDelay);
        }

        // Ending
        GetComponentInParent<CameraController>().Finish(config.GetComponentInChildren<CinemachineVirtualCamera>());
        config.GetComponentInParent<PlayerStateMachine>().EndCutscene();
        completed = true;
        running=false;
    }
}