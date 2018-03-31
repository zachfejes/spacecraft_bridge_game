using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: Make this a subclass of the standard Status Bar class, but with screen-tracking

public class TrackingStatusBar : StatusBar
{

    private RectTransform rt;
    public RectTransform canvasRT;
    public float offsetY = 0.0f;
    public float offsetX = 0.0f;

    void Awake()
    {
        Initialize();
        InitializeScreenPosition();
    }

    void Update()
    {
        UpdateScreenPosition();
        UpdateDamageBar();
        UpdateShieldBar();
    }

    void InitializeScreenPosition()
    {
        rt = GetComponent<RectTransform>();

        if (relatedTransform)
        {
            rt.anchoredPosition = CalculateScreenPosition();
        }
    }

    void UpdateScreenPosition()
    {
        if (relatedTransform)
        {
            Vector3 behindCamera = Camera.main.WorldToViewportPoint(relatedTransform.position);

            if (behindCamera.z < 0)
            {
                ToggleStatusBars(false);
            }
            else
            {
                ToggleStatusBars(true);
                rt.anchoredPosition = CalculateScreenPosition();
            }
        }
    }

    Vector2 CalculateScreenPosition()
    {
        Vector3 worldPosition = relatedTransform.position;
        Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
        return (new Vector2
        (
            viewport.x * canvasRT.sizeDelta.x - canvasRT.sizeDelta.x * 0.5f + offsetX,
            viewport.y * canvasRT.sizeDelta.y - canvasRT.sizeDelta.y * 0.5f + offsetY
        ));
    }

    void ToggleStatusBars(bool show)
    {
        health.transform.GetComponent<Image>().enabled = show;
        healthValue.transform.GetComponent<Image>().enabled = show;
        shield.transform.GetComponent<Image>().enabled = show;
        shieldValue.transform.GetComponent<Image>().enabled = show;
    }

}
