using Cinemachine;
using UnityEngine;

public class VirtualCameraZoomController : MonoBehaviour
{
    [SerializeField] private float zoomSensitivity = 0.5f;
    [SerializeField] private float zoomSmoothness = 0.1f;
    [SerializeField] private float startOrthographicSize = 5.0f;

    [SerializeField] private float minOrthographicSize = 2f;
    [SerializeField] private float maxOrthographicSize = 7f;

    private CinemachineVirtualCamera virtualCamera;
    private float scrollDelta = 0f;
    private float orthographicSizeSmoothVelocity;

    private float newOrthographicSize;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize = startOrthographicSize;
        newOrthographicSize = startOrthographicSize;
    }

    private void Update()
    {
        scrollDelta = -Input.mouseScrollDelta.y;

        if (scrollDelta != 0.0f)
        {
            newOrthographicSize = Mathf.Clamp(
                virtualCamera.m_Lens.OrthographicSize + scrollDelta * zoomSensitivity,
                minOrthographicSize,
                maxOrthographicSize);
        }

        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(
            virtualCamera.m_Lens.OrthographicSize,
            newOrthographicSize,
            ref orthographicSizeSmoothVelocity, zoomSmoothness);
    }
}
