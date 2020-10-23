﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IcemanController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    [SerializeField] private Transform spellSpawnTransform;
    [SerializeField] private GameObject frostboltPrefab;
    
    private const int frostboltPoolSize = 5;

    private GameObject[] frostboltPool;

    private float inputX, inputY;

    private bool casting = false;

    #region Movement variables
    private Vector3 cameraPlayerY;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private float currentSpeed = 0.0f;
    private Vector3 lastPosition;

    private Quaternion rotationSmoothVelocity;
    #endregion

    #region Component refrences
    private Rigidbody myRigidbody;
    private Animator myAnimator;
    #endregion

    #region MonoBehaviour Events
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();

        lastPosition = transform.position;

        FrostBoltPoolInit();

        CalculateAxes();
    }

    private void FrostBoltPoolInit()
    {
        frostboltPool = new GameObject[frostboltPoolSize];
        var poolParent = new GameObject("Frostbolt Pool Parent").transform;

        for (int i = 0; i < frostboltPool.Length; ++i)
        {
            frostboltPool[i] = Instantiate(frostboltPrefab);
            frostboltPool[i].SetActive(false);
            frostboltPool[i].transform.SetParent(poolParent);
        }
    }

    private GameObject GetNextFrostBolt()
    {
        foreach (var frostbolt in frostboltPool)
            if (!frostbolt.activeInHierarchy) 
                return frostbolt;

        Debug.LogError("Pool size is too small!");
        Debug.Break();
        return null;
    }

    private IEnumerator DisableFrostbolt(GameObject frostbolt)
    {
        yield return new WaitForSeconds(5);
        frostbolt.SetActive(false);
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
    }

    private void RotatePlayer()
    {
        if (myRigidbody.velocity.sqrMagnitude > movementSpeed * movementSpeed / 2.0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(myRigidbody.velocity);
            newRotation.eulerAngles = new Vector3(0.0f, newRotation.eulerAngles.y);
            Quaternion smoothedRotation = SmoothDampUtility.QuaternionSmoothDamp(transform.rotation, newRotation, ref rotationSmoothVelocity, 0.02f);
            myRigidbody.MoveRotation(smoothedRotation);
        }
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

    private void CastFrostbolt()
    {
        if (casting)
            return;

        casting = true;
        myAnimator.SetTrigger("Frostbolt");
    }
    
    //Called by animation event
    public void SpawnFrostbolt()
    {
        var frostbolt = GetNextFrostBolt();

        frostbolt.SetActive(true);
        StartCoroutine(DisableFrostbolt(frostbolt));

        var frostboltTransform = frostbolt.transform;
        frostboltTransform.rotation = transform.rotation;
        frostboltTransform.position = spellSpawnTransform.position;
    }

    //Called by animation event
    public void EndFrostboltCast() => casting = false;
}
