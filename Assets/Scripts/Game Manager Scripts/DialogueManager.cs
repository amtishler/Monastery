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
    private TMP_Text speaker;
    private TMP_Text body;


    // Awake Method, when game starts
    private void Awake() {
        speaker = transform.Find("Speaker").GetComponentInChildren<TMP_Text>();
        body = transform.Find("Body").GetComponentInChildren<TMP_Text>();
        _instance = this;
    }


    // Plays a dialogue
    public void Play(TextAsset dialogue) {

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
            string val = match.Value;
            string tag = tagMatcher.Match(val).Value;
            string text = textMatcher.Match(val).Value;
            if (tag == "<c>") {
                currentSpeaker = text;
                continue;
            } else {
                currentBody = text;
                StartCoroutine(RunDialogueBox(currentSpeaker, currentBody));
            }
        }
    }


    // Runs a dialogue box
    private IEnumerator RunDialogueBox(string currentSpeaker, string currentBody) {
        speaker.text = currentSpeaker;
        body.text = "";

        foreach(char c in currentBody) {
            body.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
