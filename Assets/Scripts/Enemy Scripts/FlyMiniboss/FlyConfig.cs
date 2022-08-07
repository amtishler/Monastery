using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyConfig : CharacterConfig {
    
    public GameObject target;
    public GameObject flychild;
    public GameObject spit;
    public int maxchildren = 3;
    private int numchildren = 0;
    public bool attacking;
    private Animator animator;

    public List<float> spitangles;

    [Header("Pathfinding Values")]
    //Pathfinding vars
    public float nextWaypointDistance = 0.5f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    public float targetDistance = 3f;
    public float targetDistanceWindow = 0.5f;
    private Vector3 pathendpoint;

    protected override void _Start() {
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        grabbable = false;
        attacking = true;
        pathendpoint = target.transform.position;
        InvokeRepeating("UpdatePath", 0f, .25f);
        InvokeRepeating("PlaySpit", 5f, 2f);
        return;
    }

    void OnAwake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Player";
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null) seeker.StartPath(this.transform.position, pathendpoint, OnPathComplete);
    }

    void SpawnChild()
    {
        if(attacking && numchildren < maxchildren)
        {
            animator.Play("LayEgg");
        }
    }

    void PlaySpit()
    {
        if(attacking)
        {
            animator.Play("Spit");
        }
    }

    void Spit()
    {
        foreach(float angle in spitangles){
        GameObject spitchild = Instantiate(spit);
        spitchild.transform.position = this.transform.position;
        Spitball spitconf = spitchild.GetComponent<Spitball>();

        Vector3 dirtotarget = target.transform.position - this.transform.position;

        spitconf.targetdir = Quaternion.Euler(0, 0, angle) * dirtotarget;

        float rot_z = Mathf.Atan2(spitconf.targetdir.y, spitconf.targetdir.x) * Mathf.Rad2Deg;
        spitconf.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
        }
    }

    void CreateChild()
    {
        numchildren++;
        GameObject newfly = Instantiate(flychild);
        newfly.transform.position = this.transform.position;
        EnemyConfig flyconf = newfly.GetComponent<EnemyConfig>();
        flyconf.SetTarget(target);
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

        animator.SetFloat("Speed", Speed);
        animator.SetBool("Stunned", stunned);
    }

    public void Move(GameObject target)
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
            Vector3 totarget = target.transform.position - this.transform.position;
            totarget.Normalize();
            Vector3 perptarget = Quaternion.AngleAxis(90, Vector3.forward) * totarget;
            pathendpoint = this.transform.position + (perptarget * 10);
            MoveTowards();
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
            if (Speed < minimumSpeed) Velocity = Velocity.normalized*minimumSpeed;
            Velocity += targetDir*acceleration;
            if (Speed > maximumSpeed) Velocity = Velocity.normalized*maximumSpeed;
        }

        // Update sprite TODO
        RotateSprite(targetDir);
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
