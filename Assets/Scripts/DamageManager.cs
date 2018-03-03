using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float health = 100.0f;

	public bool damage(float value) {
		health = health - value;

		if(health <= 0) {
			health = 0;
			death();
			return(true);
		}
		else {
			return(false);
		}
	}

	public void heal(float value) {
		health = health + value;

		if(health > maxHealth) {
			health = maxHealth;
		}
	}

	void death() {
		Debug.Log("A thingy got destroyed!");
		Destroy(gameObject);
	}
}
