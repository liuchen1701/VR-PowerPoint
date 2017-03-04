﻿using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.PoseTracker;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using System;
using UnityEngine;
using UnityEngine.Events;

public class BasicGrabbable : MonoBehaviour
    , IColliderEventDragStartHandler
    , IColliderEventDragFixedUpdateHandler
    , IColliderEventDragUpdateHandler
    , IColliderEventDragEndHandler
{
    [Serializable]
    public class UnityEventGrabbable : UnityEvent<BasicGrabbable> { }

    public const float MIN_FOLLOWING_DURATION = 0.02f;
    public const float DEFAULT_FOLLOWING_DURATION = 0.04f;
    public const float MAX_FOLLOWING_DURATION = 0.5f;

    private OrderedIndexedTable<ColliderButtonEventData, Pose> eventList = new OrderedIndexedTable<ColliderButtonEventData, Pose>();

    public bool alignPosition;
    public bool alignRotation;
    [Range(MIN_FOLLOWING_DURATION, MAX_FOLLOWING_DURATION)]
    public float followingDuration = DEFAULT_FOLLOWING_DURATION;
    public bool overrideMaxAngularVelocity = true;

    [SerializeField]
    private ControllerButton m_grabButton = ControllerButton.Trigger;

    public UnityEventGrabbable afterGrabbed;
    public UnityEventGrabbable beforeRelease;

    public ControllerButton grabButton
    {
        get
        {
            return m_grabButton;
        }
        set
        {
            m_grabButton = value;
            // set all child MaterialChanger heighlightButton to value;
            var matChangers = ListPool<MaterialChanger>.Get();
            GetComponentsInChildren(matChangers);
            for (int i = matChangers.Count - 1; i >= 0; --i) { matChangers[i].heighlightButton = value; }
            ListPool<MaterialChanger>.Release(matChangers);
        }
    }

    public bool isGrabbed { get { return eventList.Count > 0; } }

    public ColliderButtonEventData grabbedEvent { get { return isGrabbed ? eventList.GetLastKey() : null; } }

    private Pose GetEventPose(ColliderButtonEventData eventData)
    {
        var grabberTransform = eventData.eventCaster.transform;
        return new Pose(grabberTransform);
    }

#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        grabButton = m_grabButton;
    }

#endif

    protected virtual void Start()
    {
        grabButton = m_grabButton;
    }

    public void OnColliderEventDragStart(ColliderButtonEventData eventData)
    {
        if (!eventData.IsViveButton(m_grabButton)) { return; }

        var casterPose = GetEventPose(eventData);
        var offsetPose = Pose.FromToPose(casterPose, new Pose(transform));

        if (alignPosition) { offsetPose.pos = Vector3.zero; }
        if (alignRotation) { offsetPose.rot = Quaternion.identity; }

        eventList.AddUniqueKey(eventData, offsetPose);

        afterGrabbed.Invoke(this);
    }

    public void OnColliderEventDragFixedUpdate(ColliderButtonEventData eventData)
    {
        if (eventData != grabbedEvent) { return; }

        var rigid = GetComponent<Rigidbody>();
        if (ReferenceEquals(rigid, null)) { return; }

        // if rigidbody exists, follow eventData caster using physics
        var casterPose = GetEventPose(eventData);
        var offsetPose = eventList.GetLastValue();
        var targetPose = casterPose * offsetPose;

        // applying velocity
        var diffPos = targetPose.pos - rigid.position;
        if (Mathf.Approximately(diffPos.sqrMagnitude, 0f))
        {
            rigid.velocity = Vector3.zero;
        }
        else
        {
            rigid.velocity = diffPos / Mathf.Clamp(followingDuration, MIN_FOLLOWING_DURATION, MAX_FOLLOWING_DURATION);
        }

        // applying angular velocity
        float angle;
        Vector3 axis;
        (targetPose.rot * Quaternion.Inverse(rigid.rotation)).ToAngleAxis(out angle, out axis);
        while (angle > 360f) { angle -= 360f; }

        if (Mathf.Approximately(angle, 0f) || float.IsNaN(axis.x))
        {
            rigid.angularVelocity = Vector3.zero;
        }
        else
        {
            angle *= Mathf.Deg2Rad / Mathf.Clamp(followingDuration, MIN_FOLLOWING_DURATION, MAX_FOLLOWING_DURATION); // convert to radius speed
            if (overrideMaxAngularVelocity && rigid.maxAngularVelocity < angle) { rigid.maxAngularVelocity = angle; }
            rigid.angularVelocity = axis * angle;
        }
    }

    public void OnColliderEventDragUpdate(ColliderButtonEventData eventData)
    {
        if (eventData != grabbedEvent) { return; }

        if (!ReferenceEquals(GetComponent<Rigidbody>(), null)) { return; }

        // if rigidbody doesn't exist, just move to eventData caster's pose
        var casterPose = GetEventPose(eventData);
        var offsetPose = eventList.GetLastValue();
        var targetPose = casterPose * offsetPose;

        transform.position = targetPose.pos;
        transform.rotation = targetPose.rot;
    }

    public void OnColliderEventDragEnd(ColliderButtonEventData eventData)
    {
        var released = eventData == grabbedEvent;
        if (released) { beforeRelease.Invoke(this); }

        eventList.Remove(eventData);

        if (released && isGrabbed) { afterGrabbed.Invoke(this); }
    }
}
