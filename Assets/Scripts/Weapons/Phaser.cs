using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]

public class Phaser : Weapon {

	private float maxRange = 50.0f;
	private float noise = 1.0f;
	private float beamWidth = 0.2f;
	private LineRenderer lineRenderer;
	private float previousTime;
	private float damageRate = 0.1f;

	public Transform emissionObject;
	ParticleSystem[] endEffects;

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
	}
	
	// Update is called once per frame
	void Update () {
		if(GetInputFire() != 0) {
			lineRenderer.enabled = true;
			firePhaser();
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

	void firePhaser() {
		int layerMask = 1 << 8;
		layerMask = ~layerMask;

		RaycastHit hit;
		Physics.Raycast(transform.position, transform.forward, out hit, maxRange, layerMask);

		lineRenderer.SetPosition(0, transform.position);

		if(hit.collider && hit.collider.attachedRigidbody.transform.GetComponent<DamageManager>()) {
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
			lineRenderer.SetPosition(1, transform.position + transform.forward * maxRange);

			foreach(ParticleSystem effect in endEffects) {
				if(effect.isPlaying) {
					effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
			}

		}
	}
}
