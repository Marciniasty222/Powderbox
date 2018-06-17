using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSlot : MonoBehaviour
{
    public Magazine magazineAttached;
    
    public void OnTriggerEnter(Collider other)
    {
        Magazine magazine = other.GetComponent<Magazine>();
        if (magazine)
        {
            magazineAttached = magazine;

            HoldingData holdingData = magazineAttached.GetComponent<HoldingData>();

            magazineAttached.transform.parent = transform;
            magazineAttached.transform.localPosition = Vector3.zero;
            magazineAttached.transform.localRotation = Quaternion.identity;

            holdingData.isStatic = true;
            holdingData.gun = null;

            magazineAttached.GetComponent<Rigidbody>().isKinematic = true;

        }
    }

    public void DetachMagazine()
    {
        HoldingData holdingData = magazineAttached.GetComponent<HoldingData>();
        Rigidbody rb = magazineAttached.GetComponent<Rigidbody>();

        magazineAttached.transform.parent = null;
        holdingData.isStatic = false;
        holdingData.gun = holdingData.GetComponentInParent<Gun>();
        rb.isKinematic = false;

        magazineAttached = null;
    }
}
