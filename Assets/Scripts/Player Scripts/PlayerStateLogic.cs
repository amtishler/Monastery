using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class PlayerState : State {
    protected PlayerConfig config;
    protected PlayerStateFactory factory;

    public PlayerState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(currentContext) {
        this.config = config;
        this.factory = stateFactory;
    }
}


/* ====================
       SUBCLASSES
==================== */

// Idle
public class PlayerIdleState : PlayerState {

    public PlayerIdleState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if (Input.GetMouseButtonDown(1))SwitchStates(factory.TongueCharge());
        else if (Input.GetMouseButtonDown(0)) SwitchStates(factory.Staff());
        else if (Input.GetKeyDown("f")) SwitchStates(factory.Kick());
        else if (Input.GetKeyDown("space")) SwitchStates(factory.JumpCharge());
        else if (Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical")!=0) SwitchStates(factory.Running());
    }
}


// Running
public class PlayerRunningState : PlayerState {

    public PlayerRunningState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        Move();
    }
    
    public override void UpdateState() {
        Move();
        CheckSwitchStates();
    }

    public override void ExitState() {
        // config.Speed = 0f;
        // config.Velocity = Vector3.zero;
    }

    public override void CheckSwitchStates() {
        if (Input.GetMouseButtonDown(1)) SwitchStates(factory.TongueCharge());
        else if (Input.GetMouseButtonDown(0)) SwitchStates(factory.Staff());
        else if (Input.GetKeyDown("f")) SwitchStates(factory.Kick());
        else if (Input.GetKeyDown("space")) SwitchStates(factory.JumpCharge());
        else if (config.Speed == 0) SwitchStates(factory.Idle());
    }

    // Helper function
    public void Move() {

        // finding direction
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        targetDir.Normalize();

        // moving
        if (targetDir == Vector3.zero) {
            config.Speed = config.Speed - config.Deacceleration;
            if (config.Speed <= config.MinimumSpeed) config.Speed = 0;
            targetDir = config.Velocity;
            targetDir.Normalize();
            config.Velocity = targetDir*config.Speed;
        } else {
            if (config.Speed < config.MinimumSpeed) config.Speed = config.MinimumSpeed;
            config.Speed = config.Speed + config.Acceleration;
            if (config.Speed > config.MaximumSpeed) config.Speed = config.MaximumSpeed;
            config.Velocity = targetDir*config.Speed;
        }

        // update sprite
        if (targetDir == Vector3.zero) return;
        config.RotateSprite(targetDir);
    }
}

// Tongue charge
public class PlayerTongueChargeState : PlayerState {

    float totalChargeTime;
    float chargeTime;

    public PlayerTongueChargeState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
    }

    public override void EnterState() {
        config.SlowDown(config.Deacceleration*2.5f);
        totalChargeTime = config.tongue.GetComponent<TongueController>().ChargeTime;
        chargeTime = 0f;
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration);
        chargeTime = chargeTime + Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Velocity = Vector3.zero;
    }

    public override void CheckSwitchStates() {
        if (chargeTime >= totalChargeTime) SwitchStates(factory.Tongue());
    }
}

// Tongue shooting
public class PlayerTongueState : PlayerState {

    TongueController tongue;

    public PlayerTongueState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        tongue = config.tongue.GetComponent<TongueController>();
        config.RotateSprite(config.GetMouseDirection());

        if(!tongue.holdingObject) config.tongue.SetActive(true);
    }

    public override void UpdateState() {
        if(!tongue.holdingObject) tongue.UpdateTongue();
        else tongue.spitObject();
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if (tongue.CheckIfFinished()) {
            SwitchStates(factory.Idle());
            config.tongue.SetActive(false);
        }
        else if (tongue.grabbed) SwitchStates(factory.Grabbing());
    }
}


// Grabbing (with tongue)
public class PlayerGrabbingState : PlayerState {

    TongueController tongue;

