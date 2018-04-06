using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityTorpedo : Torpedo
{
    private float effectLifetime = 10.0f;
    private float effectStartTime;
    private float influenceRadius = 100.0f;

    private float eventHorizonRadius = 5.0f;

    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        FuseStartTime(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (EffectActive())
        {
            if (Time.time - effectStartTime >= effectLifetime)
            {
                Destroy(gameObject);
            }
            else {
                Singularity();
            }
        }
        else
        {
            if (Time.time - FuseStartTime() >= FuseTime())
            {
                Activate();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!EffectActive())
        {
            Activate();
        }
    }


    new public void Activate()
    {
        EffectActive(true);
        effectStartTime = Time.time;
        rb.velocity = new Vector3(0, 0, 0);

        transform.GetComponent<MeshRenderer>().enabled = false;
    }

    void Singularity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, influenceRadius);

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
            tempObject.GetComponent<Rigidbody>().AddExplosionForce(-10000.0f, transform.position, influenceRadius);

            // Shield hitShield = tempObject.transform.GetComponentInChildren<Shield>();
            // DamageManager hitDamageManager = tempObject.transform.GetComponent<DamageManager>();

            // if (hitShield)
            // {
            //     float shieldBeforeDamage = hitShield.GetShieldHP();
            //     float shieldAfterDamage = hitShield.DamageShield(yield);

            //     if (shieldAfterDamage == 0 && hitDamageManager)
            //     {
            //         hitDamageManager.Damage(yield - (shieldBeforeDamage - shieldAfterDamage));
            //     }
            // }
            // else if (hitDamageManager)
            // {
            //     hitDamageManager.Damage(yield);
            // }
        }
    }

}
