using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingData : MonoBehaviour {
    public HoldingMethod holdingMethod;
    public bool dropOnRelease = true;

    public bool isStatic;
    public bool isMainHeldLocally;
    public bool isDirectionHeldLocally;
    public bool isHeldByOther;

    [HideInInspector]
    public Rigidbody rb;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;


    public float fowardSlidePercent = 0; // Range(0,1)
    public float slideMaxBack = 1;
    [Tooltip("Only for bolt action")]
    public float boltMaxUp = 45;

    public Gun gun;

    void Start()
    {
        if (!GetComponentInChildren<CollisionRelay>())
            gameObject.AddComponent<CollisionRelay>().rootObject = this;

        rb = GetComponent<Rigidbody>();

        gun = GetComponentInParent<Gun>();
    }
    public bool CanBeGrabbed(bool isDirectionHandle)
    {
        if (isStatic)
            return false;
        if (isDirectionHandle && isDirectionHeldLocally)
            return false;
        if (!isDirectionHandle && isMainHeldLocally)
            return false;
        return true;

    }
    public enum HoldingMethod
    {
        MainHandle, DirectionHandle, DirectionSlideHandle, SlideHandle, RotationHandle, GrabHandle
        /*
         * MainHandle - First grip of long weapon / The only grip of a banana
         * DirectionHandle - Where is a rifle looking / Orientation of pool cue
         * DirectionSlideHandle - Pump shotgun / Something else
         * SlideHandle - Bolt action / Door lock
         * RotationHandle - Bolt action / Lever
         * GrabHandle - Moving chest around / Heavy objects
         */
    }
}
