using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Log : MonoBehaviour
{
    GameObject playerGO;
    [Range(0, 7)]
    public int direction; 
    [SerializeField]
    Sprite[] spriteList = new Sprite[8];

    //Transform hitboxChild;

    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        //hitboxChild = transform.GetChild(0);
        //hitboxChild.rotation = Quaternion.Euler(0, 0, 45 * direction);

        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(sR.sprite != spriteList[direction]) {
            sR.sprite = spriteList[direction];
            //hitboxChild.rotation = Quaternion.Euler(0, 0, 45 * direction);
        }
    }
}
