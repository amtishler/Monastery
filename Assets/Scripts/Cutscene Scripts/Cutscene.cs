using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : StoryEvent {

    // Serialized Fields
    // [SerializeField] Vector2 playerPosition;
    [SerializeField] Trigger trigger;
    [SerializeField] List<CutsceneEvent> cutsceneEvents = new List<CutsceneEvent>();
    [SerializeField] GameObject combatEventTrigger;

    // Private Fields
    enum Trigger
    {
        enterZone = 0,
        defeatEnemies = 1
    }
    private PlayerConfig config;
    private bool running = false;
    private bool completed = false;


    protected override void _Awake()
    {
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
    }

    
    // Starts cutscene if enter zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (running || completed || previousEvent != null) return;
        if (other.gameObject.CompareTag("Player") && !running)
            BeginStoryEvent();
    }

    
    // For when enemies die
    public override void BeginStoryEvent()
    {
        cameraController.ActivateCamera();
        Debug.Log("activated camera");
        Debug.LogError("Activated Camera (error)");

        StartCoroutine(StartCutscene());
    }


    IEnumerator StartCutscene() {
        Debug.Log("started cutscene");
        Debug.LogError("Started Cutscene (error)");
        running = true;
        config.GetComponentInParent<PlayerStateMachine>().BeginCutscene();
        GetComponent<CameraController>().ActivateCamera();
        foreach(CutsceneEvent cutsceneEvent in cutsceneEvents) {
            yield return new WaitForSeconds(cutsceneEvent.StartDelay);
            yield return cutsceneEvent.Play(this);
            yield return new WaitForSeconds(cutsceneEvent.EndDelay);
        }

        // Ending
        StopAllCoroutines();
        GetComponentInParent<CameraController>().Finish(config.GetComponentInChildren<CinemachineVirtualCamera>());
        config.GetComponentInParent<PlayerStateMachine>().EndCutscene();
        completed = true;
        running=false;
        if (nextEvent != null) nextEvent.BeginStoryEvent();
    }
}