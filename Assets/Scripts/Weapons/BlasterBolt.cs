using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBolt : MonoBehaviour {

	private float lifetime = 2.0f;
	private float startTime;

	private float damage;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - startTime >= lifetime) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter (Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
		Shield hitShield = contact.otherCollider.transform.GetComponent<Shield>();
		DamageManager hitDamageManager = contact.otherCollider.attachedRigidbody.transform.GetComponent<DamageManager>();

		if(hitShield) {
			hitShield.DamageShield(damage);
		}
		else if(hitDamageManager) {
			hitDamageManager.Damage(damage);
		}

		Destroy(gameObject);
    }

	public void SetDamage(float newDamage) {
		damage = newDamage;
	}

	public float GetDamage() {
		return damage;
	}
}
