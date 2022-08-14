using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyConfig : CharacterConfig {
    
    public GameObject target;
    public GameObject egg;
    public GameObject spit;
    public GameObject attackTriggerZone;
    private EnemyAttackZone aggroZone; 

    public GameObject waypointobject;
    public List<GameObject> waypoints = new List<GameObject>();
    public int currentisland = 0;
    private int islandstraversed;

    public float islandwait = 2f;
    public float stoppingspeed = 2f;
    public float retreatingspeed = 8f;
    public float retreatdistance = 3f;
    public int hitstoretreat = 2;
    private int hitstaken = 0;

    //Spawn egg every x islands;
    public int islandspawncount;

    public bool attacking;
    public bool retreating;

    private Animator animator;

    public List<float> spitangles;

    private bool stopping;
    private bool spawning;
    private float timer;
    private float originalspeed;

    public float nextWaypointDistance;

    protected override void _Start() {

        hitstaken = 0;

        //Populate waypoints
        foreach(Transform child in waypointobject.transform)
        {
            waypoints.Add(child.gameObject);
        }

        islandstraversed = 0;
        originalspeed = maximumSpeed;
        animator = GetComponent<Animator>();
       if(attackTriggerZone != null)
        {
            aggroZone = attackTriggerZone.GetComponent<EnemyAttackZone>();
        }

        //target = GameObject.FindWithTag("Player");
        grabbable = false;
        //attacking = true;
        attacking = false;
        retreating = false;
        InvokeRepeating("PlaySpit", 2f, 3f);
        return;
    }

    void OnAwake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Player";
    }

    public void SpawnChild()
    {
        if(attacking && !stunned)
        {
            animator.Play("LayEgg");
        }
    }

    public void PlaySpit()
    {
        if(attacking && !stunned && !spawning)
        {
            animator.Play("Spit");
        }
    }

    protected override void _Hit()
    {
        stopping = false;
        spawning = false;
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        hitstaken++;
        if(distance < retreatdistance && hitstaken >= hitstoretreat){
            Retreat();
        }
        
        animator.Play("GetHit");
    }

    void Retreat()
    {
        maximumSpeed = retreatingspeed;
        currentisland += (int)waypoints.Count / 2;
        if(currentisland > waypoints.Count - 1){
            currentisland -= (waypoints.Count - 1);
        }
    }

    void Spit()
    {
        foreach(float angle in spitangles){
            GameObject spitchild = Instantiate(spit);
            spitchild.transform.position = this.transform.position;
            Spitball spitconf = spitchild.GetComponent<Spitball>();

            Vector3 dirtotarget = target.transform.position - this.transform.position;
            dirtotarget.Normalize();

            spitconf.targetdir = Quaternion.Euler(0, 0, angle) * dirtotarget;

            float rot_z = Mathf.Atan2(spitconf.targetdir.y, spitconf.targetdir.x) * Mathf.Rad2Deg;
            spitconf.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
        }
    }

    void CreateChild()
    {
        GameObject newfly = Instantiate(egg);
        Vector3 offset = new Vector3(0, -2, 0);
        newfly.transform.position = this.transform.position + offset;
    }


    public void SetTarget(GameObject objtarget)
    {
        attacking = true;
        target = objtarget;
    }

    protected override void _Update() {

        animator.SetFloat("Speed", Speed);
        animator.SetBool("Stunned", stunned);
        animator.SetBool("Dead", dead);

        if(attackTriggerZone != null && target == null && aggroZone.playerentered)
        {
            SetTarget(aggroZone.playergameobj);
        }

        if(stopping)
        {
            timer += Time.deltaTime;
            if(timer > islandwait){
                hitstaken = 0;
                stopping = false;
                spawning = false;
                currentisland++;
                if(currentisland > waypoints.Count - 1){
                    currentisland = 0;
                }
                maximumSpeed = originalspeed;
            }
        }
    }

    public void Move()
    {
        Vector3 targetDir = waypoints[currentisland].transform.position - this.transform.position;
        targetDir.Normalize();
        float distance = Vector3.Distance(this.transform.position, waypoints[currentisland].transform.position);

        if(distance < nextWaypointDistance && !stopping)
        {
            timer = 0;
            if(islandstraversed % islandspawncount == 0){
                SpawnChild();
                spawning = true;
            }
            islandstraversed++;
            maximumSpeed = stoppingspeed;
            stopping = true;
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
