using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterTracking : Weapon {

	private float speed = 30.0f;
	private float previousTime;

	private float damage = 5.0f;
	
	private float frequency = 0.2f;

	public Rigidbody blasterBolt;
	ParticleSystem[] hitEffect;

	private Vector3 targetDirection;
	public float firingArcAngle;

	// Use this for initialization
	void Start () {
		previousTime = Time.time;

		if(GetTarget()) {
			updateTargetTracking();
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateTargetTracking();

		if(GetInputFire() != 0 && 
			GetTarget() && 
			Vector3.Angle(transform.forward, targetDirection) < firingArcAngle
		) {
			fireBlaster(targetDirection);
		}
	}

	void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward*speed);

			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(firingArcAngle, transform.up) * transform.forward*speed);
			Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-firingArcAngle, transform.up) * transform.forward*speed);
			Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(90, transform.forward)*Quaternion.AngleAxis(firingArcAngle, transform.up) * transform.forward*speed);
			Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(90, transform.forward)*Quaternion.AngleAxis(-firingArcAngle, transform.up) * transform.forward*speed);
    }

	void updateTargetTracking() {
		if(GetTarget()) {
			targetDirection = Vector3.Normalize(GetTarget().transform.position - transform.position);
		}
	}

	void fireBlaster(Vector3 targetDirection) {

		if(targetDirection == null) {
			return;
		}

        if(Time.time - previousTime >= frequency) {
			Rigidbody newBolt = (Rigidbody) GameObject.Instantiate(blasterBolt, transform.position, Quaternion.FromToRotation(Vector3.forward, targetDirection));
			Collider[] colliders = weaponsManager.gameObject.GetComponentsInChildren<Collider>();

			foreach (Collider collider in colliders) {
				Physics.IgnoreCollision(newBolt.transform.GetComponent<Collider>(), collider);
			}
			
			newBolt.transform.GetComponent<BlasterBolt>().SetDamage(damage);

			newBolt.velocity = speed*targetDirection;

			previousTime = Time.time;
		}
	}

}
