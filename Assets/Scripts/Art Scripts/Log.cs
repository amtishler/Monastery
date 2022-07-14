using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Log : MonoBehaviour
{
    GameObject playerGO;
    [SerializeField]
    Vector3 midpoint;
    [Range(0, 7)]
    public int direction; 
    [SerializeField]
    Sprite[] spriteList = new Sprite[8];

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerGO.transform.position.y >= transform.position.y + midpoint.y) {
            sR.sortingLayerName = "Foreground Trees";
        } else
        {
            sR.sortingLayerName = "Background Trees";
        }
        if(sR.sprite != spriteList[direction]) {
            sR.sprite = spriteList[direction];
        }
    }
}
