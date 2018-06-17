using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScientist : MonoBehaviour, IKillable, IDamageable<float> {

    public float HP = 100.0f;

	void Start () {
		
	}
	
	
	void Update () {
        
	}

    public void Kill()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().mass = 20;
        Destroy(GetComponent<AIthroughWalls>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<GhostScientist>());
    }

    public void Damage(float damageTaken)
    {
        HP -= damageTaken;
        if (HP <= 0.0f) Kill();
    }
}
