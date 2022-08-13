using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image telescopeImage;

    private RectTransform imageSize;

    private float width;
    private float height;
    //Layers
    [System.NonSerialized] public int PLAYER_HURTBOX = 3;
    [System.NonSerialized] public int TONGUE_HITBOX = 6;
    [System.NonSerialized] public int OBJECT_HURTBOX = 7;
    [System.NonSerialized] public int CAMERA_HITBOX = 8;
    [System.NonSerialized] public int WALL_COLLISION = 9;
    [System.NonSerialized] public int ATTACK_HITBOX = 10;
    [System.NonSerialized] public int ENEMY_HITBOX = 11;
    [System.NonSerialized] public int SPIT_PROJECTILE_HITBOX = 12;
    [System.NonSerialized] public int PLAYER_WALL_COLLISION = 13;
    [System.NonSerialized] public int ENEMY_WALL_COLLISION = 14;
    [System.NonSerialized] public int COLLECTIBLE_HITBOX = 15;
    [System.NonSerialized] public int BOUNDARY = 17;
    [System.NonSerialized] public int OBJECT_WALL_COLLISION = 18;
    public GameObject player;
    public GameObject[] checkpoints;
    private GameObject debug;
    private bool debugActive;
    //GameManager.Instance.whatever
    private static GameManager _instance;
    public static GameManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Game Manager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 144;
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        player = GameObject.FindGameObjectWithTag("Player");
        telescopeImage = GetComponentInChildren<Image>();
        imageSize = telescopeImage.GetComponent<RectTransform>();
        imageSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        imageSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        telescopeImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(InputManager.Instance.QuitPressed) {
            Application.Quit();
            Debug.Log("Quit");
        }
    }

    public void DamageCharacter(CharacterConfig character, float damage, float stun, Vector3 knockbackdir, float knockbackmag)
    {
        character.Hit(damage, stun, knockbackdir, knockbackmag);
    }

    //Scene Functions
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Tutorial Messages
    public TutorialMessages GetTutorialMessages() {
        return GetComponent<TutorialMessages>();
    }

    public void ResetLevel() {
        foreach (var c in checkpoints) {
            Checkpoint cp = c.GetComponent<Checkpoint>();
            if (cp.gameObject.activeInHierarchy) {
                if (cp.activeCheckpoint) {
                    cp.ResetObjects();
                    cp.ResetZones();
                    break;
                }
            }
        }
        MusicManager.Instance.HandleTrigger(false, FadeSpeed.normal, Area.Forest, 0, FadeSpeed.normal);
        InputManager.Instance.CombatMap();
    }
}
