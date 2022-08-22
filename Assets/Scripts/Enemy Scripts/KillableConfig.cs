using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KillableConfig : CharacterConfig
{
    protected EnemyTracker tracker;

    public void RegisterTracker(EnemyTracker tracker)
    {
        this.tracker = tracker;
        Debug.Log(tracker == null);
    }

    public void KillInTracker()
    {
        if (tracker == null) return;
        tracker.DecreaseEnemies();
    }
}
