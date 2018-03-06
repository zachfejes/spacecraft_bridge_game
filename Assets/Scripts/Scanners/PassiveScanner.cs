using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PassiveScanner : Scanner {
    private float lastScan;

    void Start() {
		lastScan = Time.time;
	}

	void Update() {
		PassiveScan();
	}

    void PassiveScan() {
		if(Time.time - lastScan > GetFrequency()) {
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetRange());

            List<GameObject> scannedObjects = new List<GameObject>();
			
			for(int i = 0; i < hitColliders.Length; i++) {
				GameObject tempObject = hitColliders[i].attachedRigidbody.gameObject;
				bool alreadyTracked = scannedObjects.Contains(tempObject);

				if(!alreadyTracked && tempObject != gameObject) {
					scannedObjects.Add(tempObject);
				}
			}

            SetTrackedObjects(scannedObjects);

			lastScan = Time.time;
		}
	}

    void OnDrawGizmosSelected() {
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, range);

        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, range);

        for(int i = 0; i < GetTrackedObjects().Count; i++) {
            UnityEditor.Handles.DrawWireDisc(GetTrackedObjects()[i].transform.position, Vector3.up, 5.0f);
        }
    }
}