using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour {
    public int ammoCapacity;
    public GameObject bulletPrefab;
    public WeaponDatabase.BulletCaliber caliber;
    public Stack<Bullet> bullets;
    public MeshRenderer[] visualBullets;

    void Start()
    {
        bullets = new Stack<Bullet>();
        RefreshVisualBullets(bullets.Count);
    }
    public Bullet NextBullet(bool removeFromMagazine = true)
    {
        if (bullets.Count == 0)
            return new Bullet();
        if (removeFromMagazine)
        {
            RefreshVisualBullets(bullets.Count - 1);
            return bullets.Pop();
        }
        else
            return bullets.Peek();
    }
    public bool AddBullet(Bullet bullet)
    {
        if (ammoCapacity <= bullets.Count)
            return false;
        bullets.Push(bullet);
        RefreshVisualBullets(bullets.Count);
        return true;
    }
    void RefreshVisualBullets(int bulletNumber)
    {
        if (bulletNumber == 0)
        {
            visualBullets[0].enabled = false;
            visualBullets[1].enabled = false;
        }
        else if (bulletNumber == 1)
        {
            visualBullets[0].enabled = true;
            visualBullets[1].enabled = false;
        }
        else if(bulletNumber > 1)
        {
            visualBullets[0].enabled = true;
            visualBullets[1].enabled = true;
        }
    }
}
