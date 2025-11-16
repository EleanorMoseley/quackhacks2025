using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleEffects : MonoBehaviour
{
    private Transform reticleTransform;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilStrength = 0.1f;       // Controls how large the reticle expansion is
    [SerializeField] private float recoilFrequency = 20f;      // Controls how fast the reticle expands and contracts
    [SerializeField] private float damping = 0.98f;              // 0.9 = fast, fade, 0.99 = slow fade
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.3f;
    [SerializeField] private float randomShake = 0.02f;

    private float currentStrength;

    void Start()
    {
        reticleTransform = this.transform;
    }

    void Update()
    {
        RecoilEffect(reticleTransform);
    }

    private void RecoilEffect(Transform transform)
    {
        currentStrength = Mathf.Lerp(currentStrength, recoilStrength, Time.deltaTime * 10f);
        currentStrength *= damping;

        float pulse = Mathf.Sin(Time.time * recoilFrequency) * currentStrength;

        Vector3 targetScale =
            Vector3.one * (1f + pulse)
            + (Vector3.one * Random.Range(-randomShake, randomShake));


        float clamped = Mathf.Clamp(targetScale.x, minScale, maxScale);
        transform.localScale = Vector3.one * clamped;

        //transform.localScale += Mathf.Sin(Time.time * 20) * 0.1f * Vector3.one;
    }
}
