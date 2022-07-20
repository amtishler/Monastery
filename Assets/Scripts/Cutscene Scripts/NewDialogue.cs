using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewDialogue {

    // Serialized Fields
    [SerializeField] GameObject speaker;
    [SerializeField] bool displayTextBox=true;
    enum myEnum {
        topLeft,
        topRight,
        bottomLeft,
        bottomRight,
    }
    [SerializeField] myEnum textBoxRotation = myEnum.bottomLeft;
    [TextArea(3,10)]
    public List<string> textBoxes = new List<string>();

    // Getters & Setters
    public int Pivot {get {
        switch(textBoxRotation) {
            case myEnum.topLeft: return 0;
            case myEnum.topRight: return 1;
            case myEnum.bottomLeft: return 2;
            case myEnum.bottomRight: return 3;
        }
        return 5000;
    }}

    // Kicks off our coroutine
    public IEnumerator Play(NewCutscene cutscene) {

        GameObject ob = GameObject.Instantiate(Resources.Load("DialogueBox")) as GameObject;
        RectTransform obTransform = ob.GetComponent<RectTransform>();

        ob.GetComponent<DialogueBox>().Rotate(Pivot);
        if (!displayTextBox) ob.GetComponentInChildren<Image>().color = Color.clear;

        obTransform.position = speaker.transform.position;
        ob.transform.SetParent(speaker.transform);

        foreach(string text in textBoxes) {
            yield return cutscene.StartCoroutine(ob.GetComponent<DialogueBox>().Run(text, cutscene));
        }
        GameObject.Destroy(ob);
        yield return 0;
    }
}
