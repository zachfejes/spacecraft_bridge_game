using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    private ShieldManager shieldManager;
    private Collider shieldCollider;
    private MeshRenderer shieldRenderer;
    public float maxShieldHP = 100.0f;
    private float shieldHP;
    private bool shieldActive = true;

    void Start() {
        shieldHP = maxShieldHP;
        shieldCollider = transform.GetComponent<Collider>();
        shieldRenderer = transform.GetComponent<MeshRenderer>();
    }

    public void SetShieldManager(ShieldManager newShieldManager) {
        shieldManager = newShieldManager;
    }

    public float GetShieldHP() {
        return(shieldHP);
    }

    public void DamageShield(float damage) {
        if(shieldHP - damage <= 0) {
            shieldHP = 0;
            DeactivateShield();
        }
        else {
            shieldHP -= damage;
        }
    }

    public void HealShield(float healing) {
        if(shieldHP + healing >= maxShieldHP) {
            shieldHP = maxShieldHP;
        }
        else {
            shieldHP += healing;
        }
    }

    public void ActivateShield() {
        shieldActive = true;
        shieldCollider.enabled = true;
        shieldRenderer.enabled = true;
    }

    public void DeactivateShield() {
        shieldActive = false;
        shieldCollider.enabled = false;
        shieldRenderer.enabled = false;
    }
}