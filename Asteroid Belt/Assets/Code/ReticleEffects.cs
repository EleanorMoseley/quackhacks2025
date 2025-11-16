using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleEffects : MonoBehaviour
{
    private Transform reticleTransform;

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
        transform.localScale += Mathf.Sin(Time.time * 20) * 0.1f * Vector3.one;
    }
}
