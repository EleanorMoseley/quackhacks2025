using UnityEngine;

public class ReticleTracking : MonoBehaviour
{
    [SerializeField] private RectTransform reticle; // UI reticle
    [SerializeField] private Canvas canvas;

    void Update()
    {
        if (reticle == null || canvas == null)
            return;

        Vector2 pos;
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            cam,
            out pos);

        reticle.anchoredPosition = pos;
    }
}
