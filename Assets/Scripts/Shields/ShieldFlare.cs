using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFlare : MonoBehaviour {
    private float lifetime = 2.0f;
    private float startTime;

    void Start() {
        startTime = Time.time;
    }

    void Update() {
        if(Time.time - startTime >= lifetime) {
            Destroy(gameObject);
        }
    }
}