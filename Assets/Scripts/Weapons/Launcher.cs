using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Weapon {

    private float reloadTime = 5.0f;

    private float lastFireTime = 0.0f;

    private float speed = 15.0f;
    public Rigidbody payload;

	void Update () {
		if(GetInputFire() != 0) {
			Launch();
		}
	}

    /// <summary>
    /// Public GET method for reloadTime value (in seconds)
    /// </summary>
    /// <returns>Returns a float value in seconds</returns>
    public float ReloadTime() {
        return(reloadTime);
    }

    /// <summary>
    /// Public SET method for reloadTime value (in seconds)
    /// </summary>
    /// <param name="newReloadTime">Reload time (in seconds)</param>
    public void ReloadTime(float newReloadTime) {
        reloadTime = newReloadTime;
    }


    /// <summary>
    /// Public GET method for payload prefab Rigidbody
    /// </summary>
    /// <returns>Returns a Rigidbody representing the current payload</returns>
    public Rigidbody Payload() {
        return(payload);
    }
    
    /// <summary>
    /// Public SET method for payload prefab object
    /// </summary>
    /// <param name="newPayload">Rigidbody representing the new payload</param>
    public void Payload(Rigidbody newPayload) {
        payload = newPayload;
    }

    void Launch() {
        if(Time.time - lastFireTime >= reloadTime) {
            Rigidbody newPayload = (Rigidbody) GameObject.Instantiate(payload, transform.position, Quaternion.FromToRotation(Vector3.forward, transform.forward));
            Collider[] colliders = weaponsManager.gameObject.GetComponentsInChildren<Collider>();

			foreach (Collider collider in colliders) {
				Physics.IgnoreCollision(newPayload.transform.GetComponent<Collider>(), collider);
			}

			newPayload.velocity = speed*transform.forward;

            lastFireTime = Time.time;
        }
    }

}