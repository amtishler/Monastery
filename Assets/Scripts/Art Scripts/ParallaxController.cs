using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxController : MonoBehaviour
{
    CinemachineBrain camera;
    GameObject player;
    Vector2 camPos = new Vector2();
    [SerializeField]
    public Rect worldBounds = new Rect(-50, -70, 880, 310);
    Vector2 posRatio = new Vector2();
    Vector2 bgSize = new Vector2();
    public GameObject clouds;
    public float moveSpeed = 1.0f;
    [Range(0f,10f)]
    public float depthMultiplier = 1f;
    public CinemachineVirtualCamera defaultCam;

    // Start is called before the first frame update
    void Start()
    {
        bgSize = GetComponent<SpriteRenderer>().bounds.size;
        camera = GameObject.Find("MainCamera").GetComponent<CinemachineBrain>();
        player = GameObject.Find("Player");
        defaultCam = player.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(camera != null) {
            if(camera.ActiveVirtualCamera != defaultCam) {
                camPos.x = camera.CurrentCameraState.FinalPosition.x;
                camPos.y = camera.CurrentCameraState.FinalPosition.y;
            } else {
                //camPos.x = player.transform.position.x;
                //camPos.y = player.transform.position.y;
                camPos.x = camera.transform.position.x;
                camPos.y = camera.transform.position.y;
            }
            
            
            
            //clouds.transform.position = new Vector3(camPos.x, camPos.y, 0);
            //float step = moveSpeed * Time.deltaTime;
            //clouds.transform.position = Vector3.Lerp (clouds.transform.position, new Vector3(camPos.x, camPos.y, 0), step);
            
            posRatio = (camPos - worldBounds.position) / (worldBounds.size) - (Vector2.one / 2);
            posRatio *= -1;

            camPos += (bgSize / 2) * posRatio;
            camPos *= depthMultiplier;

            transform.position = new Vector3(camPos.x, camPos.y, 0);

        }
        
        
    }

    void OnDrawGizmos()
    {
     // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(worldBounds);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0.01f), new Vector3(rect.size.x, rect.size.y, 0.01f));
    }
}
