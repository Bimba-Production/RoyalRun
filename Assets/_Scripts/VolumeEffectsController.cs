using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts
{
    public sealed class VolumeEffectsController: Singleton<VolumeEffectsController>
    {
        public void ApplyEffect(float intensity, Volume damageEffect, float duration)
        {
            StopCoroutine(nameof(ApplyEffectRoutine));
            StartCoroutine(ApplyEffectRoutine(0.7f, damageEffect, duration));
        }
        
        public void DisableEffect(Volume damageEffect, float duration)
        {
            StopCoroutine(nameof(ApplyEffectRoutine));
            StartCoroutine(ApplyEffectRoutine(0f, damageEffect, duration));
        }

        IEnumerator ApplyEffectRoutine(float intensity, Volume damageEffect, float duration)
        {
            float startWeight = damageEffect.weight;
            float targetWeight = intensity;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                elapsedTime += Time.deltaTime;
                damageEffect.weight = Mathf.Lerp(startWeight, targetWeight, t);
                yield return null;
            }

            damageEffect.weight = targetWeight;
        }
    }
}