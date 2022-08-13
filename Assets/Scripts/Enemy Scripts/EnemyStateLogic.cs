using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class EnemyState : State {
    protected EnemyConfig config;
    protected EnemyStateMachine currentContext;
    protected EnemyStateFactory factory;

    public EnemyState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(currentContext) {
        this.config = config;
        this.currentContext = currentContext;
        this.factory = stateFactory;
    }
}


/* ====================
       SUBCLASSES
==================== */

// Idle
public class EnemyIdleState : EnemyState {

    public EnemyIdleState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.projectile) SwitchStates(factory.Projectile());
        else if(config.grabbed) SwitchStates(factory.Grabbed());
        else if(CheckVision()) SwitchStates(factory.Aggressive());
    }

    private bool CheckVision()
    {
        if(config.target != null)
        {
            return true;
        }
        Collider2D collider = Physics2D.OverlapCircle(config.transform.position, config.detectionradius, LayerMask.GetMask("Player Hurtbox"));
        if(collider == null) return false;
        else{
            config.SetTarget(collider.gameObject);
            return true;
        }
    }
}

// Aggressive
public class EnemyAggressiveState : EnemyState {

    public EnemyAggressiveState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        if(config.target == null) SwitchStates(factory.Idle());
    }

    public override void UpdateState() {
        config.Move(config.target);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.grabbed) SwitchStates(factory.Grabbed());
        else if(CheckVision() && !config.oncooldown) SwitchStates(factory.Readying());
    }

    private bool CheckVision()
    {
        Collider2D collider = Physics2D.OverlapCircle(config.transform.position, config.attackradius, LayerMask.GetMask("Player Hurtbox"));
        if(collider != null)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(config.transform.position, collider.transform.position, LayerMask.GetMask("Boundary"));
            if(hits.Length == 0)
            {
                return true;
            }
        }
        return false;
    }
}

// Readying
public class EnemyReadyingState : EnemyState {

    private float oldspeed;
    private float timer;
    private bool stopped;
    private Animator animator;

    public EnemyReadyingState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        timer = 0;
        stopped = false;
        animator = config.GetComponent<Animator>();
        if(config.target == null) SwitchStates(factory.Idle());
        oldspeed = config.MaximumSpeed;
        config.MaximumSpeed = config.readyingspeed;
    }

    public override void UpdateState() {
        timer += Time.deltaTime;

        if(timer < (config.attacktimer / 2f))
        {
            config.Move(config.target);
        }
        else if(!config.isattacking && timer > config.animationstart)
        {
            animator.Play("Attack");
            config.isattacking = true;
        }
        else
        {
            // Locks in attack vector halfway through to give a chance to dodge.
            if(!stopped)
            {
                stopped = true;
                config.Velocity = Vector3.zero;
                config.attackvector = config.target.transform.position - config.transform.position;
            }
        }
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.MaximumSpeed = oldspeed;
    }

    public override void CheckSwitchStates() {
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.grabbed) SwitchStates(factory.Grabbed());
        if(timer > config.attacktimer) SwitchStates(factory.Attack());
    }
}

// Attacking
public class EnemyAttackState : EnemyState {

    public EnemyAttackState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    private Animator animator;
    private float pausetimer;
    private bool timerstarted;
    private bool doneattacking;

    public override void EnterState() {
        pausetimer = 0f;
        timerstarted = false;
        doneattacking = false;
        config.isattacking = false;
        animator = config.GetComponent<Animator>();
        config.Velocity = config.attackvector.normalized * config.pouncespeed;
        //config.attackhitbox.SetActive(true);
    }

    public override void UpdateState() {

        if(config.Speed != 0)
        {
            config.SlowDown(config.pounceslowdown);
        }
        else
        {
            if(!timerstarted) timerstarted = true;
            else
            {
                pausetimer += Time.deltaTime;
            }
        }

        if(pausetimer > config.postattackpause)
        {
            doneattacking = true;
        }
        
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.oncooldown = true;
        //config.attackhitbox.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.grabbed) SwitchStates(factory.Grabbed());
        else if(doneattacking){
            if(config.target != null) SwitchStates(factory.Aggressive());
            else SwitchStates(factory.Idle());
        }
    }
}

// Hurt
public class EnemyHurtState : EnemyState {

    private float deacceleration;
    private float recoverytimer;


    public EnemyHurtState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        deacceleration = config.RecoveryDeaccel;
        config.SlowDown(deacceleration);
        config.SetTarget(GameObject.FindWithTag("Player"));
    }

    public override void UpdateState()
    {
        deacceleration += config.RecoveryDeaccel * Time.deltaTime;
        config.SlowDown(deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates(){
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.Speed == 0){
            if(config.target != null) SwitchStates(factory.Aggressive());
            else SwitchStates(factory.Idle());
        }
    }
}

