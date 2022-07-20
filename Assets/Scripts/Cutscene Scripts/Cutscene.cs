using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using TMPro;


// Cutscene class
[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene", order = 100)]
public class Cutscene: ScriptableObject {

    [SerializeField] public float textScrollSpeed;
    [SerializeField] public float blackBarHeight;
    [SerializeField] public Vector3 originPoint;
    [SerializeField] public float triggerRadius;
    [SerializeField] public List<CutsceneEvent> events = new List<CutsceneEvent>();
    public bool played = false;

    public bool eventRunning;


    // Coroutine that kicks off the cutscene
    public IEnumerator Play() {
        played = true;
        eventRunning = false;
        CutsceneManager.Instance.BeginCutscene();
        foreach(CutsceneEvent cutsceneEvent in events) {
            if (!eventRunning) CutsceneManager.Instance.StartCoroutine(cutsceneEvent.Play(this));
            while (eventRunning) {yield return 0;}
        }
        CutsceneManager.Instance.EndCutscene();
    }
}


// Event class
[System.Serializable]
public class CutsceneEvent {

    /*
    Certain methods will be invisible in the editor, depending
    on the type.

    VALID TYPES OF EVENTS RIGHT NOW:
        "Dialogue"
        "Camera Move"
    */

    public string type;

    // For "Dialogue"
    [SerializeField] public GameObject speaker;
    private int rotation;
    public int Rotation {
        get {
            return rotation;
        }
        set {
            if (value >= 0 || value < 4) rotation = value;
        }
    }
    public List<string> textBoxes = new List<string>();

    // For "Camera Move"
    [SerializeField] public float transitionTime;
    [SerializeField] public Vector2 anchorPoint;
    [SerializeField] public float cameraSize;

    // private fields
    private bool textCrawling;


    // Coroutine that plays a specific cutscene event
    // We pass the cutscene object in wherever we go for the internal settings
    public IEnumerator Play(Cutscene cutscene) {
        cutscene.eventRunning = true;
        if (type == "Dialogue") {
            GameObject box = MakeDialogueBox();
            for(int i=0; i<textBoxes.ToArray().Length; i++) {
                yield return CutsceneManager.Instance.StartCoroutine(RunDialogueBox(box, textBoxes[i], cutscene));
            }
            GameObject.Destroy(box);
        }
        if (type == "Camera Move") {

            CinemachineVirtualCamera cam = CutsceneManager.Instance.GetComponent<CinemachineVirtualCamera>();
            // CameraSwitcher.SwitchCamera(cam);
            cam.Priority = 500;
            Debug.Log("here");
            Debug.Log(cam.transform.position);
            float size = Camera.main.orthographicSize;
            yield return new WaitForSeconds(10);
            cam.Priority = 0;
        }
        yield return new WaitForSeconds(0.1f);
        cutscene.eventRunning = false;
    }


    // Creates a dialogue box
    private GameObject MakeDialogueBox() {  
        GameObject ob = GameObject.Instantiate(Resources.Load("DialogueBox")) as GameObject;
        RectTransform obTransform = ob.GetComponent<RectTransform>();

        ob.GetComponent<DialogueBox>().Rotate(rotation);

        obTransform.position = speaker.transform.position;
        ob.transform.SetParent(speaker.transform);
        return ob;
    }


    // Runs a dialogue box (only for dialogue events) (can happen multiple times per event)
    private IEnumerator RunDialogueBox(GameObject ob, string completeText, Cutscene cutscene) {
        
        // Setting up
        GameObject body = ob.transform.Find("Body").gameObject;
        GameObject text = body.transform.Find("Text").gameObject;
        TMP_Text textMesh = text.GetComponent<TMP_Text>();

        // Resizing dialogue box
        ob.GetComponent<DialogueBox>().Scale(completeText);
        textMesh.text = "";

        // Running dialogue
        Coroutine crawl = CutsceneManager.Instance.StartCoroutine(TextCrawl(textMesh, completeText, cutscene));
        textCrawling = true;

        while (textCrawling) {
            if (InputManager.Instance.AdvancePressed) {
                CutsceneManager.Instance.StopCoroutine(crawl);
                textCrawling = false;
                textMesh.text = completeText;
            }
            yield return 0;
        }

        while (!InputManager.Instance.AdvancePressed) {
            yield return 0;
        }
        yield return 0;
    }

    // Helper function for crawling text
    private IEnumerator TextCrawl(TMP_Text textMesh, string completeText, Cutscene cutscene) {
        foreach(char c in completeText) {
            textMesh.text += c;
            yield return new WaitForSeconds(cutscene.textScrollSpeed);
        }
        textCrawling = false;
    }
} 