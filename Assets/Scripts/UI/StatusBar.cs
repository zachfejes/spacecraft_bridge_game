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
            health.gameObject.SetActive(true);
            percentHealth = relatedDamageManager.health / relatedDamageManager.maxHealth;
            float parentWidth = health.sizeDelta.x;
            float parentHeight = health.sizeDelta.y;
            float valueWidth = parentWidth * percentHealth;
            healthValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
        else {
            health.gameObject.SetActive(false);
        }
    }

    public void UpdateShieldBar()
    {
        if (relatedShieldManager && relatedShieldManager.shieldStrength.Length > 0)
        {
            shield.gameObject.SetActive(true);
            percentShield = relatedShieldManager.shieldStrength[0] / relatedShieldManager.maxShieldStrength[0];
            float parentWidth = shield.sizeDelta.x;
            float parentHeight = shield.sizeDelta.y;
            float valueWidth = parentWidth * percentShield;
            shieldValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
        else {
            shield.gameObject.SetActive(false);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        if (newTarget)
        {
            relatedTransform = newTarget.transform;
            relatedDamageManager = newTarget.transform.GetComponent<DamageManager>();
            relatedShieldManager = newTarget.transform.GetComponent<ShieldManager>();
        }
    }

}
