using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    // Serialized Fields
    // [SerializeField] Vector2 playerPosition;
    [SerializeField] Trigger trigger;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<CutsceneEvent> cutsceneEvents = new List<CutsceneEvent>();

    // Private Fields
    enum Trigger
    {
        enterZone = 0,
        defeatEnemies = 1
    }
    private PlayerConfig config;
    private bool running = false;
    private bool completed = false;
    private int enemiesLeft;


    void Awake() {
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
        enemiesLeft = enemies.Count;
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.parent = transform;
        }
    }

    
    // Starts cutscene if enter zone
    void OnTriggerEnter2D(Collider2D other) {
        if (completed || trigger != 0) return;
        if (other.gameObject.CompareTag("Player") && !running) {
            StartCoroutine(StartCutscene());
        }
    }


    // Starts cutscene when all enemies die
    public void EnemyDead()
    {
        if (completed) return;
        enemiesLeft -= 1;
        Debug.Log("dead");
        if (enemiesLeft == 0) StartCoroutine(StartCutscene());
    }


    IEnumerator StartCutscene() {
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
    }
}