using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Animations")] // 0 right, 1 up, 2 left, 3 down
    [SerializeField] AnimationClip[] idleAnimations = new AnimationClip[4];
    [SerializeField] AnimationClip[] walkAnimations = new AnimationClip[4];

    protected SpriteAnim m_anim = null;
    protected CharacterConfig config;


    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponentInParent<SpriteAnim>();
        config = GetComponentInParent<CharacterConfig>();
    }

    protected void UpdateAnimation(AnimationClip[] animation)
    {
        if (m_anim == null) { Debug.Log("m_anim null"); return; }
        if (m_anim.Clip != animation[config.currentdir])
        {
            m_anim.Play(animation[config.currentdir]);
        }
    }

    // Public Methods to change states

    public void UpdateIdleAnimation()
    {
        UpdateAnimation(idleAnimations);
    }

    public void UpdateWalkAnimation()
    {
        UpdateAnimation(walkAnimations);
    }
}