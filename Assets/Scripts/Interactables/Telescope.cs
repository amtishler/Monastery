using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Telescope : StoryEvent
{
    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private PlayerConfig config;
    private bool activatable = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        if (previousEvent == null) activatable = true;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!activatable) return;
        config = other.gameObject.GetComponentInParent<PlayerConfig>();
        if (config != null) config.inTelescopeRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!activatable) return;
        PlayerConfig player = other.gameObject.GetComponentInParent<PlayerConfig>();
        if (player != null) player.inTelescopeRange = false;
    }

    public override void BeginStoryEvent()
    {
        activatable = true;
    }

    public override void EndStoryEvent()
    {
        if (nextEvent != null) nextEvent.BeginStoryEvent();
        nextEvent = null;
    }

    public StoryEvent GetNextEvent()
    {
        return nextEvent;
    }

}
