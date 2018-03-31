using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float inputFire = 0;
	public GameObject target = null;
	public WeaponsManager weaponsManager;
	public bool playerWeapon = false;

	public void SetWeaponsManager(WeaponsManager newWeaponsManager) {
		weaponsManager = newWeaponsManager;
	}

	public void SetInputFire(float value) {
		inputFire = value;
	}

	public float GetInputFire() {
		return(inputFire);
	}

	public void SetTarget(GameObject newTarget) {
		target = newTarget;
	}

	public GameObject GetTarget() {
		return(target);
	}

	public void SetTargetDestroyed() {
		if(weaponsManager) {
			weaponsManager.SetTargetDestroyed();
		}
	}

	public void SetPlayerWeapon(bool value) {
		playerWeapon = value;
	}

	public bool GetPlayerWeapon() {
		return playerWeapon;
	}
}
