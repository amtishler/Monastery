using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private System.Collections.Generic.Dictionary<string, Transform> colliders;
    [SerializeField] private float bufferPlayerHeight;
    [SerializeField] private float bufferPlayerWidth;
    [SerializeField] private EnemyTracker enemies;
    public bool isCutscene = true;
    private bool cutsceneComplete = false;
    private bool edgesCreated = false;
    [SerializeField] private bool activated = false;
    private Vector2 screenSize;

    [SerializeField] private Vector2 maxColliderSize = new Vector2(20, 10);
    private int musicVariantToReturnTo = 0;

    public static CameraController Instance //Singleton Stuff
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


    BoxCollider2D box;
    Rigidbody2D rig;


    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        enemies = GetComponent<EnemyTracker>();
        box.isTrigger = true;
        rig.isKinematic = true;
        screenSize.x = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        colliders = new System.Collections.Generic.Dictionary<string, Transform>();
    }

    private void Update() {
        if (!isCutscene && activated) {
            if (!enemies.enemiesDefeated) enemies.CheckTimer();
            else
            {
                Debug.Log(this.gameObject);
                this.gameObject.SetActive(false);
                MusicManager.Instance.HandleTrigger(false, FadeSpeed.normal, Area.Forest, musicVariantToReturnTo, FadeSpeed.normal);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, maxColliderSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCutscene && cutsceneComplete) return;
        //Debug.Log("Trigger detected");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            _instance = this;
            activated = true;
            if (CameraSwitcher.ActiveCamera != cam) 
            {
                CameraSwitcher.SwitchCamera(cam);
                Debug.Log("Camera Switched");
            }
            if (!isCutscene && !edgesCreated) {
                CreateEdges();
                enemies.ActivateEnemies();
                musicVariantToReturnTo = MusicManager.Instance.getCurrentVariant();
                MusicManager.Instance.HandleTrigger(false, FadeSpeed.normal, Area.Forest, 4, FadeSpeed.normal);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isCutscene)
        {
            //Debug.Log("Player detected");
            CinemachineVirtualCamera playerCam = other.gameObject.GetComponentInParent<CinemachineVirtualCamera>();
            Finish(playerCam);
        }
    }

    public void Finish(CinemachineVirtualCamera playerCam)
    {
        _instance = this;
        if (CameraSwitcher.ActiveCamera == cam)
        {
            CameraSwitcher.SwitchCamera(playerCam);
            Debug.Log("Camera Switched");
        }
        cutsceneComplete = true;
    }

    private void CreateEdges() {
        colliders.Add("TopCollider",new GameObject().transform);
        colliders.Add("BottomCollider",new GameObject().transform);
        colliders.Add("RightCollider",new GameObject().transform);
        colliders.Add("LeftCollider",new GameObject().transform);

        foreach (var c in colliders) {
            c.Value.gameObject.AddComponent<BoxCollider2D>();
            c.Value.parent = this.gameObject.transform;
            c.Value.gameObject.layer = GameManager.Instance.WALL_COLLISION;

            if (c.Key == "LeftCollider" || c.Key == "RightCollider") c.Value.localScale = new Vector3(bufferPlayerWidth, screenSize.y*2, bufferPlayerWidth);
            else c.Value.localScale = new Vector3(screenSize.x*2, bufferPlayerHeight, bufferPlayerHeight);
        }

        colliders["RightCollider"].position = new Vector3(cam.transform.position.x + screenSize.x + (colliders["RightCollider"].localScale.x * 0.5f), cam.transform.position.y, 0f);
        colliders["LeftCollider"].position = new Vector3(cam.transform.position.x - screenSize.x - (colliders["LeftCollider"].localScale.x * 0.5f), cam.transform.position.y, 0f);
        colliders["TopCollider"].position = new Vector3(cam.transform.position.x, cam.transform.position.y + screenSize.y - 1f + (colliders["TopCollider"].localScale.y * 0.5f), 0f);
        colliders["BottomCollider"].position = new Vector3(cam.transform.position.x, cam.transform.position.y - screenSize.y - (colliders["BottomCollider"].localScale.y * 0.5f), 0f);

        edgesCreated = true;
        // Debug.Log(cam.transform.position);
    }

    public void ResetZone() {
        if (!isCutscene) {
            enemies.ResetEnemies();
            activated = false;
            edgesCreated = false;
            foreach (var c in colliders) {
                Destroy(c.Value.gameObject);
            }
            colliders.Clear();
            if (CameraSwitcher.ActiveCamera == cam) CameraSwitcher.SwitchCamera(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CinemachineVirtualCamera>());
        }
        else {
            activated = false;
        }
    }
}
