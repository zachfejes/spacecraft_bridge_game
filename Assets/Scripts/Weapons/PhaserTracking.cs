using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]

public class PhaserTracking : Weapon {

	private float maxRange = 50.0f;
	private float noise = 1.0f;
	private float beamWidth = 0.2f;
	private LineRenderer lineRenderer;
	private float previousTime;
	private float damageRate = 0.1f;

	public Transform emissionObject;
	ParticleSystem[] endEffects;

	public Vector3 targetDirection;
	public float firingArcAngle;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(beamWidth, beamWidth);

		endEffects = emissionObject.GetComponentsInChildren<ParticleSystem>();

		foreach(ParticleSystem effect in endEffects) {
			if(effect.isPlaying) {
				effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}

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
			Vector3.Distance(transform.position, GetTarget().transform.position) < maxRange && 
			Vector3.Angle(transform.forward, targetDirection) < firingArcAngle
		) {
			lineRenderer.enabled = true;
			firePhaser(targetDirection);
		}
		else {
			lineRenderer.enabled = false;

			foreach(ParticleSystem effect in endEffects) {
				if(effect.isPlaying) {
					effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
			}
		}
	}

	void OnDrawGizmosSelected() {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(transform.position, transform.position + transform.forward*maxRange);

			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, targetDirection + transform.position);

			// Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(firingArcAngle, transform.up) * transform.forward*maxRange);
			// Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-firingArcAngle, transform.up) * transform.forward*maxRange);
			// Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(90, transform.forward)*Quaternion.AngleAxis(firingArcAngle, transform.up) * transform.forward*maxRange);
			// Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(90, transform.forward)*Quaternion.AngleAxis(-firingArcAngle, transform.up) * transform.forward*maxRange);
    }

	void updateTargetTracking() {
		if(GetTarget() != null) {
			targetDirection = Vector3.Normalize(GetTarget().transform.position - transform.position);
		}
	}

	void firePhaser(Vector3 targetDirection) {

		if(targetDirection == null) {
			return;
		}

		RaycastHit hit;

		if(GetPlayerWeapon()) {
			int layerMask = 1 << 8;
			layerMask = ~layerMask;

			Physics.Raycast(transform.position, targetDirection, out hit, maxRange, layerMask);
		}
		else {
			Physics.Raycast(transform.position, targetDirection, out hit, maxRange);
		}

		if(hit.collider) {
			lineRenderer.SetPosition(0, transform.position);

			Shield hitShield = hit.collider.transform.GetComponent<Shield>();
			DamageManager hitDamageManager = hit.collider.attachedRigidbody.transform.GetComponent<DamageManager>();

			if(hitShield) {
				if(Time.time - previousTime >= damageRate) {
					hitShield.DamageShield(1.0f);
					previousTime = Time.time;
				}

				lineRenderer.SetPosition(1, hit.point);
				emissionObject.position = hit.point;

				foreach(ParticleSystem effect in endEffects) {
					if(effect.isStopped) {
						effect.Play(true);
					}
				}
			}
			else if(hit.collider && hitDamageManager) {

				if(Time.time - previousTime >= damageRate) {
					if(hit.collider.attachedRigidbody.transform.GetComponent<DamageManager>().Damage(1.0f)) {
						SetTargetDestroyed();
					}
					previousTime = Time.time;
				}

				lineRenderer.SetPosition(1, hit.point);
				emissionObject.position = hit.point;

				foreach(ParticleSystem effect in endEffects) {
					if(effect.isStopped) {
						effect.Play(true);
					}
				}

			}
			else {
				//Target Out Of Range
			}
		}
		else {
			//Target Out Of Range
		}

	}

}
