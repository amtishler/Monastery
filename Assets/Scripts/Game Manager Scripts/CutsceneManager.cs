using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour{
    private static CutsceneManager _instance;
    public static CutsceneManager Instance {
        get {
            if (_instance == null) Debug.Log("Dialogue Manager is Null");
            return _instance;
        }
    }
    [SerializeField] List<Cutscene> sceneList = new List<Cutscene>();

    // Private Fields
    private PlayerConfig config;
    private bool running;


    // Awake Method, when game starts
    void Awake() {
        _instance = this;
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
        running = false;
        foreach(Cutscene scene in sceneList) {
            scene.played = false;
            foreach(CutsceneEvent cutsceneEvent in scene.events) {
                if (cutsceneEvent.speaker == null) return;
                DontDestroyOnLoad(cutsceneEvent.speaker);
            }
        }
    }


    // Called every frame
    void Update() {
        if (!running) {
            foreach(Cutscene scene in sceneList) {
                if (Vector3.Distance(config.transform.position, scene.originPoint) < scene.triggerRadius && !scene.played) {
                    StartCoroutine(scene.Play());
                    break;
                }
            }
        }
    }

    // Called by cutscene classes to start cutscene
    public void BeginCutscene() {
        running = true;
        config.GetComponent<PlayerStateMachine>().BeginCutscene();
    }

    // Called again to end cutscene
    public void EndCutscene() {
        running = false;
        config.GetComponent<PlayerStateMachine>().EndCutscene();
    }
}
