using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInteractor : MonoBehaviour {
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

    public Interactions influencedObject;

    void Update()
    {
        if (!influencedObject) // If no object being interacted with, don't use methods
            return;


        if (Controller.GetHairTriggerDown())
        {
            influencedObject.TriggerDown();
        }
        if(Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            influencedObject.MenuButton();
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 axis = Controller.GetAxis();
            if (axis.x >= axis.y)
            {
                if (axis.x > -1 * axis.y)
                {
                    influencedObject.TouchpadRight();
                }
                else if (axis.x < -1 * axis.y)
                {
                    influencedObject.TouchpadDown();
                }
            }
            else if (axis.x <= axis.y)
            {
                if (axis.x > -1 * axis.y)
                {
                    influencedObject.TouchpadUp();
                }
                else if (axis.x < -1 * axis.y)
                {
                    influencedObject.TouchpadLeft();
                }
            }
        }
    }
    public void StartInfluencingObject(Interactions newObject)
    {
        influencedObject = newObject;
    }
    public void StopInfluencingObject()
    {
        influencedObject = null;
    }
}
