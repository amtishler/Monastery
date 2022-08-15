using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : ProjectileObject
{

    public GameObject fly;
    private Animator animator;

    public float hatchtime;
    private float timer;

    private bool latch;
    private bool dieonhatch;

    protected override void _Start()
    {
        dieonhatch = false;
        latch = false;
        animator = GetComponent<Animator>();
        timer = 0f;
    }

    protected override void _Update()
    {
        if(isProjectile && !latch)
        {
            latch = true;
        }
        if(!isProjectile && latch && !dieonhatch)
        {
            dieonhatch = true;
        }


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
        GameObject player = GameObject.FindWithTag("Player");
        GameObject newfly = Instantiate(fly);
        if(!dieonhatch){
            newfly.transform.position = this.transform.position;
            EnemyConfig flyconf = newfly.GetComponent<EnemyConfig>();
            flyconf.SetTarget(player);
        }else{
            Destroy(newfly);
        }
        Destroy(this.gameObject);
    }
}
