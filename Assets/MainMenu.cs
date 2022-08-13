using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private UnityEngine.UI.Button[] buttons;
    [SerializeField] private float xTextPos = 0;
    [SerializeField] private Image highlightIcon;

    public void PlayGame() {
        Debug.Log("Play");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Start() {
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var b in buttons) {
            b.transform.position = new Vector3(Screen.width/xTextPos, b.transform.position.y, 0);
        }
        highlightIcon = GetComponentInChildren<Image>();
        highlightIcon.gameObject.transform.position = new Vector2(Screen.width/xTextPos-150, buttons[0].transform.position.y);
        
    }

    // private void Update() {
    //     highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.gameObject.transform.position.x, EventSystem.current.IsPointerOverGameObject;
    // }

    public void OnPointerEnter(PointerEventData eventData){
        highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.gameObject.transform.position.x, eventData.pointerEnter.transform.position.y);
    }
    public void OnSelect(BaseEventData eventData){
        highlightIcon.transform.position = new Vector3(highlightIcon.transform.position.x, eventData.currentInputModule.transform.position.y, 0);
    }


}
