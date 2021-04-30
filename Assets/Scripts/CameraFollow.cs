using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;

    public Transform target;

    /// <summary>
    /// The camera's left to right value
    /// </summary>
    private float yaw = 0;

    /// <summary>
    /// The camera's up and down value
    /// </summary>
    private float pitch = 0;

    public float camSensitivityX = 8;
    public float camSensitivityY = 8;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        DoOrbit();

        transform.position = AnimMath.Slide(transform.position, target.position, .001f);
    }

    void DoOrbit()
    {
        // Reference the mouse positions
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        yaw += mouseX * camSensitivityX;
        pitch += mouseY * camSensitivityY;

        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);

        pitch = Mathf.Clamp(pitch, 5, 70);
    }
}
