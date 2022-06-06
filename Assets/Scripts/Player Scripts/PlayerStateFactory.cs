using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory : StateFactory
{
    PlayerConfig config;
    public PlayerStateFactory(PlayerConfig config, PlayerStateMachine currentContext) : base(currentContext)
    {
        this.config = config;
    }

    public State Idle()
    {
        return new PlayerIdleState(config, context, this);
    }
    public State Running()
    {
        return new PlayerRunningState(config, context, this);
    }
    public State Tongue()
    {
        return new PlayerTongueState(config, context, this);
    }
    public State Grabbing()
    {
        return new PlayerGrabbingState(config, context, this);
    }
    public State Attacking()
    {
        return new PlayerAttackingState(config, context, this);
    }
    public State JumpCharge()
    {
        return new PlayerJumpChargeState(config, context, this);
    }
    public State Jumping()
    {
        return new PlayerJumpingState(config, context, this);
    }
    public State Hurt()
    {
        return new PlayerHurtState(config, context, this);
    }
    public State Dead()
    {
        return new PlayerDeadState(config, context, this);
    }
    public State MapOpen()
    {
        return new PlayerMapOpenState(config, context, this);
    }
    public State Teleporting()
    {
        return new PlayerTeleportingState(config, context, this);
    }
    public State Cutscene()
    {
        return new PlayerCutsceneState(config, context, this);
    }

}
