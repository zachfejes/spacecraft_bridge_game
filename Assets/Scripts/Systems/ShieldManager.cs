using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour {

	public Shield[] shields;
	public float[] shieldStrength;
	public Transform shieldFlarePrefab;


	void Start() {
		shields = transform.GetComponentsInChildren<Shield>();
		shieldStrength = new float[shields.Length];

		for(int i = 0; i < shields.Length; i++) {
			shields[i].SetShieldManager(this);
			shieldStrength[i] = shields[i].GetShieldHP();
		}
	}

	void Update() {
		shields = transform.GetComponentsInChildren<Shield>();
		shieldStrength = new float[shields.Length];

		for(int i = 0; i < shields.Length; i++) {
			shields[i].SetShieldManager(this);
			shieldStrength[i] = shields[i].GetShieldHP();
		}
	}

	void OnCollisionEnter (Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
		Shield hitShield = contact.thisCollider.transform.GetComponent<Shield>();

		if(hitShield) {
			float collisionForce = Vector3.Magnitude(collision.impulse/Time.fixedDeltaTime);
			hitShield.DamageShield(collisionForce/1000000);

			Quaternion rot = Quaternion.FromToRotation(transform.forward, contact.normal);
			Vector3 pos = contact.point;
			Instantiate(shieldFlarePrefab, pos, rot);
		}
    }
}
