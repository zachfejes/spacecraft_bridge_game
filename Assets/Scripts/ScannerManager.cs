using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScannerManager : MonoBehaviour {

	public float normalScanningRange;
	public float longScanningRange;
	private float scanFrequency = 1.0f;
	private float lastScan;

	public List<GameObject> trackedObjects = new List<GameObject>();

	void Start() {
		lastScan = Time.time;
	}

	void Update() {
		PassiveScan();
	}

	void PassiveScan() {
		if(Time.time - lastScan > scanFrequency) {
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, normalScanningRange);

			trackedObjects.Clear();
			
			for(int i = 0; i < hitColliders.Length; i++) {
				GameObject tempObject = hitColliders[i].attachedRigidbody.gameObject;
				bool alreadyTracked = trackedObjects.Contains(tempObject);

				if(!alreadyTracked && tempObject != gameObject) {
					trackedObjects.Add(tempObject);
				}
			}

			lastScan = Time.time;
		}
	}

	public List<GameObject> GetTrackedObjects() {
		return trackedObjects;
	}

	void OnDrawGizmosSelected() {
			UnityEditor.Handles.color = Color.blue;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, normalScanningRange);

			UnityEditor.Handles.color = Color.blue;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, longScanningRange);

			for(int i = 0; i < trackedObjects.Count; i++) {
				UnityEditor.Handles.DrawWireDisc(trackedObjects[i].transform.position, Vector3.up, 5.0f);
			}
    }

}
