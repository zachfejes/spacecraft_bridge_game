using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBars : MonoBehaviour
{

    private RectTransform rt;
    public RectTransform canvasRT;
    public Transform relatedTransform;
    public DamageManager relatedDamageManager;
    public ShieldManager relatedShieldManager;
    public float offsetY = 0.0f;
    public float offsetX = 0.0f;

    public RectTransform health;
    public RectTransform healthValue;

    public float percentHealth = 1.0f;

    void Awake()
    {
        rt = GetComponent<RectTransform>();

        if (relatedTransform)
        {
            Vector3 worldPosition = relatedTransform.position;
            Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
            Vector2 screenPosition = new Vector2
            (
                viewport.x * canvasRT.sizeDelta.x,
                viewport.y * canvasRT.sizeDelta.y
            );

            // this is reversed because of the anchor we are using for this element
            screenPosition.y = -screenPosition.y;

            rt.anchoredPosition = screenPosition;


            relatedDamageManager = relatedTransform.GetComponent<DamageManager>();

            if (relatedDamageManager)
            {
                percentHealth = relatedDamageManager.health / relatedDamageManager.maxHealth;
                float parentWidth = health.sizeDelta.x;
                float parentHeight = health.sizeDelta.y;
                float valueWidth = parentWidth * percentHealth;
                healthValue.sizeDelta = new Vector2(valueWidth, parentHeight);
            }
        }
    }

    void Update()
    {
        if (relatedTransform)
        {
            Vector3 behindCamera = Camera.main.WorldToViewportPoint(relatedTransform.position);

            if (behindCamera.z < 0)
            {
                health.transform.GetComponent<Image>().enabled = false;
                healthValue.transform.GetComponent<Image>().enabled = false;
            }
            else
            {
                health.transform.GetComponent<Image>().enabled = true;
                healthValue.transform.GetComponent<Image>().enabled = true;


                Vector3 worldPosition = relatedTransform.position;
                Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
                Vector2 screenPosition = new Vector2
                (
                    viewport.x * canvasRT.sizeDelta.x - canvasRT.sizeDelta.x * 0.5f + offsetX,
                    viewport.y * canvasRT.sizeDelta.y - canvasRT.sizeDelta.y * 0.5f + offsetY
                );

                rt.anchoredPosition = screenPosition;
            }
        }

        if (relatedDamageManager)
        {
            percentHealth = relatedDamageManager.health / relatedDamageManager.maxHealth;
            float parentWidth = health.sizeDelta.x;
            float parentHeight = health.sizeDelta.y;
            float valueWidth = parentWidth * percentHealth;
            healthValue.sizeDelta = new Vector2(valueWidth, parentHeight);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        relatedTransform = newTarget.transform;
        relatedDamageManager = newTarget.transform.GetComponent<DamageManager>();
    }

}
