using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTorpedo : MonoBehaviour
{

    private float fuseTime = 3.0f;
    private float fuseStartTime;
    private float explosionLifetime = 5.0f;
    private float explosionStartTime;
    private bool detonated = false;
    private float yield = 120.0f;
    private float blastRadius = 10.0f;

    public GameObject explosion;

    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        explosion = transform.Find("Explosion").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        fuseStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (detonated)
        {
            if (Time.time - explosionStartTime >= explosionLifetime)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Time.time - fuseStartTime >= fuseTime)
            {
                Detonate();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!detonated)
        {
            Detonate();
        }
    }

    private void Yield(float newYield)
    {
        yield = newYield;
    }

    private float Yield()
    {
        return (yield);
    }

    public void Detonate()
    {
        detonated = true;
        explosionStartTime = Time.time;
        rb.velocity = new Vector3(0, 0, 0);

        if (explosion)
        {
            explosion.SetActive(true);
        }

        transform.GetComponent<MeshRenderer>().enabled = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);

        List<GameObject> scannedObjects = new List<GameObject>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject tempObject = hitColliders[i].attachedRigidbody.gameObject;
            bool alreadyTracked = scannedObjects.Contains(tempObject);

            if (!alreadyTracked && tempObject != gameObject)
            {
                scannedObjects.Add(tempObject);
            }
        }

        for (int i = 0; i < scannedObjects.Count; i++)
        {
            GameObject tempObject = scannedObjects[i];
            tempObject.GetComponent<Rigidbody>().AddExplosionForce(500.0f, transform.position, blastRadius);

            Shield hitShield = tempObject.transform.GetComponentInChildren<Shield>();
            DamageManager hitDamageManager = tempObject.transform.GetComponent<DamageManager>();

            if (hitShield)
            {
                float shieldBeforeDamage = hitShield.GetShieldHP();
                float shieldAfterDamage = hitShield.DamageShield(yield);

                if (shieldAfterDamage == 0 && hitDamageManager)
                {
                    hitDamageManager.Damage(yield - (shieldBeforeDamage - shieldAfterDamage));
                }
            }
            else if (hitDamageManager)
            {
                hitDamageManager.Damage(yield);
            }
        }
    }

}
