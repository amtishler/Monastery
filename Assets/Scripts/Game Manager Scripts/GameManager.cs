using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Layers
    public int PLAYER_HURTBOX = 3;
    public int TONGUE_HITBOX = 6;
    public int OBJECT_HURTBOX = 7;
    public int CAMERA_HITBOX = 8;
    public int WALL_COLLISION = 9;
    public int ATTACK_HITBOX = 10;
    public int ENEMY_HITBOX = 11;
    public int SPIT_PROJECTILE_HITBOX = 12;
    public int PLAYER_WALL_COLLISION = 13;
    public int ENEMY_WALL_COLLISION = 14;
    public int COLLECTIBLE_HITBOX = 15;
    public int BOUNDARY = 17;
    public int OBJECT_WALL_COLLISION = 18;
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
            if (cp != null) {
                if (cp.activeCheckpoint) {
                    cp.ResetObjects();
                    cp.ResetZones();
                }
            }
        }
        MusicManager.Instance.HandleTrigger(false, FadeSpeed.normal, Area.Forest, 0, FadeSpeed.normal);
        InputManager.Instance.CombatMap();
    }
}
