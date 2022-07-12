using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractableObject
{

    public GameObject drop;
    public int maxdrops;
    public bool dropused;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        animator.Play("Tree1-Hit");
    }
}
