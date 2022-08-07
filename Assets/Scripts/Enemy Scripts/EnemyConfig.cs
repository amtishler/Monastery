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


    [Header("Attacking Values")]
    //Stalking speed (Getting ready to attack)
    public float readyingspeed = 1f;

    //Time spent stalking (before attacking)
    public float attacktimer = 1f;

    public float animationstart = 0.8f;

    //Pouncing speed;
    public float pouncespeed = 10f;
    public float pounceslowdown = 0.1f;

    public float attackcooldown = 5f;
    public float postattackpause = 1f;
    public bool isattacking;
    public bool oncooldown;
    private bool timerstarted;
    private float cooldowntimer;

    public GameObject attackTriggerZone;
    private EnemyAttackZone aggroZone; 

    public GameObject target;
    public GameObject attackhitbox;
    public Vector3 attackvector;

    private Animator animator;

    [Header("Pathfinding Values")]
    //Pathfinding vars
    public float nextWaypointDistance = 0.5f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    public float targetDistance = 3f;
    public float targetDistanceWindow = 0.5f;
    private bool backup = false;
    private Vector3 pathendpoint;

    protected override void _Start() {
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();

        if(attackTriggerZone != null)
        {
            aggroZone = attackTriggerZone.GetComponent<EnemyAttackZone>();
        }

        isattacking = false;
        grabbable = false;
        dead = false;

        
        InvokeRepeating("UpdatePath", 0f, .25f);
        return;
    }

    void OnAwake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Player";
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null && !backup) seeker.StartPath(this.transform.position, pathendpoint, OnPathComplete);
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

        if(attackTriggerZone != null && target == null && aggroZone.playerentered)
        {
            SetTarget(aggroZone.playergameobj);
        }

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
        animator.SetBool("Dead", dead);
    }

    public void SetTarget(GameObject objtarget)
    {
        pathendpoint = objtarget.transform.position;
        target = objtarget;
    }

    public virtual void Move(GameObject target)
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if(distance < targetDistance - targetDistanceWindow)
        {
            pathendpoint = this.transform.position + (this.transform.position - target.transform.position);
            MoveTowards();
        }
        else if(distance > targetDistance + targetDistanceWindow)
        {
            pathendpoint = target.transform.position;
            MoveTowards();
        }
        else{
            SlowDown(deacceleration);
        }
    }

    public void MoveTowards()
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
            if (Speed < minimumSpeed) Velocity = Velocity.normalized * minimumSpeed;
            if (Speed > maximumSpeed) Velocity = Velocity.normalized * maximumSpeed;
            Velocity += targetDir*acceleration;
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
        if(target != null){
            Vector3 playervector = this.transform.position - target.transform.position;
            switch(Mathf.Sign(playervector.x)){
                case -1:
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                default:
                    break;
            }
        }
        else{
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
}
