using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    Vector3 playerPos = new Vector3(0,0,0);
    Vector2 res;
    SpriteRenderer sR;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MainCamera");
        sR = GetComponent<SpriteRenderer>();
        res = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        playerPos.z = 0;
        transform.position = playerPos;

        if(res.x != Screen.width || res.y != Screen.height) {
            res.x = Screen.width;
            res.y = Screen.height;

            float height = res.y;
            float width = res.x;

            if(height > width) {
                width = height / res.y * res.x;
            } else {
                height = width / res.x * res.y;
            }

            transform.localScale = new Vector3(width / sR.sprite.bounds.size.x / 64, height / sR.sprite.bounds.size.y / 64, 1);
        }
    }
}
