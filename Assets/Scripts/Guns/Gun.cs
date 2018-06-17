using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform barrel;
    public GameObject bulletPrefab;
    public MagazineSlot magazineSlot;
    public Bullet bulletInChamber;
    public Transform bulletChamberExit;

    public bool safety;
    public bool endOfMagazine;

    public float fireRate;
    public float baseDamage;
    public float bulletSpeed;

    public bool cocked = false; // Hammer ready to smash at fowardSlidePercent = 1
    public bool bulletChambered; // Bullet taken out of magazine at slideBulletTakePercent
    public bool slidePulled; // Is it currently on pulled position?
    public bool readyToShootPosition; // Is the position 0?

    public float slideBulletTakePercent = 0.8f; // Range(0,1)

    public void TryShoot()
    {
        if (CanShoot())
        {
            if(bulletChambered)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.Euler(barrel.transform.forward));
                bullet.GetComponent<Rigidbody>().AddForce(barrel.transform.right * bulletSpeed);
                bullet.GetComponent<BulletProjectile>().bulletInfo = bulletInChamber;

                bulletInChamber = new Bullet();
                bulletChambered = false;
                cocked = false;
                //AUDIO BOOM
            }
            else
            {
                bulletChambered = false;
                cocked = false;
                //AUDIO CLICK
            }
        }
    }
    public void EjectBullet() // bulletOnSpring and slideBulletTakePercent < fowardSlidePercent
    {
        //GameObject bullet = Instantiate(magazineAttached.NextBullet(), barrel.transform.position, Quaternion.Euler(barrel.transform.forward));
        //bullet.GetComponent<Rigidbody>().AddForce(barrel.transform.right * bulletSpeed);
    }
    public bool CanShoot()
    {
        if (cocked && readyToShootPosition)
            return true;
        else
            return false;
    }

    public void PercentageSlide(float percent, bool hasBolt)
    {
        if (!hasBolt) // If standard slide
        {
            if (slideBulletTakePercent < percent && !slidePulled)
            {
                slidePulled = true;
                LoadBulletIntoChamber();
            }
            if (slideBulletTakePercent >= percent)
                slidePulled = false;
            if (percent == 0)
                readyToShootPosition = true;
            else
                readyToShootPosition = false;
            if (percent == 1)
                cocked = true;
        }
        else // If bolt action
        {
            if (slideBulletTakePercent < percent && !slidePulled)
            {
                slidePulled = true;
                LoadBulletIntoChamber();
            }
            if (slideBulletTakePercent >= percent)
                slidePulled = false;
            if (percent == 1)
                cocked = true;
        }
    }
    public void PercentageBolt(float percent)
    {
        if (percent == 0)
            readyToShootPosition = true;
        else
            readyToShootPosition = false;
    }
    public void LoadBulletIntoChamber()
    {
        if (bulletChambered) //BUG - cloning ammo
        {
            Instantiate(magazineSlot.magazineAttached.bulletPrefab, bulletChamberExit.position, bulletChamberExit.rotation);
            bulletChambered = false;
            bulletInChamber = new Bullet();
        }
        if(magazineSlot.magazineAttached)
        {
            Bullet bullet = magazineSlot.magazineAttached.NextBullet();
            if(bullet.bulletCaliber != WeaponDatabase.BulletCaliber.NoBullet)
            {
                bulletChambered = true;
                bulletInChamber = bullet;
            }
        }
    }
}
