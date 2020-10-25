using UnityEngine;

public class FrostboltHitEffectDisabler : MonoBehaviour
{
    private const float disableWaitTime = 1.0f;

    private void OnEnable()
    {
        CancelInvoke("DisableHitEffect");
        Invoke("DisableHitEffect", disableWaitTime);
    }

    private void DisableHitEffect() => gameObject.SetActive(false);
}
