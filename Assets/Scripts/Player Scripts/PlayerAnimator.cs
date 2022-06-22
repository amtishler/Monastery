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

    int currentdir;

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
        currentdir = config.currentdir;
        if(staff || kick) {
            if(m_anim.GetNormalisedTime() >= 1f) {
                UpdateIdleAnimation();
            }
        }
    }

    public void UpdateWalkAnimation() {
        if(m_anim.Clip != walkAnimations[currentdir]) {
            m_anim.Play(walkAnimations[currentdir]);
        }
    }

    public void UpdateIdleAnimation() {
        if(m_anim.Clip != idleAnimations[currentdir] && m_anim.GetNormalisedTime() >= 1f && !jumpCharge) {
            m_anim.Play(idleAnimations[currentdir]);
        }
    }

    public void UpdateStaffAnimation() {
        if(m_anim.Clip != staffAnimations[currentdir]) {
            m_anim.Play(staffAnimations[currentdir]);
            staff = true;
        }
    }

    public void UpdateKickAnimation() {
        if(m_anim.Clip != kickAnimations[currentdir]) {
            m_anim.Play(kickAnimations[currentdir]);
            kick = true;
        }
    }

    public void UpdateSpitAnimation() {
        if(m_anim.Clip != spitAnimations[currentdir]) {
            m_anim.Play(spitAnimations[currentdir]);
        }
    }

    public void UpdateJumpChargeAnimation() {
        if(m_anim.Clip != jumpChargeAnimations[currentdir]) {
            m_anim.Play(jumpChargeAnimations[currentdir]);
            jumpCharge = true;
        }
    }

    public void UpdateJumpAnimation() {
        if(m_anim.Clip != jumpAnimations[currentdir]) {
            m_anim.Play(jumpAnimations[currentdir]);
            jumpCharge = false;
        }
    }
}
