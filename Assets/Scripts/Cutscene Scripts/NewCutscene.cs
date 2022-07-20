using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCutscene : MonoBehaviour {

    // Serialized Fields
    [SerializeField] float startDelay=1.5f;
    [SerializeField] float endDelay=1.5f;
    // [SerializeField] Vector2 playerPosition;
    [SerializeField] float textScrollSpeed;
    [SerializeField] List<NewDialogue> dialogues = new List<NewDialogue>();

    // Private Fields
    private PlayerConfig config;
    private bool running = false;
    private bool completed = false;
    
    // Getters & Setters
    public float TextScrollSpeed {get {return textScrollSpeed;}}


    void Awake() {
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
    }


    void OnTriggerEnter2D(Collider2D other) {
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
        yield return new WaitForSeconds(startDelay);
        foreach(NewDialogue dialogue in dialogues) {
            yield return dialogue.Play(this);
        }
        yield return new WaitForSeconds(endDelay);

        // Ending
        config.GetComponentInParent<PlayerStateMachine>().EndCutscene();
        running=false;
        gameObject.SetActive(false);
    }
}