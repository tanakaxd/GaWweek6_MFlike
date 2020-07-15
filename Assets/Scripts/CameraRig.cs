using UnityEngine;

public class CameraRig : MonoBehaviour
{
    private Camera camera;
    private Transform cameraTransform;
    private Vector3 initialCameraPosRelativeToRig;
    private Vector3 moveVelocity;
    [SerializeField] private float rotationSpeed = 7.0f;
    [SerializeField] private float zoomScale = 1.0f;
    public float dampTime = 0.2f;
    private float yAngleRange = 50;
    private int direction = 1;
    [SerializeField] private Transform[] fighters;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        cameraTransform = camera.transform;
        initialCameraPosRelativeToRig = cameraTransform.localPosition;
    }

    private void Start()
    {
    }

    private void Update()
    {
            Rotate();
            Zoom();

    }

    private void Rotate()
    {

        //Debug.Log(transform.eulerAngles.y);
        float angleY = transform.eulerAngles.y;
        if (angleY >= 180)
        {
            angleY -= 360;
        }
        //Debug.Log(angleY);
        if (angleY >= yAngleRange|| angleY <= -yAngleRange)
        {
            direction *= -1;
        }
        transform.rotation = transform.rotation * Quaternion.AngleAxis(rotationSpeed * Time.deltaTime*direction, Vector3.up);

    }

    private void Zoom()
    {
        float maxDistance = 10;
        float distance = CalcDistance();//0-10
        //Debug.Log(distance);
        float normalizedDistance = Mathf.Lerp(-0.5f, 8, distance / maxDistance);
        //Debug.Log((int)normalizedDistance);

        Vector3 desiredPos = transform.rotation* initialCameraPosRelativeToRig - cameraTransform.forward * normalizedDistance;

        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPos, ref moveVelocity, dampTime);


        //if (Input.GetKey("w"))
        //{
        //    cameraTransform.position = cameraTransform.position + transform.forward * Time.deltaTime;
        //}
        //if (Input.GetKey("s"))
        //{
        //    cameraTransform.position = cameraTransform.position - transform.forward * Time.deltaTime;
        //}

        //cameraTransform.position = initialCameraPos;
    }

    private float CalcDistance()
    {
        return (fighters[0].position - fighters[1].position).magnitude;
    }
}