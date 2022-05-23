using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour {

    //Movement variables
    private CircleCollider2D playerCollider;
    private Vector3 moveDelta;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    Vector3 currentDir = Vector3.zero;
    Vector3 currentDirVelocity = Vector3.zero;


    /*
    Start method, called before the first frame update
    */
    void Start() {   
        playerCollider = GetComponent<CircleCollider2D>();
    }

    /*
    Update method, called once per frame
    */
    void Update() {
        UpdateMovement();
    }

    /*
    Updates movement
    */
    void UpdateMovement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        targetDir.Normalize();

        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        Vector3 velocity = currentDir * walkSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }
}
