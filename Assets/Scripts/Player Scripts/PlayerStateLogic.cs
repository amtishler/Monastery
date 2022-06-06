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
        if (Input.GetMouseButtonDown(1)) SwitchStates(factory.Tongue());
        else if (Input.GetMouseButtonDown(0)) SwitchStates(factory.Attacking());
        else if (Input.GetKeyDown("space")) SwitchStates(factory.JumpCharge());
        else if (Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical")!=0) SwitchStates(factory.Running());
    }

    public override void InitializeSubState() {}
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
        config.Speed = 0f;
        config.Velocity = Vector3.zero;
    }

    public override void CheckSwitchStates() {
        if (Input.GetMouseButtonDown(1)) SwitchStates(factory.Tongue());
        else if (Input.GetMouseButtonDown(0)) SwitchStates(factory.Attacking());
        else if (Input.GetKeyDown("space")) SwitchStates(factory.JumpCharge());
        else if (config.Speed == 0) SwitchStates(factory.Idle());
    }

    public override void InitializeSubState(){}

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
        config.Step();

        // update sprite
        if (targetDir == Vector3.zero) return;
        config.RotateSprite(targetDir);
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
        config.tongue.SetActive(true);
    }

    public override void UpdateState() {
        tongue.UpdateTongue();
        CheckSwitchStates();
    }

    public override void ExitState() {
        Debug.Log("Here");
        config.tongue.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if (tongue.Done()) SwitchStates(factory.Idle());
    }

    public override void InitializeSubState() {}
}


// Grabbing (with tongue)
public class PlayerGrabbingState : PlayerState {
    public PlayerGrabbingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState() {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
    public override void InitializeSubState(){}
}


// Attacking (staff)
public class PlayerAttackingState : PlayerState {

    StaffController staff;

    public PlayerAttackingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.RotateSprite(config.GetMouseDirection());
        staff = config.staff.GetComponent<StaffController>();
        config.staff.SetActive(true);
    }

    public override void UpdateState() {
        staff.UpdateStaff();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.staff.SetActive(false);
    }

    public override void CheckSwitchStates() {
        if (staff.Done()) SwitchStates(factory.Idle());
    }

    public override void InitializeSubState(){}
}


// Jump (charging)
public class PlayerJumpChargeState : PlayerState {
    
    float chargeTime;

    public PlayerJumpChargeState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        chargeTime = 0f;
    }

    public override void UpdateState() {
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

    public override void InitializeSubState(){}
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

    public override void InitializeSubState() {}

    public void Move() {
        if (currentDist < totalDist) {
            if (config.Speed < config.JumpMinimumSpeed) config.Speed = config.JumpMinimumSpeed;
            if (config.Speed < config.JumpMaximumSpeed) config.Speed = config.Speed + config.JumpAcceleration;
            config.Velocity = config.Speed*direction;
            config.Step();
            currentDist = currentDist + config.Speed*Time.deltaTime;
        } else {
            config.Speed = config.Speed - config.JumpDeacceleration;
            config.Velocity = config.Speed*direction;
            config.Step();
            if(config.Speed < config.JumpMinimumSpeed) finished = true;
        }
    }
}


// Hurt (invicibility)
public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
    public override void InitializeSubState(){}
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
    public override void InitializeSubState(){}
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
    public override void InitializeSubState(){}
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
    public override void InitializeSubState(){}
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
    public override void InitializeSubState(){}
}