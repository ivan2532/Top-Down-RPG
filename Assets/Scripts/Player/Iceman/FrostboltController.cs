using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrostboltController : MonoBehaviour
{
    private const float disableWaitTime = 5.0f;

    [SerializeField] private float frostboltSpeed = 5.0f;
    [field: SerializeField] public GameObject HitEffectPrefab { get; private set; }
    [SerializeField] private GameObject frostboltPrefab;
    [SerializeField] private Transform spellSpawnTransform;

    private const int frostboltPoolSize = 5;

    private GameObject[] frostboltPool;
    private GameObject[] frostboltHitEffectPool;


    private Rigidbody myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        FrostboltPoolInit();
    }

    private void OnEnable()
    {
        Invoke("DisableFrostbolt", disableWaitTime);
        myRigidbody.velocity = transform.forward * frostboltSpeed;
    }

    private void FrostboltPoolInit()
    {
        frostboltPool = new GameObject[frostboltPoolSize];
        var frostboltPoolParent = new GameObject("Frostbolt Pool Parent").transform;

        for (int i = 0; i < frostboltPool.Length; ++i)
        {
            frostboltPool[i] = Instantiate(frostboltPrefab);
            frostboltPool[i].SetActive(false);
            frostboltPool[i].transform.SetParent(frostboltPoolParent);
        }

        frostboltHitEffectPool = new GameObject[frostboltPoolSize];
        var frostboltHitEffectPoolParent = new GameObject("Frostbolt Hit Effect Pool Parent").transform;

        for (int i = 0; i < frostboltPool.Length; ++i)
        {
            frostboltHitEffectPool[i] = Instantiate(frostboltPrefab.GetComponent<FrostboltController>().HitEffectPrefab);
            frostboltHitEffectPool[i].SetActive(false);
            frostboltHitEffectPool[i].transform.SetParent(frostboltHitEffectPoolParent);
        }
    }

    private void DisableFrostbolt() => gameObject.SetActive(false);

    //Called by animation event
    public void SpawnFrostbolt()
    {
        var frostbolt = GetNextFrostbolt();

        var frostboltTransform = frostbolt.transform;

        frostboltTransform.rotation = transform.rotation;
        frostboltTransform.position = spellSpawnTransform.position;

        frostbolt.SetActive(true);
    }

    //Called by animation event
    public void EndFrostboltCast() => casting = false;

    public GameObject GetNextFrostbolt()
    {
        foreach (var frostbolt in frostboltPool)
            if (!frostbolt.activeInHierarchy)
                return frostbolt;

        Debug.LogError("Pool size is too small!");
        Debug.Break();
        return null;
    }

    public GameObject GetNextFrostHitEffectBolt()
    {
        foreach (var frostboltHitEffect in frostboltHitEffectPool)
            if (!frostboltHitEffect.activeInHierarchy)
                return frostboltHitEffect;

        Debug.LogError("Pool size is too small!");
        Debug.Break();
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var frostboltHitEffect = GetNextFrostHitEffectBolt();
        var contact = collision.GetContact(0);
        frostboltHitEffect.transform.position = contact.point + contact.normal * 0.5f;
        frostboltHitEffect.SetActive(true);

        gameObject.SetActive(false);
        CancelInvoke("DisableFrostbolt");
    }
}
