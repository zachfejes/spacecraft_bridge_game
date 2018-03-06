using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scanner : MonoBehaviour {

	private ScannerManager scannerManager;
	public float range;
	private float frequency = 1.0f;
	private List<GameObject> trackedObjects = new List<GameObject>();

	public float GetRange() {
		return range;
	}

	public void SetScannerManager(ScannerManager newScannerManager) {
		scannerManager = newScannerManager;
	}

	public void SetRange(float newRange) {
		range = newRange;
	}

	public float GetFrequency() {
		return frequency;
	}

	public void SetFrequency(float newFrequency) {
		frequency = newFrequency;
	}

	public List<GameObject> GetTrackedObjects() {
		return trackedObjects;
	}

	public void SetTrackedObjects(List<GameObject> newTrackedObjects) {
		trackedObjects = newTrackedObjects;
	}

}
