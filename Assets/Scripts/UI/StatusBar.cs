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

    public float percentHealth = 1.0f;
    public float percentShield = 1.0f;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        UpdateDamageBar();
        UpdateShieldBar();
    }

    private void Initialize()
    {
        if (relatedTransform)
        {
            relatedDamageManager = relatedTransform.GetComponent<DamageManager>();
            relatedShieldManager = relatedTransform.GetComponent<ShieldManager>();

            UpdateDamageBar();
            UpdateShieldBar();
        }
    }

    private void UpdateDamageBar()
    {
        if (relatedDamageManager)
        {
            percentHealth = relatedDamageManager.health / relatedDamageManager.maxHealth;
            float parentWidth = health.sizeDelta.x;
            float parentHeight = health.sizeDelta.y;
            float valueWidth = parentWidth * percentHealth;
            healthValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
    }

    private void UpdateShieldBar()
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
