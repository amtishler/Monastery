using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractableObject
{

    public GameObject drop;
    public int maxdrops;
    public bool dropused;
    private bool hit;
    public GameObject childdrop;

    public float dropy = -2;
    public float dropx = 2; 

    private Animator animator;
    private int numdropsused;

    void Start()
    {
        hit = false;
        dropused = false;
        numdropsused = 0;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Hit", hit);
        if(hit)
        {
            hit = false;
        }
        if(this.transform.childCount == 0 && numdropsused < maxdrops)
        {
            dropused = false;
        }
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        hit = true;
        if(!dropused && drop != null)
        {
            numdropsused++;
            dropused = true;
            childdrop = Instantiate(drop) as GameObject;
            childdrop.transform.parent = this.transform;

            Vector3 childpos = new Vector3(dropx, dropy, 0);
            childdrop.transform.position = this.transform.position + childpos;
        }
    }
}
