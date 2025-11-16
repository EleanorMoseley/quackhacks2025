using UnityEngine;

public class ReticleEffects : MonoBehaviour
{
    private Transform reticleTransform;
    private Vector3 baseScale;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilFrequency = 20f;   // how fast it pulses
    [SerializeField] private float minScale = 0.8f;         // minimum multiplier
    [SerializeField] private float maxScale = 1.3f;         // maximum multiplier

    void Start()
    {
        reticleTransform = transform;
        baseScale = reticleTransform.localScale;
    }

    void Update()
    {
        RecoilEffect();
    }

    private void RecoilEffect()
    {
        // Sin wave between -1 and 1
        float s = Mathf.Sin(Time.time * recoilFrequency);

        // Remap -1..1 -> 0..1
        float t = (s + 1f) * 0.5f;

        // Lerp between min and max scale multipliers
        float scaleFactor = Mathf.Lerp(minScale, maxScale, t);

        // Apply relative to original size
        reticleTransform.localScale = baseScale * scaleFactor;
    }
}
