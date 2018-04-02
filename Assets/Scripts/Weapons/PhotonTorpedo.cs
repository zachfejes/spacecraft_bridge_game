using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTorpedo : MonoBehaviour {

	private float lifetime = 3.0f;
	private float startTime;
    private float yield = 1.0f;

    public GameObject explosion;


	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - startTime >= lifetime) {
            Detonate();
		}
	}

	void OnCollisionEnter (Collision collision)
    {
        Detonate();
    }

    private void Yield(float newYield) {
        yield = newYield;
    }

    private float Yield() {
        return(yield);
    }

    public void Detonate() {
        GameObject.Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
