using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class IcemanController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    private bool _casting = false;
    private float _inputX, _inputY;

    #region Movement variables
    private Vector3 _cameraPlayerY;
    private Vector3 _forwardDirection;
    private Vector3 _rightDirection;

    private float _currentSpeed = 0.0f;
    private Vector3 _lastPosition;

    private Quaternion _targetRotation;
    private Quaternion _rotationSmoothVelocity;
    #endregion

    #region Component refrences
    private Rigidbody _rigidBody;
    private Animator _animator;
    #endregion

    #region MonoBehaviour Events
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _lastPosition = transform.position;
        _targetRotation = transform.rotation;

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
        _currentSpeed = (transform.position - _lastPosition).sqrMagnitude;
        _lastPosition = transform.position;

        Vector3 movementInput = /*something.casting*/true ? Vector3.zero : new Vector3(_inputX, 0.0f, _inputY).normalized * movementSpeed;
        _rigidBody.velocity = _forwardDirection * movementInput.z + _rightDirection * movementInput.x + new Vector3(0.0f, _rigidBody.velocity.y);
        _animator.SetFloat("Speed", _currentSpeed / 0.01f);

        if(_rigidBody.velocity.sqrMagnitude > movementSpeed * movementSpeed / 2.0f)
            _targetRotation = Quaternion.LookRotation(_rigidBody.velocity);
    }

    private void RotatePlayer()
    {
        _targetRotation.eulerAngles = new Vector3(0.0f, _targetRotation.eulerAngles.y);
        Quaternion smoothedRotation = SmoothDampUtility.QuaternionSmoothDamp(transform.rotation, _targetRotation, ref _rotationSmoothVelocity, 0.02f);
        _rigidBody.MoveRotation(smoothedRotation);
    }

    private void ProcessInput()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            if (_casting)
                return;

            _casting = true;

            SetPlayerTargetRotation();
            _animator.SetTrigger("Frostbolt");
        }
    }

    private void SetPlayerTargetRotation()
    {
        var playerXZPlane = new Plane(Vector3.up, transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (playerXZPlane.Raycast(ray, out float distance))
        {
            var hitPoint = ray.GetPoint(distance);
            _targetRotation = Quaternion.LookRotation(hitPoint - transform.position);
        }
    }

    private void CalculateAxes()
    {
        Vector3 mainCameraPosition = Camera.main.transform.position;
        _cameraPlayerY = new Vector3(mainCameraPosition.x, transform.position.y, mainCameraPosition.z);
        _forwardDirection = (transform.position - _cameraPlayerY).normalized;
        _rightDirection = Vector3.Cross(Vector3.up, _forwardDirection);
    }
}
