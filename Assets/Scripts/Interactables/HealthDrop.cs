using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{

    public float healthgain = 10f;
    public float dropspeed = 0f;
    public float dropaccel = 0.5f;
    private bool dropping = false;
    private float droplevel = 0f;

private static FMOD.Studio.EventInstance dropSound = new FMOD.Studio.EventInstance();

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterConfig d = collision.transform.GetComponent<PlayerConfig>();
        CharacterConfig parent = this.transform.parent.transform.GetComponent<PlayerConfig>();
        if (d != null && parent != null)
        {
            d.Heal(healthgain);
            Destroy(this.gameObject);
        }
    }*/

    private void Start()
    {
        dropSound = FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Features/Health Fruit/Hit Ground");
    }

    void Update() {
        if(dropping)
        {
            Vector3 poschange = new Vector3(0, -dropspeed * Time.deltaTime, 0);
            this.transform.position += poschange;
            dropspeed += dropaccel;
        }
        if(dropping && this.transform.localPosition.y < droplevel)
        {
            dropping = false;
            dropSound.start();
            this.GetComponent<Collider2D>().enabled = true;
        }
    }

    public void Eat(PlayerConfig player) {
        player.Heal(healthgain);
        Destroy(this.gameObject);
    }

    public void Drop(float level)
    {
        droplevel = level;
        dropping = true;
    }
}
