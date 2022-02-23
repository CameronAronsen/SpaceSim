using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float scrollSensitivity = 10f;
    public float rotateSensitivity = 50f;
    public float panSensitivity = 50f;

    public Transform focusObject;
    public bool focused;

    public float height;
    public float distance;

    public float minAngle;
    public float maxAngle;

    public float minDistance;
    public float maxDistance;

    // Unfocused
    private float xRotation;
    private float yRotation;

    // Focused
    private float xRotationFocus;
    private float yRotationFocus;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    public float smoothTime = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        transform.LookAt(focusObject.transform);
        transform.position = new Vector3(0, height, distance);
        currentRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (focused)
        {
            transform.LookAt(focusObject.transform);
            UpdateCamera();
        }
        if (Input.GetMouseButton(2))
        {
            PanCamera();
        }
        else if (Input.GetMouseButton(1) && focused)
        {
            RotateCameraFocused();
        }
        else if (Input.GetMouseButton(1) && !focused)
        {
            RotateCameraUnFocused();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ZoomCamera();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    void PanCamera()
    {
        focused = false;
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");
        Vector3 rotation = new Vector3(xAxis, yAxis, 0) * rotateSensitivity * Time.deltaTime * 10;
        transform.Translate(-rotation);
    }

    void RotateCameraUnFocused()
    {
        float xAxis = Input.GetAxis("Mouse X") * rotateSensitivity * Time.deltaTime * 10;
        float yAxis = Input.GetAxis("Mouse Y") * rotateSensitivity * Time.deltaTime * 10;

        yRotation += xAxis;
        xRotation -= yAxis;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        Vector3 nextRotation = new Vector3(xRotation, yRotation);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

    }

    void RotateCameraFocused()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotateSensitivity * Time.deltaTime * 10;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSensitivity * Time.deltaTime * 10;

        yRotationFocus += mouseX;
        xRotationFocus += -mouseY;

        xRotationFocus = Mathf.Clamp(xRotationFocus, minAngle, maxAngle);

        Vector3 nextRotation = new Vector3(xRotationFocus, yRotationFocus);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;
        float distanceFromTarget = Vector3.Distance(focusObject.position, transform.position);
        transform.position = focusObject.position - transform.forward * distanceFromTarget;
    }    

    void ZoomCamera()
    {
        float scrollVal = Input.GetAxis("Mouse ScrollWheel");
        Vector3 direction = transform.position - focusObject.position;

        Vector3 movement = -(direction * scrollVal * scrollSensitivity * Time.deltaTime * 10);
        transform.position += movement;

        direction = transform.position - focusObject.position;

        if (direction.magnitude < minDistance)
        {
            direction = direction.normalized * minDistance;
            transform.position = direction;
        }
        else if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
            transform.position = direction;
        }
    }

    void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            focused = true;
            focusObject = hit.transform;
            minDistance = hit.transform.localScale.x;
        }
        else if(Universe.planetSelected != Universe.planetTypes.None)
        {

        }
    }

    void UpdateCamera()
    {
        if(focusObject.name != "Sun")
        {
            transform.position = focusObject.position + new Vector3(0, height, distance);
        }
    }

}
