using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public SpaceFlightController flightControl;
	public WeaponsManager weaponsManager;
	public ScannerManager scannerManager;

	// Use this for initialization
	void Start () {
		flightControl = transform.GetComponent<SpaceFlightController>();
		weaponsManager = transform.GetComponent<WeaponsManager>();
		scannerManager = transform.GetComponent<ScannerManager>();

		weaponsManager.SetPlayerWeapon(true);
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

		if(weaponsManager) {
			weaponsManager.SetInputFire(Input.GetAxis("Fire"));
		}

		if(scannerManager) {
			scannerManager.SetInputScan(Input.GetAxis("Scan"));
		}
	}
}
