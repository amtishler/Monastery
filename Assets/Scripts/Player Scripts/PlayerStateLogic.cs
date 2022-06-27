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

    TongueController tongue;

    public PlayerIdleState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerIdle";
    }

    public override void EnterState() {
        tongue = config.tongue.GetComponent<TongueController>();
        config.playerAnimator.UpdateIdleAnimation();
    }

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if ((config.Input.TonguePressed && tongue.heldObject == null) || (!config.Input.TongueHeld && tongue.heldObject != null)) SwitchStates(factory.TongueCharge());
        if (config.Input.Move != Vector3.zero) SwitchStates(factory.Running());
        else if(tongue.heldObject == null) {
            if (config.Input.StaffPressed) SwitchStates(factory.Staff());
            else if (config.Input.KickPressed) SwitchStates(factory.Kick());
            else if (config.Input.JumpPressed) SwitchStates(factory.JumpCharge());
        }
    }
}


// Running
public class PlayerRunningState : PlayerState {

    TongueController tongue;

    public PlayerRunningState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerRun";
    }

    public override void EnterState() {
        tongue = config.tongue.GetComponent<TongueController>();
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
        if ((config.Input.TonguePressed && tongue.heldObject == null) || (!config.Input.TongueHeld && tongue.heldObject != null)) SwitchStates(factory.TongueCharge());
        if (config.Speed == 0) SwitchStates(factory.Idle());
        else if(tongue.heldObject == null) {
            if (config.Input.StaffPressed) SwitchStates(factory.Staff());
            else if (config.Input.KickPressed) SwitchStates(factory.Kick());
            else if (config.Input.JumpPressed) SwitchStates(factory.JumpCharge());
        }
    }

    // Helper function
    public void Move() {

        // finding direction
        Vector3 targetDir = config.Input.Move;

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
        config.playerAnimator.UpdateWalkAnimation();
    }
}

// Tongue charge
public class PlayerTongueChargeState : PlayerState {

    float totalChargeTime;
    float chargeTime;

    public PlayerTongueChargeState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerTongueCharge";
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
    : base(config, currentContext, stateFactory){
        name = "PlayerTongueShoot";
    }

    public override void EnterState() {
        tongue = config.tongue.GetComponent<TongueController>();
        Vector3 direction = config.Input.Aim;
        config.RotateSprite(direction);

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
    : base(config, currentContext, stateFactory){
        name = "PlayerGrab";
    }

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
        if (!config.Input.TongueHeld || tongue.autoRetract){
            SwitchStates(factory.Tongue());
        }
    }

    // Helper function
    public void Move() {

        // finding direction
        Vector3 targetDir = config.Input.Move;

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
    : base(config, currentContext, stateFactory){
        name = "PlayerStaff";
    }

    public override void EnterState() {
        Vector3 direction = config.Input.Aim;
        config.RotateSprite(direction);
        staff = config.staff.GetComponent<StaffController>();
        config.staff.SetActive(true);
        config.playerAnimator.UpdateStaffAnimation();
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
    : base(config, currentContext, stateFactory){
        name = "PlayerKick";
    }

    public override void EnterState() {
        config.SlowDown(config.Deacceleration*3);
        Vector3 direction = config.Input.Aim;
        kick = config.kick.GetComponent<KickController>();
        config.kick.SetActive(true);
        config.RotateSprite(direction);
        config.playerAnimator.UpdateKickAnimation();
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration*3);
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
    : base(config, currentContext, stateFactory){
        name = "PlayerJumpCharge";
    }

    public override void EnterState() {
        config.SlowDown(config.Deacceleration*2);
        chargeTime = 0f;
        config.playerAnimator.UpdateJumpChargeAnimation();
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration*2);
        chargeTime = chargeTime + Time.deltaTime;
        config.RotateSprite(config.Input.Aim);
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates() {
        if (!config.Input.JumpHeld) {
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
    : base(config, currentContext, stateFactory){
        name = "PlayerJump";
    }

    public override void EnterState() {
        finished = false;
        totalDist = config.JumpTotalDist;
        currentDist = 0f;
        direction = config.Input.Aim;
        Debug.Log(direction);
        config.RotateSprite(direction);
        Move();
        config.playerAnimator.UpdateJumpAnimation();
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
    : base(config, currentContext, stateFactory){
        name = "PlayerHurt";
    }

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
    : base(config, currentContext, stateFactory){
        name = "PlayerDead";
    }

    public override void EnterState(){
        Debug.Log("You are dead :(");
    }
    public override void UpdateState()
    {
        config.SlowDown(config.Deacceleration);
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){
        InputHandler deadstate = config.GetComponent<InputHandler>();
        deadstate.DeathMap();
        if (config.Input.ResetPressed){
            GameManager.Instance.ReloadScene();
        }
    }
}


// Map open (or menu)
public class PlayerMapOpenState : PlayerState
{
    public PlayerMapOpenState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerMapOpen";
    }

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
    : base(config, currentContext, stateFactory){
        name = "PlayerTeleport";
    }

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
    : base(config, currentContext, stateFactory){
        name = "PlayerCutscene";
    }

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
}