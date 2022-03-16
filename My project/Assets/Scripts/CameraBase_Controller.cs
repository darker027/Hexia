using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase_Controller : MonoBehaviour
{
    private static CameraBase_Controller mainCam_Instance;
    public static CameraBase_Controller MainCam_Instance { get {return mainCam_Instance; } }

    public Camera camera;

    [Header("Camera Setting")]
    public Vector3 anchorOffset;
    public Vector3 cameraOffset;

    [Header("Camera Transform")]
    [SerializeField] private float minZoomIn_Distance;
    [SerializeField] private float maxZoomOut_Distance;

    private float zoomDistance;

    [Header("Camera Rotation")]
    [SerializeField] private float rotationSmoothing;

    private float rotateY;
    private float angleY;

    private void Awake()
    {
        if (mainCam_Instance != null && mainCam_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            mainCam_Instance = this;
        }
    }

    void Start()
    {
        camera = gameObject.GetComponentInChildren<Camera>();

        rotateY = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.localPosition = cameraOffset;
        camera.transform.LookAt(this.gameObject.transform);

        Camera_Rotation();
    }

    //private void Camera_Transform()
    //{
    //    //
    //    float currentdistance = Vector3.Distance(camera.transform.position, gameObject.transform.position);

    //    if(currentdistance != zoomDistance)
    //    {
    //        if(currentdistance > )
    //    }
    //}

    private void Camera_Rotation()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            rotateY += 60;
            Debug.Log("Turn left!");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotateY -= 60;
            Debug.Log("Turn right!");
        }

        angleY = Mathf.Lerp(angleY, rotateY, Time.deltaTime * rotationSmoothing);
        Quaternion rotation = Quaternion.Euler(0, angleY, 0);
        gameObject.transform.rotation = rotation;
    }
}
