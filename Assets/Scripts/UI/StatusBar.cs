using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: Make this the standard Status Bar parent class
public class StatusBar : MonoBehaviour
{
    public Transform relatedTransform;
    public DamageManager relatedDamageManager;
    public ShieldManager relatedShieldManager;

    public RectTransform health;
    public RectTransform healthValue;

    public RectTransform shield;
    public RectTransform shieldValue;

    public float percentHealth;
    public float percentShield;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        UpdateDamageBar();
        UpdateShieldBar();
    }

    public void Initialize()
    {
        if (relatedTransform)
        {
            relatedDamageManager = relatedTransform.GetComponent<DamageManager>();
            relatedShieldManager = relatedTransform.GetComponent<ShieldManager>();

            UpdateDamageBar();
            UpdateShieldBar();
        }
    }

    public void UpdateDamageBar()
    {
        if (relatedDamageManager)
        {   
            Debug.Log("relatedDamageManager.health: " + relatedDamageManager.health + ", relatedDamageManager.maxHealth: " + relatedDamageManager.maxHealth);
            percentHealth = relatedDamageManager.health / relatedDamageManager.maxHealth;
            float parentWidth = health.sizeDelta.x;
            float parentHeight = health.sizeDelta.y;
            float valueWidth = parentWidth * percentHealth;
            healthValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
    }

    public void UpdateShieldBar()
    {
        if (relatedShieldManager && relatedShieldManager.shieldStrength.Length > 0)
        {
            percentShield = relatedShieldManager.shieldStrength[0] / 100;
            float parentWidth = shield.sizeDelta.x;
            float parentHeight = shield.sizeDelta.y;
            float valueWidth = parentWidth * percentShield;
            shieldValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        relatedTransform = newTarget.transform;
        relatedDamageManager = newTarget.transform.GetComponent<DamageManager>();
        relatedShieldManager = newTarget.transform.GetComponent<ShieldManager>();
    }

}
