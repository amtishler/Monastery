using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float enemyCheck = 1f;
    public bool enemiesDefeated = false;

    private void Awake() {
        GetComponent<CameraController>().isCutscene = false;
        foreach (var e in enemies) {
            EnemyConfig enemy = e.GetComponent<EnemyConfig>();
            if (enemy == null) Debug.LogError("Not a character");
            enemy.detectionradius = 0f;
        }
    }

    private void CheckEnemies() {
        int count = 0;
        foreach (var e in enemies) {
            CharacterConfig enemy = e.GetComponent<CharacterConfig>();
            if (enemy == null) Debug.LogError("Not a character");
            else if (!enemy.dead) break;
            ++count;
        }
        if (count == enemies.Length) enemiesDefeated = true;
    }

    public void CheckTimer() {
        --enemyCheck;
        if (enemyCheck <= 0f) {
            CheckEnemies();
            if(!enemiesDefeated) enemyCheck = 1f;
        }
    }

    public void ActivateEnemies() {
        foreach (var e in enemies) {
            EnemyConfig enemy = e.GetComponent<EnemyConfig>();
            if (enemy == null) Debug.LogError("Not a character");
            enemy.detectionradius = 6f;
        }
    }
}
