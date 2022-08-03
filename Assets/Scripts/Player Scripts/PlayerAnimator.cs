using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class PlayerAnimator : CharacterAnimator
{
    // All character animators already have idle and walk animations.
    [SerializeField] AnimationClip[] staffAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] kickAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] jumpChargeAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] jumpAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] spitAnimations = new AnimationClip[4];

    // Public Methods to change states
    // Note that Idle and Walk are in by default (inherited)

    public void UpdateStaffAnimation() {
        UpdateAnimation(staffAnimations);
    }

    public void UpdateKickAnimation() {
        UpdateAnimation(kickAnimations);
    }

    public void UpdateSpitAnimation() {
        UpdateAnimation(spitAnimations);
    }

    public void UpdateJumpChargeAnimation() {
        UpdateAnimation(jumpChargeAnimations);
    }

    public void UpdateJumpAnimation() {
        UpdateAnimation(jumpAnimations);
    }
}
