using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IcemanController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    [SerializeField] private Transform spellSpawnTransform;
    [SerializeField] private GameObject frostboltPrefab;
    
    private const int frostboltPoolSize = 5;

    private GameObject[] frostboltPool;
    private GameObject[] frostboltHitEffectPool;

    private float inputX, inputY;

    private bool casting = false;

    #region Movement variables
    private Vector3 cameraPlayerY;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private float currentSpeed = 0.0f;
    private Vector3 lastPosition;

    private Quaternion targetRotation;
    private Quaternion rotationSmoothVelocity;
    #endregion

    #region Component refrences
    private Rigidbody myRigidbody;
    private Animator myAnimator;
    private Camera mainCamera;
    #endregion

    #region MonoBehaviour Events
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        lastPosition = transform.position;
        targetRotation = transform.rotation;

        FrostboltPoolInit();
        CalculateAxes();
    }

    private void Update()
    {
        ProcessInput();
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    #endregion

    private void PlayerMovement()
    {
        currentSpeed = (transform.position - lastPosition).sqrMagnitude;
        lastPosition = transform.position;

        Vector3 movementInput = casting ? Vector3.zero : new Vector3(inputX, 0.0f, inputY).normalized * movementSpeed;
        myRigidbody.velocity = forwardDirection * movementInput.z + rightDirection * movementInput.x + new Vector3(0.0f, myRigidbody.velocity.y);
        myAnimator.SetFloat("Speed", currentSpeed / 0.01f);

        if(myRigidbody.velocity.sqrMagnitude > movementSpeed * movementSpeed / 2.0f)
            targetRotation = Quaternion.LookRotation(myRigidbody.velocity);
    }

    private void RotatePlayer()
    {
        targetRotation.eulerAngles = new Vector3(0.0f, targetRotation.eulerAngles.y);
        Quaternion smoothedRotation = SmoothDampUtility.QuaternionSmoothDamp(transform.rotation, targetRotation, ref rotationSmoothVelocity, 0.02f);
        myRigidbody.MoveRotation(smoothedRotation);
    }

    private void ProcessInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButtonDown(0))
            CastFrostbolt();
    }

    private void CalculateAxes()
    {
        Vector3 mainCameraPosition = Camera.main.transform.position;
        cameraPlayerY = new Vector3(mainCameraPosition.x, transform.position.y, mainCameraPosition.z);
        forwardDirection = (transform.position - cameraPlayerY).normalized;
        rightDirection = Vector3.Cross(Vector3.up, forwardDirection);
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

    private void CastFrostbolt()
    {
        if (casting)
            return;

        var playerXZPlane = new Plane(Vector3.up, transform.position);

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (playerXZPlane.Raycast(ray, out float distance))
        {
            var hitPoint = ray.GetPoint(distance);
            targetRotation = Quaternion.LookRotation(hitPoint - transform.position);
        }

        casting = true;
        myAnimator.SetTrigger("Frostbolt");
    }
    
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
}
