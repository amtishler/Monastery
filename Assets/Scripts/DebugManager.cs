using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    // Input refs


    // Camera refs
    private CinemachineBrain mainCamera;
    private CinemachineVirtualCamera cam;
    private CameraAim camDir;

    // Music refs
    private MusicManager musicManager;

    // Player refs
    private GameObject player;
    private PlayerConfig playerConfig;
    private PlayerStateMachine playerStateMachine;
    private PlayerAnimator playerAnimator;


    [Header("Input")]

    [Header("Camera")]
    [SerializeField] private TextMeshProUGUI activeCam;
    [SerializeField] private TextMeshProUGUI playerCamPos;
    [SerializeField] private TextMeshProUGUI combatArena;

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
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CinemachineVirtualCamera>();
        camDir = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CameraAim>();
        // Music refs
        musicManager = MusicManager.Instance;

        // Player refs
        player = GameObject.FindGameObjectWithTag("Player");
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
        activeCam.text = "ActiveCam: " + mainCamera.ActiveVirtualCamera.Name;
        playerCamPos.text = "PlayerCamPos: " + camDir.DebugVec();
        // Was causing errors:
        //combatArena.text = "CombatArena: " + (cam.Priority != 10).ToString();


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
