using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabber : MonoBehaviour
{
    #region SteamVR trackedObj & Controller
    SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    #endregion
    bool isCurrentCollisionDirection;
    HoldingData currentCollision;
    public HoldingData heldObject;
    public ControllerGrabber otherController;
    bool isMainController;
    ControllerInteractor interactor;

    void Start()
    {
        interactor = GetComponent<ControllerInteractor>();
    }
    void LateUpdate()
    {
        if (heldObject && heldObject.isStatic)
            RemoveReferencesToObject();
        if (heldObject)
            MoveObject();
    }
    void Update()
    {
        if (heldObject && heldObject.isStatic)
            RemoveReferencesToObject();
        if (Controller.GetHairTriggerDown() && currentCollision && currentCollision.CanBeGrabbed(isCurrentCollisionDirection))
        {
            Grab();
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && heldObject)
        {
            Drop();
        }
        if (Controller.GetHairTriggerUp() && heldObject && heldObject.dropOnRelease)
            Drop();
    }
    void Grab()
    {
        heldObject = currentCollision;
        if (heldObject.rb)
            heldObject.rb.isKinematic = true;
        if (isCurrentCollisionDirection)
        {
            heldObject.isDirectionHeldLocally = true;
            isMainController = false;
        }
        else
        {
            heldObject.isMainHeldLocally = true;
            isMainController = true;
            interactor.StartInfluencingObject(heldObject.GetComponent<Interactions>());
        }
    }
    void Drop()
    {
        if (heldObject.rb && otherController.heldObject != heldObject)
        {
            heldObject.rb.isKinematic = false;
            heldObject.rb.velocity = Controller.velocity;
            heldObject.rb.angularVelocity = Controller.angularVelocity;
        }
            
        if (isCurrentCollisionDirection)
        {
            heldObject.isDirectionHeldLocally = false;
            heldObject = null;
        }
        else
        {
            heldObject.isMainHeldLocally = false;
            heldObject = null;
        }
        isMainController = false;
        interactor.StopInfluencingObject();
    }
    void RemoveReferencesToObject() // Just letting go, no parameters of an object changed - magazine attaching
    {
        if (isCurrentCollisionDirection)
        {
            heldObject.isDirectionHeldLocally = false;
            heldObject = null;
        }
        else
        {
            heldObject.isMainHeldLocally = false;
            heldObject = null;
        }
        isMainController = false;
        interactor.StopInfluencingObject();
    }
    void MoveObject()
    {
        if (otherController.heldObject == heldObject) // If item is held with two hands
        {
            if (isMainController) // If this is the main hand from two 
                switch (heldObject.holdingMethod)
                {
                    case HoldingData.HoldingMethod.MainHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.DirectionHandle:
                        TwoHandedPositioningStandard();
                        break;
                    case HoldingData.HoldingMethod.DirectionSlideHandle:
                        TwoHandedPositioningDirectionSliding();
                        break;
                    case HoldingData.HoldingMethod.SlideHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.RotationHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.GrabHandle:
                        break;
                    default:
                        break;
                }
        }
        else // If item is held with one hand
        {
            if (isMainController) // If this is main grip and the only hand
                switch (heldObject.holdingMethod)
                {
                    case HoldingData.HoldingMethod.MainHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.DirectionHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.DirectionSlideHandle:
                        OneHandedPositioning();
                        break;
                    case HoldingData.HoldingMethod.SlideHandle:
                        heldObject.transform.position = Vector3.Project(transform.position, heldObject.transform.parent.right);
                        Vector3 newPosition = heldObject.transform.localPosition;
                        newPosition.y = newPosition.z = 0;
                        newPosition.x = Mathf.Clamp(newPosition.x, 0, heldObject.slideMaxBack);
                        heldObject.transform.localPosition = newPosition;
                        heldObject.gun.PercentageSlide(newPosition.x / heldObject.slideMaxBack, false);
                        break;
                    case HoldingData.HoldingMethod.RotationHandle:
                        if (heldObject.transform.localPosition.x == 0)
                        {
                            heldObject.transform.localRotation = Quaternion.Euler(Mathf.Clamp(Vector3.SignedAngle(heldObject.transform.parent.up, Vector3.ProjectOnPlane(transform.position - heldObject.transform.parent.position, heldObject.transform.parent.right), heldObject.transform.parent.right), -heldObject.boltMaxUp, 0), heldObject.transform.localRotation.eulerAngles.y, heldObject.transform.localRotation.eulerAngles.z);

                            if (heldObject.transform.localRotation.eulerAngles.x == 0) // 360 formula problem, welp don't touch, is work.
                                heldObject.gun.PercentageBolt(0);
                            else
                                heldObject.gun.PercentageBolt((360 - heldObject.transform.localRotation.eulerAngles.x) / heldObject.boltMaxUp);
                        }
                        if (heldObject.transform.localRotation.eulerAngles.x == 360 - heldObject.boltMaxUp)
                        {
                            heldObject.transform.position = Vector3.Project(transform.position, heldObject.transform.parent.right);
                            newPosition = heldObject.transform.localPosition;
                            newPosition.y = newPosition.z = 0;
                            newPosition.x = Mathf.Clamp(newPosition.x, 0, heldObject.slideMaxBack);
                            heldObject.transform.localPosition = newPosition;
                            heldObject.gun.PercentageSlide(newPosition.x / heldObject.slideMaxBack, true);
                        }
                        break;
                    case HoldingData.HoldingMethod.GrabHandle:
                        break;
                    default:
                        break;
                }
            else // If one hand is holding an object and is not a main grip
                switch (heldObject.holdingMethod)
                {
                    case HoldingData.HoldingMethod.DirectionHandle:
                        break;
                    case HoldingData.HoldingMethod.DirectionSlideHandle:
                        break;
                    case HoldingData.HoldingMethod.GrabHandle:
                        break;
                    default:
                        break;
                }
        }
    }
    void Holster(GameObject inventorySlot)
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (currentCollision || heldObject)
            return;
        if (other.GetComponent<CollisionRelay>() && other.GetComponent<CollisionRelay>().rootObject.GetComponent<HoldingData>().CanBeGrabbed(!other.GetComponent<CollisionRelay>().isMainGrip))
        {
            currentCollision = other.GetComponent<CollisionRelay>().rootObject.GetComponent<HoldingData>();
            isCurrentCollisionDirection = !other.GetComponent<CollisionRelay>().isMainGrip;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CollisionRelay>() && currentCollision == other.GetComponent<CollisionRelay>().rootObject.GetComponent<HoldingData>())
            currentCollision = null;
    }



    void OneHandedPositioning()
    {
        heldObject.transform.position = transform.position;
        heldObject.transform.Translate(heldObject.positionOffset);
        heldObject.transform.rotation = transform.rotation;
        heldObject.transform.Rotate(heldObject.rotationOffset);
    }
    void TwoHandedPositioningStandard()
    {
        Vector3 normalizedDirection = (otherController.transform.position - transform.position).normalized;
        Vector3.Normalize(normalizedDirection);
        heldObject.transform.position = transform.position;
        heldObject.transform.rotation = Quaternion.LookRotation(normalizedDirection, transform.forward);
    }
    void OffsetHandlePositioning()
    {
        Debug.Log("OffsetHandleTODO");
    }
    void TwoHandedPositioningDirectionSliding()
    {

    }
}
