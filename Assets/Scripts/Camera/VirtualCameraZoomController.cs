using Cinemachine;
using UnityEngine;

public class VirtualCameraZoomController : MonoBehaviour
{
    [SerializeField] private float zoomSensitivity = 0.3f;
    [SerializeField] private float minOrthographicSize = 2f;
    [SerializeField] private float maxOrthographicSize = 7f;

    private CinemachineVirtualCamera virtualCamera;
    private float scrollDelta = 0f;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        scrollDelta = -Input.mouseScrollDelta.y;

        if (scrollDelta != 0.0f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(
                virtualCamera.m_Lens.OrthographicSize + scrollDelta * zoomSensitivity,
                minOrthographicSize,
                maxOrthographicSize);
        }
    }
}
