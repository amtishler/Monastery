using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Vector2 boxSize;

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
        box.isTrigger = true;
        box.size = boxSize;
        rig.isKinematic = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            _instance = this;
            if (CameraSwitcher.ActiveCamera != cam) 
            {
                CameraSwitcher.SwitchCamera(cam);
                Debug.Log("Camera Switched");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            _instance = this;
            if (CameraSwitcher.ActiveCamera == cam) 
            {
                CinemachineVirtualCamera playerCam = other.GetComponentInParent<CinemachineVirtualCamera>();
                Debug.Log(playerCam != null);
                CameraSwitcher.SwitchCamera(playerCam);
                Debug.Log("Camera Switched");
            }
        }
    }
}
