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
    protected void NewPoint() {
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
// PlayerTongue
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
        config.grounded = true;
        tongue = config.tongue.GetComponent<TongueController>();
        config.playerAnimator.UpdateIdleAnimation();
    }

    public override void UpdateState() {
        NewPoint();
        if (config.Velocity != Vector3.zero) config.SlowDown(config.Deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        // if(!config.grounded) SwitchStates(factory.Falling());
        if ((InputManager.Instance.TonguePressed && tongue.heldObject == null) || (!InputManager.Instance.TongueHeld && tongue.heldObject != null)) SwitchStates(factory.Tongue());
        if (InputManager.Instance.Move != Vector3.zero) SwitchStates(factory.Running());
        else if(tongue.heldObject == null) {
            if (InputManager.Instance.StaffPressed) SwitchStates(factory.Staff());
            else if (InputManager.Instance.KickPressed) SwitchStates(factory.KickCharge());
        }
    }
}


// Running
public class PlayerRunningState : PlayerState
{
    TongueController tongue;
    bool goingFast;

    public PlayerRunningState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory)
    {
        name = "PlayerRun";
    }

    public override void EnterState()
    {
        config.grounded = true;
        returnPoint = config.resetPosition;
        tongue = config.tongue.GetComponent<TongueController>();
        goingFast = false;
        if (config.Speed > config.MaximumSpeed) goingFast = true;
        Move();
    }
    
    public override void UpdateState()
    {
        Move();
        NewPoint();
        CheckSwitchStates();
        if (config.Speed < config.MaximumSpeed) goingFast = false;
    }

    public override void ExitState()
    {
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates()
    {
        if ((InputManager.Instance.TonguePressed && tongue.heldObject == null) || (!InputManager.Instance.TongueHeld && tongue.heldObject != null))
            SwitchStates(factory.Tongue());
        if (config.Speed == 0) SwitchStates(factory.Idle());
        else if(tongue.heldObject == null)
        {
            if (InputManager.Instance.StaffPressed) SwitchStates(factory.Staff());
            else if (InputManager.Instance.KickPressed) SwitchStates(factory.KickCharge());
            if(!config.grounded) SwitchStates(factory.Falling());
        }
    }

    // Helper function
    public void Move() 
    {
        // finding direction
        Vector3 targetDir = InputManager.Instance.Move;

        if (goingFast)
        {
            float speed = config.Speed;
            speed -= config.Deacceleration;
            config.Velocity = (targetDir*config.Acceleration + config.Velocity).normalized * speed;
        }
        else
        {
            if (targetDir == Vector3.zero)
            {
                config.Velocity -= config.Velocity.normalized * config.Deacceleration;
                if (config.Speed <= config.MinimumSpeed) config.Velocity = Vector3.zero;
            }
            else
            {
                config.Velocity += targetDir * config.Acceleration;
                if (config.Speed > config.MaximumSpeed) config.Velocity = config.Velocity.normalized * config.MaximumSpeed;
            }
        }

        //// moving
        //if (targetDir == Vector3.zero)
        //{
        //    if (config.Speed <= config.MinimumSpeed) config.Velocity = Vector3.zero;
        //    else config.Velocity -= config.Velocity.normalized * config.Deacceleration;
        //}
        //else
        //{
        //    config.Velocity += targetDir * config.Acceleration;
        //    if (config.Speed > config.MaximumSpeed && !goingFast)
        //    {
        //        config.Velocity = config.Velocity.normalized * config.MaximumSpeed;
        //    };
        //}

        // update sprite
        if (targetDir == Vector3.zero) return;
        config.RotateSprite(targetDir);
        config.playerAnimator.UpdateWalkAnimation();
    }
}


// Tongue shooting
public class PlayerTongueState : PlayerState {

    TongueController tongue;
    State buffer;

