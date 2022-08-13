using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssister : MonoBehaviour
{
    [SerializeField] float distanceToPlayer;
    PlayerConfig config;
    SpriteRenderer sprite;
    Vector3 defaultAim;

    // Start is called before the first frame update
    void Start()
    {
        config = GetComponentInParent<PlayerConfig>();
        defaultAim = (new Vector3(-1,-1,0)).normalized;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputManager.Instance.UsingController()) { sprite.enabled = false; return; }
        sprite.enabled = true;
        Vector3 aim = InputManager.Instance.Aim;
        transform.position = config.transform.position + aim * distanceToPlayer;

        float angle = Vector3.Angle(aim, Vector3.right);
        bool down = false;
        if (Vector3.Dot(aim, Vector3.up) < 0) down = true;
        if (down) angle = -angle;
        transform.rotation = Quaternion.Euler(0, 0, 135 + angle);
    }
}
