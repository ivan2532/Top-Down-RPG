using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrostboltController : MonoBehaviour
{
    private const float disableWaitTime = 5.0f;

    [SerializeField] private float frostboltSpeed = 5.0f;
    [field: SerializeField] public GameObject HitEffectPrefab { get; private set; }

    private Rigidbody myRigidbody;
    private IcemanController icemanController;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        icemanController = FindObjectOfType<IcemanController>();
    }

    private void OnEnable()
    {
        Invoke("DisableFrostbolt", disableWaitTime);
        myRigidbody.velocity = transform.forward * frostboltSpeed;
    }

    private void DisableFrostbolt() => gameObject.SetActive(false);

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var frostbollHitEffect = icemanController.GetNextFrostHitEffectBolt();
        var contant = collision.GetContact(0);
        frostbollHitEffect.transform.position = contant.point + contant.normal * 0.5f;
        frostbollHitEffect.SetActive(true);

        gameObject.SetActive(false);
        CancelInvoke("DisableFrostbolt");
    }
}
