using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class Menu : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private UnityEngine.UI.Button[] buttons;
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private float xTextPos = 0;
    [SerializeField] private Image highlightIcon;
    [SerializeField] private DeviceChange change;

    private float backButtonCornerBuffer = 20f;

    public void PlayGame() {
        Debug.Log("Play");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Start() {
        if (change == null) change = GameObject.FindGameObjectWithTag("Event").GetComponent<DeviceChange>();
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        change.lastButtonHighlighted = buttons[0].gameObject;
        Debug.Log(change.lastButtonHighlighted);
        if (backButton != null) backButton.transform.position = new Vector2(Screen.width - backButton.image.rectTransform.sizeDelta.x/2 - backButtonCornerBuffer, backButton.image.rectTransform.sizeDelta.y + backButtonCornerBuffer);
        foreach (var b in buttons) {
            if (b != backButton) b.transform.position = new Vector2(Screen.width/xTextPos, b.transform.position.y);

        }
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        highlightIcon.gameObject.transform.position = new Vector2(buttons[0].transform.position.x - buttons[0].image.rectTransform.sizeDelta.x/1.5f, buttons[0].transform.position.y);
    }

    private void Update() {
        if (change.usingController && EventSystem.current.currentSelectedGameObject != null) highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.transform.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y);
    }

    public void OnPointerEnter(PointerEventData eventData){
        EventSystem.current.SetSelectedGameObject(eventData.pointerEnter);
        highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.transform.position.x, eventData.pointerEnter.transform.position.y);
        change.lastButtonHighlighted = eventData.pointerEnter;
        // Debug.Log(EventSystem.current);
        // Debug.Log(eventData.pointerEnter);
    }
}
