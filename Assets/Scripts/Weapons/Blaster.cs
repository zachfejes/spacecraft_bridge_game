using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Weapon {

	private float speed = 30.0f;
	private float previousTime;

	private float damage = 5.0f;
	
	private float frequency = 0.2f;

	public Rigidbody blasterBolt;
	ParticleSystem[] hitEffect;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(GetInputFire() != 0) {
			fireBlaster();
		}
	}

	void fireBlaster() {
		if(Time.time - previousTime >= frequency) {
			Rigidbody newBolt = (Rigidbody) GameObject.Instantiate(blasterBolt, transform.position, Quaternion.FromToRotation(Vector3.forward, transform.forward));
			Collider[] colliders = weaponsManager.gameObject.GetComponentsInChildren<Collider>();

			foreach (Collider collider in colliders) {
				Physics.IgnoreCollision(newBolt.transform.GetComponent<Collider>(), collider);
			}
			
			newBolt.transform.GetComponent<BlasterBolt>().SetDamage(damage);

			newBolt.velocity = speed*transform.forward;

			previousTime = Time.time;
		}
	}
}
