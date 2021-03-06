﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour {

	public Weapon[] weapons;
	public float inputFire = 0;
	public GameObject target;
	public bool playerWeapon = false;

    void OnDrawGizmosSelected() {
		if(target != null) {
            UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(target.transform.position, Vector3.up, 6.0f);
			UnityEditor.Handles.DrawWireDisc(target.transform.position, Vector3.right, 6.0f);
			UnityEditor.Handles.DrawWireDisc(target.transform.position, Vector3.forward, 6.0f);
		}
    }

	// Use this for initialization
	void Start () {
		weapons = transform.GetComponentsInChildren<Weapon>();

		for(int i = 0; i < weapons.Length; i++) {
			weapons[i].SetWeaponsManager(this);

			if(target) {
				weapons[i].SetTarget(target);
			}

			if(playerWeapon) {
				weapons[i].SetPlayerWeapon(true);
			}
		}
	}
	
	public void SetInputFire(float value) {
		inputFire = value;

		for(int i = 0; i < weapons.Length; i++) {
			weapons[i].SetInputFire(value);
		}
	}

	public void SetTarget(GameObject newTarget) {
		target = newTarget;

		for(int i = 0; i < weapons.Length; i++) {
			weapons[i].SetTarget(newTarget);
		}
	}

	public void SetTargetDestroyed() {
		SetTarget(null);
	}

	public void SetPlayerWeapon(bool value) {
		playerWeapon = value;

		for(int i = 0; i < weapons.Length; i++) {
			weapons[i].SetPlayerWeapon(value);
		}
	}
}
