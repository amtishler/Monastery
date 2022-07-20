using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour {

    // Serialized fields
    [SerializeField] GameObject body;
    [SerializeField] GameObject text;
    [SerializeField] Sprite[] sprites = new Sprite[4];
    [SerializeField] Vector2 padding;

    // Private fields
    private float min_width = 2f;
    private float max_width = 6f;
    private float min_height = 0.85f;
    private float max_height = 2.7f;
    private bool textCrawling=false;
    private int absPivot;


    // Runs a box
    public IEnumerator Run(string completeText, NewCutscene cutscene) {
        // Setting up
        TMP_Text textMesh = text.GetComponent<TMP_Text>();

        // Resizing dialogue box
        Scale(completeText);
        textMesh.text = "";

        // Running dialogue
        Coroutine crawl = cutscene.StartCoroutine(TextCrawl(completeText, cutscene) );
        textCrawling = true;

        while (textCrawling) {
            if (InputManager.Instance.AdvancePressed) {
                StopCoroutine(crawl);
                textCrawling = false;
                textMesh.text = completeText;
            } yield return 0;
        }

        while (!InputManager.Instance.AdvancePressed) yield return 0;
        yield return 0;
    }


    // Helper method to make the text scoll by
    private IEnumerator TextCrawl(string completeText, NewCutscene cutscene) {
        TMP_Text textMesh = text.GetComponent<TMP_Text>();
        foreach(char c in completeText) {
            text.GetComponent<TMP_Text>().text += c;
            yield return new WaitForSeconds(cutscene.TextScrollSpeed);
        }
        textCrawling = false;
        yield return 0;
    }


    // Scales the dialogue box. pivots: 0=top left...3=bottom right.
    public void Rotate(int pivotIndex) {
        absPivot = pivotIndex;
        Vector2 pivot = Vector2.zero;
        switch(pivotIndex) {
            case 0: pivot = new Vector2(0,1); break;
            case 1: pivot = new Vector2(1,1); break;
            case 2: pivot = new Vector2(0,0); break;
            case 3: pivot = new Vector2(1,0); break;
        }
        body.GetComponent<RectTransform>().pivot = pivot;
        body.GetComponent<Image>().sprite = sprites[pivotIndex];
    }


    // Resizes unity window
    public void Scale(string completeText) {
        TMP_Text textMesh = text.GetComponent<TMP_Text>();
        textMesh.text = completeText;
        float height = textMesh.preferredHeight;
        if (height < min_height) height = min_height;
        if (height > max_height) height = max_height;
        float width = textMesh.preferredWidth;
        if (width < min_width) width = min_width;
        if (width > max_width) width = max_width;
        Vector2 sizeDelta = new Vector2(width, height);

        RectTransform bodyRect = body.GetComponent<RectTransform>();
        RectTransform textRect = text.GetComponent<RectTransform>();
        bodyRect.sizeDelta = sizeDelta + padding;
        textRect.sizeDelta = sizeDelta;
        Vector3 shift = new Vector3(bodyRect.sizeDelta.x/2, bodyRect.sizeDelta.y/2, 0);
        if (absPivot == 0) shift.y = -shift.y;
        if (absPivot == 1) shift = -shift;
        if (absPivot == 3) shift.x = -shift.x;
        textRect.position = bodyRect.position + shift;
    }
}