    public PlayerGrabbingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){
        tongue = config.tongue.GetComponent<TongueController>();
        Move();
    }

    public override void UpdateState() {
        Move();
        tongue.SetSpawn(config.transform.position);
        tongue.SetEndpoint();
        tongue.ResizeTongueBody();
        CheckSwitchStates();
    }

    public override void ExitState() {
        tongue.UnGrab();
    }
    public override void CheckSwitchStates() {
        if (!Input.GetMouseButton(1) || tongue.autoRetract){
            SwitchStates(factory.Tongue());
        }
    }

    // Helper function
    public void Move() {

        // finding direction
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        targetDir.Normalize();


        float distancetoobj = Vector3.Distance(tongue.transform.position, config.transform.position);

        // Tongue Moving, slower and no sprite update
        if (targetDir == Vector3.zero) {
            config.Speed = config.Speed - config.Deacceleration;
            if (config.Speed <= config.MinimumSpeed) config.Speed = 0;
            targetDir = config.Velocity;
            targetDir.Normalize();
            config.Velocity = targetDir*config.Speed;
        } else {
            if (config.Speed < config.MinimumSpeed) config.Speed = config.MinimumSpeed;
            config.Speed = config.Speed + config.Acceleration;
            if (config.Speed > tongue.PlayerMoveSpeed) config.Speed = tongue.PlayerMoveSpeed;
            config.Velocity = targetDir*config.Speed;
        }
    }
}


// Staff swinging
public class PlayerStaffState : PlayerState {

    StaffController staff;

    public PlayerStaffState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.RotateSprite(config.GetMouseDirection());
        staff = config.staff.GetComponent<StaffController>();
        config.staff.SetActive(true);
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration*2);
        staff.UpdateStaff();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.staff.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if (staff.Done()) SwitchStates(factory.Idle());
    }
}


// Kicking
public class PlayerKickState : PlayerState {

    KickController kick;
    
    public PlayerKickState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.SlowDown(config.Deacceleration);
        kick = config.kick.GetComponent<KickController>();
        config.kick.SetActive(true);
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration);
        kick.UpdateKick();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.kick.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if (kick.Done()) SwitchStates(factory.Idle());
    }
}


// Jump (charging)
public class PlayerJumpChargeState : PlayerState {
    
    float chargeTime;

    public PlayerJumpChargeState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.SlowDown(config.Deacceleration*2);
        chargeTime = 0f;
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration*2);
        chargeTime = chargeTime + Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates() {
        if (!Input.GetKey("space")) {
            if (chargeTime < config.JumpChargeTime) SwitchStates(factory.Idle());
            else SwitchStates(factory.Jumping());
        }
    }
}


// Jumping
public class PlayerJumpingState : PlayerState {

    Vector3 direction;
    float totalDist;
    float currentDist;
    bool finished;

    public PlayerJumpingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        finished = false;
        totalDist = config.JumpTotalDist;
        currentDist = 0f;
        direction = config.GetMouseDirection();
        config.RotateSprite(direction);
        Move();
    }

    public override void UpdateState() {
        Move();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Speed = 0f;
        config.Velocity = Vector3.zero;
    }

    public override void CheckSwitchStates() {
        if (finished) SwitchStates(factory.Idle());
    }

    public void Move() {
        if (currentDist < totalDist) {
            if (config.Speed < config.JumpMinimumSpeed) config.Speed = config.JumpMinimumSpeed;
            if (config.Speed < config.JumpMaximumSpeed) config.Speed = config.Speed + config.JumpAcceleration;
            config.Velocity = config.Speed*direction;
            currentDist = currentDist + config.Speed*Time.deltaTime;
        } else {
            config.Speed = config.Speed - config.JumpDeacceleration;
            config.Velocity = config.Speed*direction;
            if(config.Speed < config.JumpMinimumSpeed) finished = true;
        }
    }
}


// Hurt (invicibility)
public class PlayerHurtState : PlayerState
{

    TongueController tongue;

    public PlayerHurtState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){
        tongue = config.tongue.GetComponent<TongueController>();
        if(tongue.grabbed) tongue.UnGrab();
        config.tongue.SetActive(false);
    }

    public override void UpdateState()
    {
        config.SlowDown(config.RecoveryDeaccel);
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){
        if(config.Speed == 0) SwitchStates(factory.Idle());
    }
}


// Dead
public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
}


// Map open (or menu)
public class PlayerMapOpenState : PlayerState
{
    public PlayerMapOpenState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
}


// Teleporting
public class PlayerTeleportingState : PlayerState
{
    public PlayerTeleportingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
}


// Cutscene / upgrade
public class PlayerCutsceneState : PlayerState
{
    public PlayerCutsceneState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
}