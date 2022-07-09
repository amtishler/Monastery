using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalAmbience : MonoBehaviour
{
    // A note: All FMOD ambience files need to have a master volume parameter named "MasterVol" for this to work

    private enum AmbienceFX
    {
        river,
        // Reminder to always at the end of the list, so we don't mess up serialization
    }

    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> localAmbiences = new List<FMOD.Studio.EventInstance>();

    private static PlayerConfig player;


    private FMOD.Studio.EventInstance currentLocalAmbience;


    [SerializeField]
    private float radius;

    private bool playerInZone;

    [SerializeField]
    private AmbienceFX ambienceName;

    private float currentVolume;

    private CircleCollider2D circle;
    private Rigidbody2D rig;

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        circle.isTrigger = true;
        circle.radius = radius;
        rig.isKinematic = true;

        localAmbiences.Add(FMODUnity.RuntimeManager.CreateInstance("event:/AmbientSFX/River"));

        currentVolume = 0;

        // Better way to do this?
        player = FindObjectOfType<PlayerConfig>();

        // What if player spawns in the zone? Fix later
        playerInZone = false;
    }

    private void Update()
    {
        if (playerInZone)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

            float playerDistance = Vector2.Distance(pos, playerPos);

            currentVolume = (radius - playerDistance) / radius;
            if (currentVolume < 0)
                currentVolume = 0;

            currentLocalAmbience.setParameterByName("MasterVol", currentVolume);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Local Ambience On");

            currentLocalAmbience = localAmbiences[(int)ambienceName];
            currentLocalAmbience.start();
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Local Ambience Off");

            currentLocalAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentVolume = 0;

            //MusicManager.Instance.HandleTrigger(trigger);
        }
    }




}
