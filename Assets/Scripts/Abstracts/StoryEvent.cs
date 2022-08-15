using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent : MonoBehaviour
{
    [Header("Before and after triggers. If blank, they're automatic.")]
    [SerializeField] protected StoryEvent previousEvent;
    [SerializeField] protected StoryEvent nextEvent;
    protected CameraController cameraController;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        _Awake();
    }

    protected virtual void _Awake() { }
    public virtual void BeginStoryEvent() { }
    public virtual void EndStoryEvent() { }
}
