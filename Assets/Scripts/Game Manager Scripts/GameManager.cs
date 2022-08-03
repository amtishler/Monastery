using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject[] checkpoints;
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
