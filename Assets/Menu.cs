using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;



public class Menu : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private UnityEngine.UI.Button[] buttons;
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private float xTextPos = 0;
    [SerializeField] private Image highlightIcon;
    [SerializeField] private DeviceChange change;
    

    private float backButtonCornerBuffer = 20f;

    bool fading = false;
    MainMenuMusic mmm;
    Image overlay;
    public TextMeshProUGUI loadingText;
    float overlayAlpha = 0f;

    public void PlayGame() {
        Debug.Log("Play");
        overlay.gameObject.SetActive(true);
        fading = true;
        mmm.fadeOut = true;

    }

    public void LoadLevel()
    {
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Awake()
    {
        mmm = GameObject.FindObjectOfType<MainMenuMusic>();
        if (GameObject.Find("FadeOverlay")) overlay = GameObject.Find("FadeOverlay").GetComponent<Image>();

        if (loadingText != null) loadingText.gameObject.SetActive(false);
        if (overlay != null) overlay.gameObject.SetActive(false);
    }

    private void Start() {
        if (change == null) change = GameObject.FindGameObjectWithTag("Event").GetComponent<DeviceChange>();
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        if (backButton != null) backButton.transform.position = new Vector2(Screen.width - backButton.image.rectTransform.sizeDelta.x/2 - backButtonCornerBuffer, backButton.image.rectTransform.sizeDelta.y + backButtonCornerBuffer);
        foreach (var b in buttons) {
            if (b != backButton) b.transform.position = new Vector2(Screen.width/xTextPos, b.transform.position.y);
        }
        EventSystem.current.firstSelectedGameObject = buttons[0].gameObject;
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        if (highlightIcon != null) highlightIcon.gameObject.transform.position = new Vector2(buttons[0].transform.position.x - buttons[0].image.rectTransform.sizeDelta.x/1.5f, buttons[0].transform.position.y);
    }

    private void Update() {
        if (change.usingController && EventSystem.current.currentSelectedGameObject != null && highlightIcon != null) {
            highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.transform.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y);
            Debug.Log("HERE 1");
        }
        if (fading)
        {
            overlayAlpha += .25f * Time.deltaTime;
            overlay.color = new Color(0, 0, 0, overlayAlpha);

            if (overlayAlpha >= 1.13)
            {
                LoadLevel();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        EventSystem.current.SetSelectedGameObject(eventData.pointerEnter);
        Debug.Log(eventData);
        if (highlightIcon != null)
        {
            highlightIcon.gameObject.transform.position = new Vector2(highlightIcon.transform.position.x, eventData.pointerEnter.transform.position.y);
            Debug.Log("HERE 2");
        }
    }

    public void ResetButtons() {
        foreach (var b in buttons) {
            if (b != backButton) b.transform.position = new Vector2(Screen.width/xTextPos, b.transform.position.y);
        }
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        EventSystem.current.firstSelectedGameObject = buttons[0].gameObject;
        if (highlightIcon != null)
        {
            highlightIcon.gameObject.transform.position = new Vector2(buttons[0].transform.position.x - buttons[0].image.rectTransform.sizeDelta.x / 1.5f, buttons[0].transform.position.y);
            Debug.Log("HERE 3");
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }

    public void ResumeGame() {
        Debug.Log("Unpause");
        InputManager.Instance.CombatMap();
        Time.timeScale = 1;
    }
}
