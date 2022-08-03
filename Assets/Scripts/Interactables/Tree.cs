using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractableObject
{

    public GameObject drop;
    public int maxdrops;
    public bool dropused;
    private bool hit;
    private List<GameObject> childdrop;

    public float dropy = -2f;
    public float dropx = 2f; 

    public float xrangetree = 2f;
    public float yrangetree = 3f;

    private Animator animator;
    private int numdropsused;

    void Start()
    {
        childdrop = new List<GameObject>();
        hit = false;
        dropused = true;
        numdropsused = 0;
        animator = GetComponent<Animator>();
        for (int i = 0; i < maxdrops; i++)
        {
            childdrop.Add(Instantiate(drop) as GameObject);
            childdrop[i].transform.parent = this.transform;
            Vector3 childpos = new Vector3(Random.Range(-xrangetree, xrangetree), Random.Range(1.5f-yrangetree, 1.5f+yrangetree), 0);
            childdrop[i].transform.position = this.transform.position + childpos;
            childdrop[i].GetComponent<Collider2D>().enabled = false;
        }
    }

    void Update()
    {
        animator.SetBool("Hit", hit);
        if(hit)
        {
            hit = false;
        }
        //Not counting wallcollision and shadowprefab
    }

    public override void OnHit(Vector3 dir, float mag)
    {
        hit = true;
        if(childdrop.Count != 0)
        {
            HealthDrop healthDrop = childdrop[0].GetComponent<HealthDrop>();
            healthDrop.Drop(dropy);
            childdrop.RemoveAt(0);
        }
    }
}
