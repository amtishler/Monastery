using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    // Input refs


    // Camera refs
    private Camera mainCamera;

    // Music refs
    private MusicManager musicManager;

    // Player refs
    private GameObject player;
    private InputHandler inputHandler;
    private PlayerConfig playerConfig;
    private PlayerStateMachine playerStateMachine;
    private PlayerAnimator playerAnimator;


    [Header("Input")]

    [Header("Camera")]

    [Header("Music")]
    [SerializeField] private TextMeshProUGUI area;
    [SerializeField] private TextMeshProUGUI variant;
    [SerializeField] private TextMeshProUGUI activeVolume;
    [SerializeField] private TextMeshProUGUI fadingVolume;

    [Header("Player")]
    [SerializeField] private TextMeshProUGUI actionInput;
    [SerializeField] private TextMeshProUGUI state;


    private void Start()
    {
        // Input refs


        // Camera refs
        mainCamera = GameObject.Find("PlayerCam").GetComponent<Camera>();

        // Music refs
        musicManager = MusicManager.Instance;

        // Player refs
        player = GameObject.Find("Player");
        inputHandler = player.GetComponent<InputHandler>();
        playerConfig = player.GetComponent<PlayerConfig>();
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerAnimator = player.GetComponent<PlayerAnimator>();

        Debug.Log(playerStateMachine.currentState);
    }


    // Update is called once per frame
    void Update()
    {
        // Input:


        // Camera:


        // Music:
        area.text = "Area: " + MusicManager.Instance.debugTexts[0];
        variant.text = "Variant: " + MusicManager.Instance.debugTexts[1];
        activeVolume.text = "Active Volume: " + MusicManager.Instance.debugTexts[2];
        fadingVolume.text = "Fading Volume: " + MusicManager.Instance.debugTexts[3];

        // Player:
        actionInput.text = "Action Input: ";// + inputHandler.inputText;
        state.text = "State: " + playerStateMachine.currentState.name;

    }

}
