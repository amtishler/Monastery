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
        if(config.stunned) SwitchStates(factory.Stunned());
        else if(CheckVision()) SwitchStates(factory.Aggressive());
    }

    private bool CheckVision()
    {
        Collider2D collider = Physics2D.OverlapCircle(config.transform.position, config.detectionradius, LayerMask.GetMask("Player Hurtbox"));
        if(collider == null) return false;
        else{
            config.target = collider.gameObject;
            return true;
        }
    }
}

// Aggressive
public class EnemyAggressiveState : EnemyState {

    public EnemyAggressiveState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        Debug.Log("GRRR");
        if(config.target == null) SwitchStates(factory.Idle());
    }

    public override void UpdateState() {
        config.MoveTowards(config.target);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if(config.grabbed) SwitchStates(factory.Grabbed());
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
    }

    public override void UpdateState()
    {
        deacceleration += config.RecoveryDeaccel * Time.deltaTime;
        config.SlowDown(deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates(){
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

    public EnemyDeadState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.invincible = true;
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        selfhitbox.gameObject.SetActive(false);
        deacceleration = config.RecoveryDeaccel;
        config.SlowDown(deacceleration);
    }

    public override void UpdateState()
    {
        if(config.Speed != 0)
        {
            deacceleration += config.RecoveryDeaccel * Time.deltaTime;
            config.SlowDown(deacceleration);
        }
    }

    public override void ExitState() {}

    public override void CheckSwitchStates(){

    }
}

// Grabbed
public class EnemyGrabbedState : EnemyState {
    public EnemyGrabbedState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    private HitboxController selfhitbox;

    public override void EnterState() {
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        selfhitbox.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.grabbed = false;
        selfhitbox.gameObject.SetActive(true);
    }    

    public override void CheckSwitchStates(){
        if(config.stunned) SwitchStates(factory.Stunned());
    }
}

// Stunned (Enemy acts as projectile)
public class EnemyStunnedState : EnemyState {

    private float deacceleration;
    private float recoverytimer = 1.5f;
    private HitboxController selfhitbox;

    public EnemyStunnedState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        selfhitbox = config.GetComponentInChildren<HitboxController>();
        selfhitbox.gameObject.SetActive(true);
        selfhitbox.gameObject.layer = LayerMask.NameToLayer("Spit Projectile Hitbox");
        config.SlowDown(deacceleration);
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
        selfhitbox.gameObject.layer = LayerMask.NameToLayer("Enemy Hitbox");
        config.invincible = false;
    }

    public override void CheckSwitchStates(){
        if(config.Speed == 0 && config.target != null){
            config.stunned = false;
            SwitchStates(factory.Aggressive());
        }
        else if(config.Speed == 0){
            config.stunned = false;
            SwitchStates(factory.Idle());
        }
    }
}

