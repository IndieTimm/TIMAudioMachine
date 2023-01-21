using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Zoomer : MonoBehaviour
{
    [SerializeField] private float zoom = 2.0F;
    [SerializeField] private float zoomSpeed = 4.0F;

    private Camera currentCamera;
    private float defaultFieldOfView;
    private float zoomFactor = 0;

    private void Awake()
    {
        currentCamera = GetComponent<Camera>();
        defaultFieldOfView = currentCamera.fieldOfView;
    }
    private void Update()
    {
        var factor = zoomFactor;

        if (Input.GetKey(KeyCode.C))
        {
            factor += zoomSpeed * Time.deltaTime;
        }
        else
        {
            factor -= 4 * zoomSpeed * Time.deltaTime;
        }

        if (!Mathf.Approximately(factor, zoomFactor))
        {
            zoomFactor = Mathf.Clamp01(factor);
            currentCamera.fieldOfView = defaultFieldOfView * (1 - (1 / zoom) * zoomFactor);
        }
    }
}
