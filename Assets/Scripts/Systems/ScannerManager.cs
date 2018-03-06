using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScannerManager : MonoBehaviour {

	public Scanner[] scanners;
	public List<GameObject> trackedObjects = new List<GameObject>();
	private float inputScan = 0;
	private GameObject target;

	void Start () {
		scanners = transform.GetComponentsInChildren<Scanner>();

		for(int i = 0; i < scanners.Length; i++) {
			scanners[i].SetScannerManager(this);
		}
	}
	void Update() {
		trackedObjects.Clear();

		for(int i = 0; i < scanners.Length; i++) {
			for(int j = 0; j < scanners[i].GetTrackedObjects().Count; j++) {
				bool alreadyTracked = trackedObjects.Contains(scanners[i].GetTrackedObjects()[j]);

				if(!alreadyTracked && scanners[i].GetTrackedObjects()[j] != gameObject) {
					trackedObjects.Add(scanners[i].GetTrackedObjects()[j]);
				}
			}
		}
	}

	public void SetInputScan(float value) {
		inputScan = value;

		for(int i = 0; i < scanners.Length; i++) {
			if(!scanners[i].GetPassive()) {
				if(value == 0) {
					scanners[i].SetTrackedObjects(new List<GameObject>());
					scanners[i].enabled = false;
				}
				else {
					scanners[i].enabled = true;
				}
			}
		}
	}

	public void SetTarget(GameObject newTarget) {
		target = newTarget;
	}

	public List<GameObject> GetTrackedObjects() {
		return trackedObjects;
	}


}
