using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : StoryEvent
{
    [SerializeField] private GameObject[] enemies;
    private int enemiesLeft;
    public bool EnemiesDefeated { get { return enemiesLeft <= 0; } }

    protected override void _Awake() {
        enemiesLeft = enemies.Length;
        foreach (var e in enemies) {
            EnemyConfig enemy = e.GetComponent<EnemyConfig>();
            enemy.RegisterTracker(this);
            if (enemy == null) Debug.LogError("Not a character");
            enemy.detectionradius = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemiesLeft <= 0 && previousEvent != null) return;
        if (other.gameObject.CompareTag("Player")) BeginStoryEvent();
        Debug.Log("here");
    }

    public override void BeginStoryEvent() {
        cameraController.ActivateCamera();
        cameraController.FightingMusic();
        foreach (var e in enemies) {
            EnemyConfig enemy = e.GetComponent<EnemyConfig>();
            if (enemy == null) Debug.LogError("Not a character");
            enemy.detectionradius = 6f;
        }
    }

    public void DecreaseEnemies()
    {
        enemiesLeft--;
        Debug.Log(enemiesLeft);
        if (EnemiesDefeated)
        {
            CinemachineVirtualCamera cam = FindObjectOfType<PlayerConfig>().gameObject.GetComponent<CinemachineVirtualCamera>();
            Debug.Log("false");
            if (nextEvent != null) { Debug.Log("beginning next"); nextEvent.BeginStoryEvent(); }
            GetComponent<CameraController>().DeactivateCamera(cam);
        }
    }

    public void ResetEnemies() {
        enemiesLeft = enemies.Length;
        foreach (var e in enemies) {
            e.GetComponent<CharacterConfig>().Reset();
        }
    }
}
