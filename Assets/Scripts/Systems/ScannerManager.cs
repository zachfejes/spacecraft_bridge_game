using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScannerManager : MonoBehaviour {

	public Scanner[] scanners;
	public List<GameObject> trackedObjects = new List<GameObject>();

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

	public List<GameObject> GetTrackedObjects() {
		return trackedObjects;
	}


}
