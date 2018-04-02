using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public float lifetime = 5.0f;
	public float startTime;

	public float damage = 50.0f;

	public float blastRadius = 10.0f;

	// Use this for initialization
	void Start () {
		startTime = Time.time;

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);

		for(int i = 0; i < hitColliders.Length; i++) {
			GameObject tempObject = hitColliders[i].attachedRigidbody.gameObject;
			
			Shield hitShield = tempObject.transform.GetComponent<Shield>();
			DamageManager hitDamageManager = tempObject.transform.GetComponent<DamageManager>();

			if(hitShield) {
				hitShield.DamageShield(damage);
			}
			else if(hitDamageManager) {
				hitDamageManager.Damage(damage);
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - startTime >= lifetime) {
			Destroy(gameObject);
		}
	}

	public void Damage(float newDamage) {
		damage = newDamage;
	}

	public float Damage() {
		return(damage);
	}
}
