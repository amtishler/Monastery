using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;

public class FootstepParticleController : MonoBehaviour
{
    
    [Range(0f, 10f)]
    public float velocityThreshold = 5.0f;
    
    public Rigidbody2D rigidbody;
    public ParticleSystem pSystem;
    public Material particleMat;

    public int particlesPerStep = 2;
    public float minSpeed = -0.5f;
    public float maxSpeed = 1.0f;

    bool emitting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidbody.velocity.magnitude >= velocityThreshold && emitting == false) {
            //transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
            /*if(groundGrid != null) {
                Vector3Int gridPos = groundGrid.WorldToCell(transform.position);
                var tile = groundGrid.GetTile<SuperTile>(gridPos);
                if(tile != null) {
                    particleMat.SetColor("_Color", SampleColor(tile.m_Sprite.texture));
                    pSystem.GetComponent<ParticleSystemRenderer>().material = particleMat;
                }

            }*/
            InvokeRepeating("DoEmit", 0f, .2f);
            emitting = true;
        }
        else {
            if(emitting = true) {
                CancelInvoke();
                emitting = false;
            }            
        }
    }

    void DoEmit() {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = Color.red;
        emitParams.velocity = ((Vector3)rigidbody.velocity * Random.Range(maxSpeed, minSpeed));
        
        pSystem.Emit(emitParams, particlesPerStep);
    }
}
