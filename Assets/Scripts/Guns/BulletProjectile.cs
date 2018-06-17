using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    public Bullet bulletInfo;

    float bulletClearTime = 4f;
	
	void Start () {
        Destroy(gameObject, bulletClearTime);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Damagable")
        {
            if (collision.gameObject.GetComponent<IDamageable<float>>() != null)
            {
                collision.gameObject.GetComponent<IDamageable<float>>().Damage(10); //DAMAGE
            }
            
        }
    }
}
