using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
    }

    private void Update()
    {
        
    }

    public void DamageCharacter(CharacterConfig character, float damage, Vector3 knockbackdir, float knockbackmag)
    {
        character.Hit(damage, knockbackdir, knockbackmag);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
