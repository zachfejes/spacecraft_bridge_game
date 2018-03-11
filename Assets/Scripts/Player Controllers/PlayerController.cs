using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public SpaceFlightController flightControl;
	public WeaponsManager weaponsManager;
	public ScannerManager scannerManager;

	void OnDestroy() {
		transform.Find("Camera").transform.parent=null;
	}

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

		if(Input.GetKeyDown(KeyCode.T)) {
			if(scannerManager && scannerManager.GetTrackedObjects() && scannerManager.GetTrackedObjects().Count > 0) {
				GameObject nearestObject = null;

				for(int i = 0; i < scannerManager.GetTrackedObjects().Count; i++) {
					if(nearestObject == null) {
						nearestObject = scannerManager.GetTrackedObjects()[i];
					}
					else if (Vector3.Distance(scannerManager.GetTrackedObjects()[i].transform.position, transform.position) < Vector3.Distance(nearestObject.transform.position, transform.position)) {
						nearestObject = scannerManager.GetTrackedObjects()[i];
					}
				}
				
				weaponsManager.SetTarget(nearestObject);

			}
		}

	}
}