    public PlayerTongueState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory){
        name = "PlayerTongueShoot";
    }

    public override void EnterState() {
        config.grounded = true;
        buffer = factory.Idle();
        tongue = config.tongue.GetComponent<TongueController>();
        Vector3 direction = InputManager.Instance.Aim;
        config.SlowDown(tongue.Friction);
        config.RotateSprite(direction);

        if (!tongue.HoldingObject) {
            config.tongue.SetActive(true);
        }
        
    }

    public override void UpdateState() {
        NewPoint();
        config.SlowDown(config.Deacceleration);
        if(!tongue.HoldingObject) tongue.UpdateTongue();
        else tongue.spitObject();
        CheckSwitchStates();

        // buffering...
        if (InputManager.Instance.StaffPressed) buffer = factory.Staff();
        if (InputManager.Instance.KickHeld) buffer = factory.KickCharge();
        if (InputManager.Instance.TonguePressed) buffer = factory.Tongue();
        if (tongue.heldObject != null) buffer = factory.Running();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if (tongue.IsFinished) {
            tongue.UnGrab();
            SwitchStates(buffer);
            config.tongue.SetActive(false);
        }
        else if (tongue.Grabbed) SwitchStates(factory.Pulling());
    }
}


// Pulling (with tongue)
public class PlayerPullingState : PlayerState
{
    TongueController tongue;

    public PlayerPullingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory)
    {
        name = "PlayerPull";
    }

    public override void EnterState()
    {
        config.grounded = true;
        tongue = config.tongue.GetComponent<TongueController>();
    }

    public override void UpdateState()
    {
        NewPoint();
        tongue.PullPlayer();
        config.RotateSprite(tongue.Direction);
        config.playerAnimator.UpdateIdleAnimation();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        tongue.UnGrab();
    }

    public override void CheckSwitchStates()
    {
        if (tongue.IsFinished)
        {
            config.tongue.SetActive(false);
            config.Velocity = Vector3.zero;
            SwitchStates(factory.Idle());
        }
        if (!InputManager.Instance.TongueHeld)
        {
            SwitchStates(factory.Flying());
        }
    }
}


// Flying (with tongue)
public class PlayerFlyingState : PlayerState
{
    TongueController tongue;
    float deaccel;
    State buffer;

    public PlayerFlyingState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory)
    {
        name = "PlayerFly";
    }

    public override void EnterState()
    {
        tongue = config.tongue.GetComponent<TongueController>();
        tongue.RemovePlayerInputForce();
        deaccel = tongue.FlyingDeaccel;
    }

    public override void UpdateState()
    {
        // Movement updates
        config.Velocity -= config.Velocity.normalized*deaccel;
        if (config.IsTouchingWall) config.Velocity *= 0.25f;
        // Everything else 
        NewPoint();
        if (config.tongue.activeInHierarchy) tongue.UpdateTongue();
        if (tongue.IsFinished) { config.tongue.SetActive(false); }
        CheckSwitchStates();

        // buffering...
        if (InputManager.Instance.StaffPressed) buffer = factory.Staff();
        if (InputManager.Instance.TonguePressed) buffer = factory.Tongue();
        if (InputManager.Instance.KickHeld) buffer = factory.KickCharge();
        if (InputManager.Instance.Move != Vector3.zero) buffer = factory.Running();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (config.Speed < config.MinimumSpeed || InputManager.Instance.Move != Vector3.zero)
        {
            if (config.tongue.activeInHierarchy)
            {
                SwitchStates(factory.Tongue());
            }
            else
            {
                SwitchStates(factory.Running());
            }
        }
        if (!config.tongue.activeInHierarchy)
        {
            if (buffer != null) SwitchStates(buffer);
            if (config.Speed < config.MinimumSpeed) SwitchStates(factory.Running());
        }
        else if (config.Speed < config.MinimumSpeed) SwitchStates(factory.Tongue());
    }
}


// Staff swinging
public class PlayerStaffState : PlayerState
{
    Attack staff;

