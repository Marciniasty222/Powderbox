using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour {
    float tempTimer = 0;
    public float timerSpeed = 0;

    public Transform gunRotation;

    public float verticalKickStrength;
    public float horizontalKickStrength;
    public float knockbackStrength;
    public float stabilizationSpeed;
    public float currentRecoilVertical;
    public float currentRecoilHorizontal;
    public float currentKnockback;
    

	void Update () {
        tempTimer += Time.deltaTime;
        if (tempTimer > timerSpeed)
        {
            Shoot();
            tempTimer = 0;
        }
        Stabilization();
        gunRotation.localPosition = Vector3.zero;
        gunRotation.Translate(-currentKnockback, 0, 0);
        gunRotation.localRotation = Quaternion.identity;
        gunRotation.Rotate(0, currentRecoilHorizontal, currentRecoilVertical);
	}
    void Shoot()
    {
        currentRecoilVertical += verticalKickStrength;
        currentRecoilHorizontal += Random.Range(-horizontalKickStrength, horizontalKickStrength);
        currentKnockback += knockbackStrength;
    }
    void Stabilization()
    {
        currentRecoilVertical = Mathf.Lerp(currentRecoilVertical, 0, Time.deltaTime * stabilizationSpeed);
        currentRecoilHorizontal = Mathf.Lerp(currentRecoilHorizontal, 0, Time.deltaTime * stabilizationSpeed);
        currentKnockback = Mathf.Lerp(currentKnockback, 0, Time.deltaTime * stabilizationSpeed);
    }
}
