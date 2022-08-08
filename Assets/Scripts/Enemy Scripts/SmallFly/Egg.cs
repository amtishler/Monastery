using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : ProjectileObject
{

    public GameObject fly;
    private Animator animator;

    public float hatchtime;
    private float timer;

    protected override void _Start()
    {
        animator = GetComponent<Animator>();
        timer = 0f;
    }

    protected override void _Update()
    {
        timer += Time.deltaTime;
        if(timer > hatchtime && !isProjectile && this.gameObject.activeSelf){
            rigidBody.simulated = false;
            animator.Play("Hatch");
        }
    }

    protected override void _OnHit()
    {
        Destroy(this.gameObject);
    }

    public void Hatch()
    {
        GameObject newfly = Instantiate(fly);
        newfly.transform.position = this.transform.position;
        EnemyConfig flyconf = newfly.GetComponent<EnemyConfig>();
        flyconf.SetTarget(GameObject.FindWithTag("Player"));
        Destroy(this.gameObject);
    }
}
