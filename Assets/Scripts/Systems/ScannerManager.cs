using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScannerManager : MonoBehaviour {

    public GameObject targetInfoPrefab;
	public Scanner[] scanners;
	public List<GameObject> trackedObjects = new List<GameObject>();
	private float inputScan = 0;
	private GameObject target;
	private RectTransform targetInfo;

	void Start () {
		if(targetInfoPrefab) {
			GameObject canvas = GameObject.Find("Canvas");
			if(canvas) {
				GameObject newTargetInfo = GameObject.Instantiate(targetInfoPrefab);
				newTargetInfo.transform.SetParent(canvas.transform);
				newTargetInfo.GetComponent<StatusBars>().canvasRT = canvas.GetComponent<RectTransform>();
				targetInfo = newTargetInfo.GetComponent<RectTransform>();
				targetInfo.sizeDelta = new Vector2(0,0);
				
			}
		}

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

		if(targetInfo) {
			StatusBars targetStatus = targetInfo.transform.GetComponent<StatusBars>();
			targetStatus.SetTarget(newTarget);
		}
	}

	public List<GameObject> GetTrackedObjects() {
		return trackedObjects;
	}


}
