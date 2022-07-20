using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class PlayerState : State {
    protected PlayerConfig config;
    protected PlayerStateFactory factory;

    protected Vector3 returnPoint;
    protected float updatePoint = 0f;

    public PlayerState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(currentContext) {
        this.config = config;
        this.factory = stateFactory;
    }

    //Helper function
protected void newPoint() {
    updatePoint -= Time.deltaTime;
    if (updatePoint <= 0 && config.grounded) {
        returnPoint = config.gameObject.transform.position;
        updatePoint = 0.5f;
    }
    //Debug.Log(returnPoint);
}
}


/* ====================
       SUBCLASSES
==================== */

// State Names:
// PlayerIdle
// PlayerRun
// PlayerTongueCharge
// PlayerTongueShoot
// PlayerStaff
// PlayerKickCharge
// PlayerKick
// PlayerJumpChargeState
// PlayerJumpState
// PlayerHurtState
// PlayerDeadState
// PlayerMapOpenState
// PlayerTeleportingState
// PlayerCutsceneState
// PlayerFall

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
        newPoint();
        if (config.Velocity != Vector3.zero) config.SlowDown(config.Deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        // if(!config.grounded) SwitchStates(factory.Falling());
        if ((InputManager.Instance.TonguePressed && tongue.heldObject == null) || (!InputManager.Instance.TongueHeld && tongue.heldObject != null)) SwitchStates(factory.TongueCharge());
        if (InputManager.Instance.Move != Vector3.zero) SwitchStates(factory.Running());
        else if(tongue.heldObject == null) {
            if (InputManager.Instance.StaffPressed) SwitchStates(factory.Staff());
            else if (InputManager.Instance.KickPressed) SwitchStates(factory.KickCharge());
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
        returnPoint = config.resetPosition;
        tongue = config.tongue.GetComponent<TongueController>();
        Move();
    }
    
    public override void UpdateState() {
        Move();
        newPoint();
        CheckSwitchStates();
    }

    public override void ExitState() {
        // config.Speed = 0f;
        // config.Velocity = Vector3.zero;
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates() {
        if(!config.grounded) SwitchStates(factory.Falling());
        if ((InputManager.Instance.TonguePressed && tongue.heldObject == null) || (!InputManager.Instance.TongueHeld && tongue.heldObject != null)) SwitchStates(factory.TongueCharge());
        if (config.Speed == 0) SwitchStates(factory.Idle());
        else if(tongue.heldObject == null) {
            if (InputManager.Instance.StaffPressed) SwitchStates(factory.Staff());
            else if (InputManager.Instance.KickPressed) SwitchStates(factory.KickCharge());
        }
    }

    // Helper function
    public void Move() {

        // finding direction
        Vector3 targetDir = InputManager.Instance.Move;

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
        returnPoint = config.resetPosition;
        config.SlowDown(config.Deacceleration*2.5f);
        totalChargeTime = config.tongue.GetComponent<TongueController>().ChargeTime;
        chargeTime = 0f;
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration);
        newPoint();
        chargeTime = chargeTime + Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Velocity = Vector3.zero;
        config.resetPosition = returnPoint;
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
        Vector3 direction = InputManager.Instance.Aim;
        // config.RotateSprite(direction);

        if(!tongue.holdingObject) {
            config.tongue.SetActive(true);
        }
        
    }

    public override void UpdateState() {
        newPoint();
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
    }

    public override void UpdateState() {
        newPoint();
        tongue.PullPlayer();
        CheckSwitchStates();
        Debug.Log(InputManager.Instance.TongueHeld);
    }

    public override void ExitState() {
        config.Speed = 0f;
        tongue.UnGrab();
    }

    public override void CheckSwitchStates() {
        if (!InputManager.Instance.TongueHeld || tongue.autoRetract){
            SwitchStates(factory.Tongue());
        }
    }

    // Helper function
    public void Move() {

        // finding direction
        Vector3 targetDir = InputManager.Instance.Move;

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
        returnPoint = config.resetPosition;
        config.Velocity = InputManager.Instance.GetAim()*config.Speed;
        config.SlowDown(config.Deacceleration);
        Vector3 direction = InputManager.Instance.Aim;
        config.RotateSprite(direction);
        config.playerAnimator.UpdateStaffAnimation();
        staff = config.staff.GetComponent<StaffController>();
        config.staff.SetActive(true);
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration);
        newPoint();
        staff.UpdateStaff();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.staff.SetActive(false);
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates() {
        if(!config.grounded) SwitchStates(factory.Falling());
        if (staff.Done()) SwitchStates(factory.Idle());
    }
}

