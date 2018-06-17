using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineInteractions : Interactions {
    public Magazine magazine;
    public Transform bulletDropPlace;


    public override void TouchpadUp()
    {
        Bullet bullet = magazine.NextBullet();
        if (bullet.bulletCaliber == WeaponDatabase.BulletCaliber.NoBullet)
            return;
        GameObject go = Instantiate(magazine.bulletPrefab, bulletDropPlace.position, bulletDropPlace.rotation);
        go.GetComponent<BulletInWorld>().caliber = bullet.bulletCaliber;
        go.GetComponent<BulletInWorld>().modification = bullet.bulletMod;
    }
}
