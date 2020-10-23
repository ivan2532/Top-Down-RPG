using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GroundFreezeEffect : MonoBehaviour
{
    private ShardController[] shards;
    private Material myMaterial;

    private bool effectInProgress = false;

    private float effectDuration = 1.0f;
    private float areaAlphaSmoothness;

    private void Awake()
    {
        shards = GetComponentsInChildren<ShardController>();
        myMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        float newAlpha = Mathf.SmoothDamp(myMaterial.color.a, 
            effectInProgress ? 0.04f : 0.0f, 
            ref areaAlphaSmoothness, 
            effectInProgress ? 0.2f : 0.05f);

        myMaterial.color = new Color(myMaterial.color.r, myMaterial.color.g, myMaterial.color.b, newAlpha);

        if (Input.GetKeyDown(KeyCode.T))
            PlayEffect(0.5f);
    }

    public void PlayEffect(float duration)
    {
        effectDuration = duration;
        effectInProgress = true;

        foreach (var shard in shards)
            shard.PlayEffect();

        StartCoroutine(EndEffect());
    }

    private IEnumerator EndEffect()
    {
        yield return new WaitForSeconds(effectDuration);

        effectInProgress = false;

        foreach (var shard in shards)
            shard.EndEffect();
    }
}
