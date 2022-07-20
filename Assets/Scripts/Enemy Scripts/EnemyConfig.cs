using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyConfig : CharacterConfig {

    public float detectionradius = 10f;
    public float attackradius = 4f;
    public float collisiondamage = 0f;
    public float collisionknockback = 10f;
    public float projectileslowdown = 0.1f;

    //Stalking speed (Getting ready to attack)
    public float readyingspeed = 1f;

    //Time spent stalking (before attacking)
    public float attacktimer = 1f;

    //Pouncing speed;
    public float pouncespeed = 10f;
    public float pounceslowdown = 0.1f;

    public float attackcooldown = 5f;
    public bool isattacking;
    public bool oncooldown;
    private bool timerstarted;
    private float cooldowntimer;

    public GameObject target;
    public GameObject attackhitbox;
    public Vector3 attackvector;

    private Animator animator;

    //Pathfinding vars
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    protected override void _Start() {
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        isattacking = false;
        grabbable = false;

        
        InvokeRepeating("UpdatePath", 0f, .5f);
        return;
    }

    void OnAwake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Player";
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null) seeker.StartPath(rigidBody.position, target.transform.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected override void _Update() {
        if(!timerstarted && oncooldown)
        {
            cooldowntimer = 0;
            timerstarted = true;
        }
        if(timerstarted)
        {
            cooldowntimer += Time.deltaTime;
            if(cooldowntimer > attackcooldown)
            {
                oncooldown = false;
                timerstarted = false;
            }
        }

        animator.SetFloat("Speed", Speed);
        animator.SetBool("Attacking", isattacking);
        animator.SetBool("Stunned", stunned);
    }



    public void MoveTowards(GameObject target)
    {

        if(path == null)
        {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            SlowDown(deacceleration);
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector3 targetDir = path.vectorPath[currentWaypoint] - this.transform.position;
        targetDir.Normalize();

        float distance = Vector3.Distance(this.transform.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (targetDir == Vector3.zero) {
            SlowDown(deacceleration);
        } else {
            if (speed < minimumSpeed) speed = minimumSpeed;
            speed = speed + acceleration;
            if (speed > maximumSpeed) speed = maximumSpeed;
            Velocity = targetDir*speed;
        }

        // Update sprite TODO
        RotateSprite(targetDir);
    }

    //Added to visualize radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
        Gizmos.DrawWireSphere(transform.position, attackradius);
    }


    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        switch(angle){
            case 0:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 2:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }
        currentdir = angle;
    }

}
