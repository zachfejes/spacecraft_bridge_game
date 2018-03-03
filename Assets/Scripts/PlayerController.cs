using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public SpaceFlightController flightControl;
	public WeaponsManager weaponsControl;

	// Use this for initialization
	void Start () {
		flightControl = transform.GetComponent<SpaceFlightController>();
		weaponsControl = transform.GetComponent<WeaponsManager>();

		weaponsControl.SetPlayerWeapon(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(flightControl) {
			flightControl.SetHorizontal(Input.GetAxis("Horizontal"));
			flightControl.SetVertical(Input.GetAxis("Vertical"));
			flightControl.SetForeback(Input.GetAxis("Foreback"));
			flightControl.SetRoll(Input.GetAxis("Roll"));
			flightControl.SetPitch(Input.GetAxis("Pitch"));
			flightControl.SetYaw(Input.GetAxis("Yaw"));
		}

		if(weaponsControl) {
			weaponsControl.SetInputFire(Input.GetAxis("Fire"));
		}
	}
}
