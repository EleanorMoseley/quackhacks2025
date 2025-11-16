using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthAnimation : MonoBehaviour
{
    [SerializeField] private PlayerStatsComponent playerStatsComponent;
    [SerializeField] private Image damageImage;       // UI Image to flash
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 1f;

    private Color _baseColor;
    private Coroutine _flashRoutine;

    private void Awake()
    {
        if (damageImage != null)
            _baseColor = damageImage.color;
    }

    private void Update()
    {
        // Did we take damage this frame?
        if (playerStatsComponent != null && playerStatsComponent.tookDamage)
        {
            // Reset the flag so we only respond once
            playerStatsComponent.tookDamage = false;

            // Restart the flash if one is already running
            if (_flashRoutine != null)
                StopCoroutine(_flashRoutine);

            _flashRoutine = StartCoroutine(FlashDamage());
        }
    }

    private IEnumerator FlashDamage()
    {
        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = timer / flashDuration;  // 0 -> 1 over time

            // Sine wave: 0 -> 1 -> 0 as t goes 0 -> 1
            float wave = Mathf.Sin(t * Mathf.PI);

            // Lerp between base and flash color using the wave
            if (damageImage != null)
                damageImage.color = Color.Lerp(_baseColor, flashColor, wave);

            yield return null;
        }

        // Reset color at the end
        if (damageImage != null)
            damageImage.color = _baseColor;

        _flashRoutine = null;
    }
}