// Dead
public class EnemyDeadState : EnemyState {

    private float deacceleration;
    private float recoverytimer;
    private HitboxController selfhitbox;
    private Rigidbody2D body;

    public EnemyDeadState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory)
    {
        name = "EnemyDead";
    }

    public override void EnterState() {
        config.invincible = true;
        config.dead = true;
        config.grabbable = false;
        body = config.GetComponent<Rigidbody2D>();
        body.simulated = false;
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        if(selfhitbox != null) selfhitbox.gameObject.SetActive(false);
        deacceleration = config.RecoveryDeaccel;
        config.SlowDown(deacceleration);
        
        //Killing enemies
        Cutscene cutscene = config.GetComponentInParent<Cutscene>();
        if (cutscene != null) cutscene.EnemyDead();
    }

    public override void UpdateState()
    {
        if(config.Speed != 0)
        {
            deacceleration += config.RecoveryDeaccel * Time.deltaTime;
            config.SlowDown(deacceleration);
        }
        CheckSwitchStates();
    }

    public override void ExitState() {
        Debug.Log("exiting");
        config.invincible = false;
        config.dead = false;
        body = config.GetComponent<Rigidbody2D>();
        body.simulated = true;
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        if(selfhitbox != null) selfhitbox.gameObject.SetActive(true);
    }

    public override void CheckSwitchStates(){
        if (InputManager.Instance.ResetPressed) {
            Debug.Log("reset pressed");
            SwitchStates(factory.Idle());
            config.Reset();
        }
    }
}

// Grabbed
public class EnemyGrabbedState : EnemyState {
    public EnemyGrabbedState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    private Rigidbody2D body;

    public override void EnterState() {
        config.Velocity = Vector3.zero;
        config.grabbed = true;
        body = config.GetComponent<Rigidbody2D>();
        body.simulated = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.grabbed = false;
    }    

    public override void CheckSwitchStates(){
        if(config.projectile) SwitchStates(factory.Projectile());
    }
}

// Stunned (Grabbable)
public class EnemyStunnedState : EnemyState {

    public EnemyStunnedState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.grabbable = true;
        config.stunned = true;
        Renderer renderer = config.GetComponent<Renderer>();
        renderer.material.SetInt("_Active", 1);
    }

    public override void UpdateState() {
        if(config.Speed > 0){
            config.SlowDown(config.RecoveryDeaccel);
        }
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Stun = 0;
        config.stunned = false;
        config.grabbable = false;
        Renderer renderer = config.GetComponent<Renderer>();
        renderer.material.SetInt("_Active", 0);
    }

    public override void CheckSwitchStates() {
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.grabbed) SwitchStates(factory.Grabbed());
        else if(!config.stunned){
            if(config.target != null) SwitchStates(factory.Aggressive());
            else SwitchStates(factory.Idle());
        }
    }
}

// Projectile (Enemy acts as projectile)
public class EnemyProjectileState : EnemyState {

    private float deacceleration;
    private float recoverytimer = 1.5f;
    private HitboxController selfhitbox;
    private Rigidbody2D body;

    public EnemyProjectileState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.attackhitbox.SetActive(true);
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        selfhitbox.gameObject.layer = LayerMask.NameToLayer("Spit Projectile Hitbox");
        body = config.GetComponent<Rigidbody2D>();
        config.gameObject.GetComponent<Collider2D>().enabled = false;
        config.projectile = true;
        body.simulated = true;
    }

    public override void UpdateState()
    {
        if(config.Speed != 0)
        {
            config.SlowDown(config.projectileslowdown);
        }
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.attackhitbox.SetActive(false);
        config.gameObject.GetComponent<Collider2D>().enabled = true;
        config.invincible = false;
        config.projectile = false;
    }

    public override void CheckSwitchStates(){
        if(!config.grounded && config.canFall) SwitchStates(factory.Falling());
        if(config.Speed == 0 && config.target != null){
            SwitchStates(factory.Aggressive());
        }
        else if(config.Speed == 0){
            SwitchStates(factory.Idle());
        }
    }
}

// Falling
public class EnemyFallingState : EnemyState
{
    private float _fallAnim;
    private Rigidbody2D character;
    public EnemyFallingState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "EnemyFallingState";
    }

    public override void EnterState(){
        _fallAnim = config.fallingAnimDuration;
        config.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        character = config.GetComponent<Rigidbody2D>();
        config.SlowDown(config.Deacceleration);
        character.gravityScale += config.gravity;
    }
    public override void UpdateState(){
        // config.SlowDown(config.Deacceleration/10f);
        _fallAnim -= Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState(){
        config.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }
    public override void CheckSwitchStates(){
        if (_fallAnim <= 0f) {
            //SwitchStates(factory.Dead());
            config.Hit(config.Health, 0, Vector3.zero, 0);
            character.gameObject.SetActive(false);
        }
    }
}
