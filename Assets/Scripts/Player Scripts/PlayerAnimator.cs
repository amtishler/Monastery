using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animations")] // 0 right, 1 up, 2 left, 3 down
    [SerializeField] AnimationClip[] idleAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] walkAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] staffAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] kickAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] jumpChargeAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] jumpAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] spitAnimations = new AnimationClip[4];

    protected SpriteAnim m_anim = null;
    protected PlayerConfig config;

    bool kick = false;
    bool staff = false;
    bool jumpCharge = false;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponentInParent<SpriteAnim>();
        config = GetComponentInParent<PlayerConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        if (config.State == "PlayerStaff" || config.State == "PlayerKick") {
            if(m_anim.GetNormalisedTime() >= 1f) {
                UpdateIdleAnimation();
            }
        }
    }

    void UpdateAnimation(AnimationClip[] animation) {
        if(m_anim.Clip != animation[config.currentdir]) {
            m_anim.Play(animation[config.currentdir]);
        }
    }

    // Public Methods to change states
    
    public void UpdateWalkAnimation() {
        UpdateAnimation(walkAnimations);
    }

    public void UpdateIdleAnimation() {
        UpdateAnimation(idleAnimations);
    }

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
