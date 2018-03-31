using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{

    public Transform relatedTransform;
    private RectTransform rt;
    public RectTransform canvasRT;

    public Camera minimapCamera;

    void Awake()
    {
        InitializeScreenPosition();
    }

    void Update()
    {
        UpdateScreenPosition();
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
            rt.anchoredPosition = CalculateScreenPosition();
        }

    }

    Vector2 CalculateScreenPosition()
    {
        Vector3 worldPosition = relatedTransform.position;
        Vector2 viewport = minimapCamera.WorldToViewportPoint(worldPosition);
        return (new Vector2
        (
            viewport.x * canvasRT.sizeDelta.x - canvasRT.sizeDelta.x * 0.5f,
            viewport.y * canvasRT.sizeDelta.y - canvasRT.sizeDelta.y * 0.5f
        ));
    }

    public void SetTarget(GameObject target)
    {
        if (target)
        {
            relatedTransform = target.transform;
        }
    }

    public void SetCamera(Camera camera)
    {
        if (camera)
        {
            minimapCamera = camera;
        }
    }

}
