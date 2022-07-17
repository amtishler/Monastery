using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour{
    private static DialogueManager _instance;
    public static DialogueManager Instance {
        get {
            if (_instance == null) Debug.Log("Dialogue Manager is Null");
            return _instance;
        }
    }
    private GameObject dialogueBox;
    private TMP_Text speaker;
    private TMP_Text body;
    private GameObject player;
    [SerializeField] float dialogueSpeed;

    // Coroutine variables
    private bool running;
    private bool textCrawling;
    public bool Running {get {return running;}}


    // Awake Method, when game starts
    private void Awake() {
        dialogueBox = transform.Find("DialogueBox").gameObject;
        speaker = dialogueBox.transform.Find("Speaker").GetComponentInChildren<TMP_Text>();
        body = dialogueBox.transform.Find("Body").GetComponentInChildren<TMP_Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        running = false;
        textCrawling = false;
       _instance = this;
    }


    // Starts a coroutine without actually being a coroutine itself.
    // Notice that running is set to true outside of the corroutine but needs
    // to be set to false from inside the coroutine.
    public void Play(TextAsset dialogue) {
        if (running == true) {return;}
        running = true;
        player.GetComponent<PlayerStateMachine>().BeginCutscene();
        dialogueBox.SetActive(true);
        StartCoroutine(RunDialogue(dialogue));
    }


    // Plays a dialogue
    private IEnumerator RunDialogue(TextAsset dialogue) {

        string content = dialogue.ToString();
        content = content.Replace(System.Environment.NewLine, " ");
        content = content.Replace("\t", "");

        Regex itemMatcher = new Regex("<(c|t)>.+?(?=<(c|t|e)>)");   // matches tag + item and everything before tag
        Regex tagMatcher = new Regex("<(c|t|e)>");                  // matches  tag
        Regex textMatcher = new Regex(@"(?<=(<(c|t)>\s+))(.*$)");   // matches everything after tag
        MatchCollection matches = itemMatcher.Matches(content);

        string currentSpeaker = "";
        string currentBody = "";

        foreach(Match match in matches) {
            Debug.Log(match.Value);
        }

        foreach(Match match in matches) {
            Debug.Log("here");
            string val = match.Value;
            string tag = tagMatcher.Match(val).Value;
            string text = textMatcher.Match(val).Value;
            if (tag == "<c>") {
                currentSpeaker = text;
                continue;
            } else {
                currentBody = text;
                yield return StartCoroutine(RunDialogueBox(currentSpeaker, currentBody));
            }
        }

        dialogueBox.SetActive(false);
        running = false;
        player.GetComponent<PlayerStateMachine>().EndCutscene();
        yield return 0;
    }


    // Runs a dialogue box
    private IEnumerator RunDialogueBox(string currentSpeaker, string currentBody) {

        speaker.text = currentSpeaker;
        body.text = "";
        float speed = dialogueSpeed;
        bool running = true;

        Coroutine crawl = StartCoroutine(TextCrawl(currentBody));
        textCrawling = true;

        yield return 0;  // to clear the player input;

        while (textCrawling) {
            if (InputManager.Instance.InteractPressed) {
                StopCoroutine(crawl);
                textCrawling = false;
                body.text = currentBody;
            }
            yield return 0;
        }
        
        while (!InputManager.Instance.InteractPressed) {
            yield return 0;
        }
    }

    private IEnumerator TextCrawl(string currentBody) {
        foreach(char c in currentBody) {
            body.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        textCrawling = false;
    }
}