    public PlayerStaffState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory) { name = "PlayerStaff"; }

    public override void EnterState()
    {
        config.grounded = true;
        returnPoint = config.resetPosition;
        if (config.Velocity != Vector3.zero)
            config.Velocity += InputManager.Instance.GetAim() * config.MaximumSpeed;
        config.SlowDown(config.Deacceleration);
        Vector3 direction = InputManager.Instance.Aim;
        config.RotateSprite(direction);
        config.playerAnimator.UpdateStaffAnimation();
        staff = config.staff.GetComponent<Attack>();
        config.staff.SetActive(true);
    }

    public override void UpdateState()
    {
        config.SlowDown(staff.Friction);
        NewPoint();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        config.staff.SetActive(false);
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates()
    {
        if (staff.Done) SwitchStates(factory.Idle());
        if(!config.grounded) SwitchStates(factory.Falling());
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
        config.grounded = true;
        config.SlowDown(config.Deacceleration*2.5f);
        totalChargeTime = config.KickChargeTime;
        chargeTime = 0f;
    }

    public override void UpdateState() {
        Vector3 targetDir = InputManager.Instance.Aim;
        config.RotateSprite(targetDir);
        config.playerAnimator.UpdateJumpChargeAnimation();

        config.SlowDown(config.Deacceleration);
        NewPoint();
        chargeTime = chargeTime + Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Velocity = Vector3.zero;
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates() {
        if (!InputManager.Instance.KickHeld) {
            if (chargeTime <config.KickChargeTime) SwitchStates(factory.Idle());
            else SwitchStates(factory.Kick());
        }
        if(!config.grounded) SwitchStates(factory.Falling());
    }
}

// Kicking
public class PlayerKickState : PlayerState
{
    Attack kick;
    
    public PlayerKickState(PlayerConfig config, StateMachine currentContext, PlayerStateFactory stateFactory)
    : base(config, currentContext, stateFactory) { name = "PlayerKick"; }

    public override void EnterState()
    {
        config.grounded = true;
        Vector3 direction = InputManager.Instance.Aim;
        kick = config.kick.GetComponent<Attack>();
        config.SlowDown(kick.Friction);
        config.kick.SetActive(true);
        config.RotateSprite(direction);
        config.playerAnimator.UpdateKickAnimation();
    }

    public override void UpdateState()
    {
        config.SlowDown(config.Deacceleration*3);
        NewPoint();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        config.kick.SetActive(false);
        config.resetPosition = returnPoint;
    }

    public override void CheckSwitchStates()
    {
        if (kick.Done) SwitchStates(factory.Idle());
        if(!config.grounded) SwitchStates(factory.Falling());
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
        config.grounded = true;
        returnPoint = config.resetPosition;
        tongue = config.tongue.GetComponent<TongueController>();
        if(tongue.Grabbed) tongue.UnGrab();
        config.tongue.SetActive(false);
    }

    public override void UpdateState(){
        config.SlowDown(config.RecoveryDeaccel);
        NewPoint();
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
        InputManager.Instance.DeathMap();
        Debug.Log("You are dead :(");
    }
    public override void UpdateState(){
        config.SlowDown(config.Deacceleration);
        CheckSwitchStates();
    }
    public override void ExitState(){
        config.Reset();
        GameManager.Instance.ResetLevel();
    }
    public override void CheckSwitchStates(){
        if (InputManager.Instance.ResetPressed){
            SwitchStates(factory.Idle());
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

    public override void EnterState(){
        config.grounded = true;
    }
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

    public override void EnterState(){
        config.grounded = true;
    }
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
        config.grounded = true;
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
        config.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        character = config.GetComponent<Rigidbody2D>();
        cam = config.GetComponentInChildren<CinemachineVirtualCamera>();
        offset = new Vector3(0, 0, -10);
        config.SlowDown(config.Deacceleration);
        character.gravityScale += config.gravity;
    }
    public override void UpdateState(){
        _fallAnim -= Time.deltaTime;
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
            config.Velocity = Vector3.zero;
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