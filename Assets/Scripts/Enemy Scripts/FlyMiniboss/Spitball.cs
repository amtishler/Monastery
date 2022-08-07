using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
    
    public Vector3 targetdir;
    public float flyingtime;
    public float speed;
    public float damage;
    public float stun;
    public float knockback;

    private float timer;

    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        this.transform.position += (targetdir * speed * Time.deltaTime);

        if(timer > flyingtime)
        {
            Destroy(this.gameObject);
        }
    }

    // Trigger enter, when attack hits something else
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //LIVING THINGS
        CharacterConfig d = collision.transform.GetComponent<CharacterConfig>();
        if (d != null)
        {
            // put knockback calculations here or something
            Vector3 knockbackdir = collision.transform.position - this.transform.position;
            knockbackdir.Normalize();
            GameManager.Instance.DamageCharacter(d, damage, stun, knockbackdir, knockback);
            Destroy(this.gameObject);
        }

    }
}