// Kick Charge
public class PlayerKickChargeState : PlayerState {

    float totalChargeTime;
    float chargeTime;

    public PlayerKickChargeState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerKickCharge";
    }

    public override void EnterState() {
        config.SlowDown(config.Deacceleration*2.5f);
        totalChargeTime = config.kick.GetComponent<KickController>().KickChargeTime;
        chargeTime = 0f;
    }

    public override void UpdateState() {
        Vector3 targetDir = InputManager.Instance.Aim;
        config.RotateSprite(targetDir);
        config.playerAnimator.UpdateJumpChargeAnimation();

        config.SlowDown(config.Deacceleration);
        newPoint();
        chargeTime = chargeTime + Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Velocity = Vector3.zero;
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates() {
        if(!config.grounded) SwitchStates(factory.Falling());
        if (!InputManager.Instance.KickHeld) {
            if (chargeTime <config.kick.GetComponent<KickController>().KickChargeTime) SwitchStates(factory.Idle());
            else SwitchStates(factory.Kick());
        }
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
        Vector3 direction = InputManager.Instance.Aim;
        kick = config.kick.GetComponent<KickController>();
        config.kick.SetActive(true);
        config.RotateSprite(direction);
        config.playerAnimator.UpdateKickAnimation();
    }

    public override void UpdateState() {
        config.SlowDown(config.Deacceleration*3);
        newPoint();
        kick.UpdateKick();
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.kick.SetActive(false);
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates() {
        if(!config.grounded) SwitchStates(factory.Falling());
        if (kick.Done()) SwitchStates(factory.Idle());
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
        returnPoint = config.resetPosition;
        tongue = config.tongue.GetComponent<TongueController>();
        if(tongue.grabbed) tongue.UnGrab();
        config.tongue.SetActive(false);
    }

    public override void UpdateState(){
        config.SlowDown(config.RecoveryDeaccel);
        newPoint();
        CheckSwitchStates();
    }
    public override void ExitState(){
        config.resetPosition = returnPoint;
    }
    public override void CheckSwitchStates(){
        if(config.Speed == 0) SwitchStates(factory.Idle());
        if(!config.grounded) SwitchStates(factory.Falling());
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
    public override void UpdateState(){
        config.SlowDown(config.Deacceleration);
        CheckSwitchStates();
    }
    public override void ExitState(){
    }
    public override void CheckSwitchStates(){
        InputManager.Instance.DeathMap();
        if (InputManager.Instance.ResetPressed){
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

    public override void EnterState(){
        config.playerAnimator.UpdateIdleAnimation();
        InputManager.Instance.CutsceneMap();
        config.Velocity = Vector3.zero;
    }
    public override void UpdateState() {
        CheckSwitchStates();
    }
    public override void ExitState(){
        InputManager.Instance.CombatMap();
    }
    public override void CheckSwitchStates() {}
}

// Falling
public class PlayerFall : PlayerState
{
    private float _fallAnim;
    private Rigidbody2D character;
    private CinemachineVirtualCamera cam;
    private Vector3 offset;
    public PlayerFall(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerFall";
    }

    public override void EnterState(){
        _fallAnim = config.fallingAnimDuration;
        config.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        character = config.GetComponent<Rigidbody2D>();
        cam = config.GetComponentInChildren<CinemachineVirtualCamera>();
        offset = new Vector3(0, 0, -10);
        config.SlowDown(config.Deacceleration);
        character.gravityScale += config.gravity;
    }
    public override void UpdateState(){
        // config.SlowDown(config.Deacceleration/10f);
        // newPoint();
        _fallAnim -= Time.deltaTime;
        // if (offset.y < 1f) offset.y += Time.deltaTime;
        // else offset.y += Time.deltaTime*3f;
        offset.y += Time.deltaTime*config.gravity*2;
        cam.GetComponent<CinemachineCameraOffset>().m_Offset = offset;
        CheckSwitchStates();
    }
    public override void ExitState(){
        config.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        character.gravityScale = 0f;
        cam.GetComponent<CinemachineCameraOffset>().m_Offset = Vector3.zero;
    }
    public override void CheckSwitchStates(){
        if (_fallAnim <= 0) {
            config.Speed = 0;
            config.Hit(config.fallDamage, 0, Vector3.zero, 0);
            if(config.Health <= 0) {
                SwitchStates(factory.Dead());
            } else {
                // Debug.Log("Reset: " + config.resetPosition);
                config.gameObject.transform.position = config.resetPosition;
                config.grounded = true;
                SwitchStates(factory.Idle());
            }
        }
    }
}