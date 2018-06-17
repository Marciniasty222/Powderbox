using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInWorld : MonoBehaviour
{
    public WeaponDatabase.BulletCaliber caliber;
    public WeaponDatabase.BulletMod modification;
    
    public void OnTriggerEnter(Collider other)
    {
        Magazine magazine = other.GetComponent<Magazine>();
        if (magazine && magazine.caliber == caliber && magazine.AddBullet(new Bullet(caliber, modification)))
            Destroy(gameObject);
    }
}
