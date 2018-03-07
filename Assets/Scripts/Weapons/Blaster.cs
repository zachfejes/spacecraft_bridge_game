using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Weapon {

	private float speed = 1.0f;
	private float previousTime;

	private float damage = 5.0f;
	
	private float frequency = 1.0f;

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
			Rigidbody newBolt = (Rigidbody) GameObject.Instantiate(blasterBolt);
			newBolt.velocity = speed*transform.forward;

			previousTime = Time.time;
		}
	}
}
