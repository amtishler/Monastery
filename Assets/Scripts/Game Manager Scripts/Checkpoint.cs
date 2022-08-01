using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private EnemyConfig[] respawnEnemies;
    public Transform location;
    BoxCollider2D box;
    Rigidbody2D rig;
}
