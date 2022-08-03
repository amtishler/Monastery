using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutsceneEvent {

    // Serialized Fields
    [Header("Event Set-Up")]
    [SerializeField] float startDelay = 0f;
    [SerializeField] GameObject mainCharacter;
    [Header("Camera movement (these can be blank)")]
    [SerializeField] GameObject cameraFocus;
    [SerializeField] float cameraMoveTime = 0.1f;
    [Header("Character Movement")]
    [SerializeField] bool moveCharacter;
    [SerializeField] bool speakWhileMoving;
    [SerializeField] float moveTime = 0.1f;
    [SerializeField] GameObject destination;
    [Header("Dialogue Boxes")]
    [SerializeField] bool displayTextBox = true;
    enum myEnum {
        topLeft,
        topRight,
        bottomLeft,
        bottomRight,
    }
    [SerializeField] myEnum textBoxRotation = myEnum.bottomLeft;
    [SerializeField] float textScrollSpeed = 0.1f;
    [TextArea(3,10)]
    public List<string> textBoxes = new();
    [Header("Event Ending")]
    [SerializeField] float endDelay = 0f;

    // Private variables

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
    public float StartDelay { get { return startDelay; } }
    public float EndDelay { get { return endDelay; } }


    // Kicks off our coroutine
    public IEnumerator Play(Cutscene cutscene) {

        CinemachineVirtualCamera virtualCamera = cutscene.GetComponentInChildren<CinemachineVirtualCamera>();

        // Moving Camera
        if (cameraFocus != null)
        {
            float inTime = cameraMoveTime;
            if (inTime == 0f) inTime = 0.1f;
            Vector3 startPos = virtualCamera.gameObject.transform.position;
            Vector3 endPos = cameraFocus.gameObject.transform.position;
            endPos = new Vector3(endPos.x, endPos.y, startPos.z);

            for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
            {
                virtualCamera.gameObject.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, inTime, t));
                yield return null;
            }
        }

        if (moveCharacter)
        {
            if (speakWhileMoving) cutscene.StartCoroutine(MoveCharacter());
            else yield return cutscene.StartCoroutine(MoveCharacter());
        }

        GameObject ob = GameObject.Instantiate(Resources.Load("DialogueBox")) as GameObject;
        RectTransform obTransform = ob.GetComponent<RectTransform>();

        ob.GetComponent<DialogueBox>().Rotate(Pivot);
        if (!displayTextBox) ob.GetComponentInChildren<Image>().color = Color.clear;

        obTransform.position = mainCharacter.transform.position;
        ob.transform.SetParent(mainCharacter.transform);

        foreach(string text in textBoxes) {
            yield return cutscene.StartCoroutine(ob.GetComponent<DialogueBox>().Run(text, textScrollSpeed));
        }
        GameObject.Destroy(ob);
        virtualCamera.LookAt = null;
        yield return 0;
    }

    // Helper coroutine to move characters
    IEnumerator MoveCharacter()
    {
        CharacterConfig config = mainCharacter.GetComponent<CharacterConfig>();
        CharacterAnimator animator = mainCharacter.GetComponent<CharacterAnimator>();

        float inTime = moveTime;
        if (inTime == 0f) inTime = 0.1f;
        Vector3 startPos = mainCharacter.transform.position;
        Vector3 endPos = destination.transform.position;
        if (config != null) config.RotateSprite(endPos - startPos);
        endPos = new Vector3(endPos.x, endPos.y, startPos.z);
        for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            mainCharacter.transform.position = Vector3.Lerp(startPos, endPos, t);
            if (animator != null) animator.UpdateWalkAnimation();
            yield return null;
        }
        yield return null;
    }
}
