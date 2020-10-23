using UnityEngine;

public class ShardController : MonoBehaviour
{
    public const float scaleRandomness = 0.03f;
    public const float startingScale = 0.125f;

    private float targetScale;
    private Quaternion targetRotation;

    private Vector3 scaleSmoothness;
    private Quaternion rotationSmoothness;

    private float scaleSmoothAmount = 0.2f;
    private float rotationSmoothAmount = 0.2f;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, 
            new Vector3(targetScale, targetScale, targetScale * 1.8f), 
            ref scaleSmoothness, scaleSmoothAmount);

        transform.rotation = SmoothDampUtility.QuaternionSmoothDamp(transform.rotation, targetRotation, ref rotationSmoothness, rotationSmoothAmount);
    }

    public void PlayEffect()
    {
        scaleSmoothAmount = 0.05f;
        rotationSmoothAmount = 0.05f;

        targetRotation = Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 180.0f);
        targetScale = Random.Range(startingScale - scaleRandomness, startingScale + scaleRandomness);
    }

    public void EndEffect()
    {
        scaleSmoothAmount = 0.05f;
        rotationSmoothAmount = 0.04f;

        targetRotation = Quaternion.Euler(90.0f, 0.0f, 180.0f);
        targetScale = 0.0f;
    }
}
