using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    [SerializeField] TextAsset dialogue;
    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5) {
            if (InputManager.Instance.InteractPressed) {
                DialogueManager.Instance.Play(dialogue);
            }
        }
    }
}
