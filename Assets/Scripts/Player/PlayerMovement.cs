using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    private float inputX, inputY;

    private Vector3 cameraPlayerY;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private float currentSpeed = 0.0f;
    Vector3 lastPosition;

    private Quaternion rotationSmoothVelocity;

    private Rigidbody myRigidbody;
    private Animator myAnimator;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();

        lastPosition = transform.position;

        CalculateAxes();
    }

    private void FixedUpdate()
    {
        currentSpeed = (transform.position - lastPosition).sqrMagnitude;
        lastPosition = transform.position;

        Vector3 movementInput = new Vector3(inputX, 0.0f, inputY).normalized * movementSpeed;
        myRigidbody.velocity = forwardDirection * movementInput.z + rightDirection * movementInput.x + new Vector3(0.0f, myRigidbody.velocity.y);
        myAnimator.SetFloat("Speed", currentSpeed / 0.01f);
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (myRigidbody.velocity.sqrMagnitude > movementSpeed * movementSpeed / 2.0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(myRigidbody.velocity);
            newRotation.eulerAngles = new Vector3(0.0f, newRotation.eulerAngles.y);
            Quaternion smoothedRotation = SmoothDampUtility.QuaternionSmoothDamp(transform.rotation, newRotation, ref rotationSmoothVelocity, 0.02f);
            myRigidbody.MoveRotation(smoothedRotation);
        }
    }

    private void CalculateAxes()
    {
        Vector3 mainCameraPosition = Camera.main.transform.position;
        cameraPlayerY = new Vector3(mainCameraPosition.x, transform.position.y, mainCameraPosition.z);
        forwardDirection = (transform.position - cameraPlayerY).normalized;
        rightDirection = Vector3.Cross(Vector3.up, forwardDirection);
    }
}
