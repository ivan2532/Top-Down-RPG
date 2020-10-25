using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrostboltController : MonoBehaviour
{
    private const float disableWaitTime = 3.0f;

    [SerializeField] private float frostboltSpeed = 5.0f;
    [SerializeField] private GameObject hitEffect;

    private const int frostboltPoolSize = 5;
    private const string frostboltPrefabPath = "Spell Effects/Frostbolt Projectile";

    private static ObjectPool frostboltPool = new ObjectPool();

    private Rigidbody myRigidbody;

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethodLoad()
    {
        frostboltPool.InitializePool(frostboltPoolSize, Resources.Load<GameObject>(frostboltPrefabPath), "Frostbolt Pool");
    }

    public static void SpawnFrostbolt(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        var frostbolt = frostboltPool.GetNextObjectFromPool();
        var frostboltTransform = frostbolt.transform;

        frostboltTransform.position = spawnPosition;
        frostboltTransform.rotation = spawnRotation;

        frostbolt.SetActive(true);
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        hitEffect.transform.SetParent(null);
    }

    private void OnEnable()
    {
        CancelInvoke("DisableFrostbolt");
        Invoke("DisableFrostbolt", disableWaitTime);
        myRigidbody.velocity = transform.forward * frostboltSpeed;
    }

    private void DisableFrostbolt() => gameObject.SetActive(false);

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var contact = collision.GetContact(0);
        hitEffect.transform.position = contact.point + contact.normal * 0.5f;

        //Reset particle effect
        hitEffect.SetActive(false);
        hitEffect.SetActive(true);

        gameObject.SetActive(false);
        CancelInvoke("DisableFrostbolt");
    }
}
