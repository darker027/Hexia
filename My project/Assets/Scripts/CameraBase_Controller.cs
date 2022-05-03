using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase_Controller : MonoBehaviour
{
    private static CameraBase_Controller mainCam_Instance;
    public static CameraBase_Controller MainCam_Instance { get {return mainCam_Instance; } }

    [SerializeField] private GameMechanic_Controller Main_Controller;

    public Camera camera;

    [Header("Camera Setting")]
    public Vector3 anchorOffset;
    public Vector3 cameraOffset;
    private Vector3 targetOffset;

    [Header("Camera Transform")]
    [SerializeField] private float minZoomIn_Distance;
    [SerializeField] private float maxZoomOut_Distance;

    private float zoomDistance;
    private float zoomSpeed = 2f;

    [SerializeField] private float cameraAngle;

    [Header("Camera Rotation")]
    [SerializeField] private float rotationSmoothing;

    [SerializeField] private float baseRotateY;
    private float baseAngleY;

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

        zoomDistance = 7.5f;
        baseRotateY = 90.0f;

        Main_Controller = GameObject.Find("Game Controller").GetComponent<GameMechanic_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.LookAt(this.gameObject.transform);

        Camera_Rotation();
        Camera_Zooming();

        if(Main_Controller.selectedUnit != null)
        {
            transform.position = Vector3.Lerp(transform.position, Main_Controller.selectedUnit.transform.position, Time.deltaTime * rotationSmoothing);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime * rotationSmoothing);
        }
    }


    private void Camera_Rotation()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            baseRotateY += 60;
            if(Main_Controller.selectedUnit != null)
            {
                StartCoroutine(ChangeAngle());
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            baseRotateY -= 60;
            if (Main_Controller.selectedUnit != null)
            {
                StartCoroutine(ChangeAngle());
            }
        }

        baseAngleY = Mathf.Lerp(baseAngleY, baseRotateY, Time.deltaTime * rotationSmoothing);
        Quaternion rotation = Quaternion.Euler(0, baseAngleY, 0);
        gameObject.transform.rotation = rotation;

        if(Input.GetKey(KeyCode.W))
        {
            cameraAngle++;
            cameraAngle = Mathf.Clamp(cameraAngle, 5.0f, 85.0f);
        }

        if(Input.GetKey(KeyCode.S))
        {
            cameraAngle--;
            cameraAngle = Mathf.Clamp(cameraAngle, 5.0f, 85.0f);
        }
    }

    private void Camera_Zooming()
    {
        //zoomDistance = Vector3.Distance(camera.transform.position, gameObject.transform.position);

        if(zoomDistance >= minZoomIn_Distance && zoomDistance <= maxZoomOut_Distance)
        {
            zoomDistance += Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
            zoomDistance = Mathf.Clamp(zoomDistance, minZoomIn_Distance, maxZoomOut_Distance);
            //Debug.Log("Can zoom");
        }

        cameraOffset.z = -Mathf.Abs(Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * zoomDistance);
        cameraOffset.y = Mathf.Abs(Mathf.Sin(cameraAngle * Mathf.Deg2Rad) * zoomDistance);
        camera.transform.localPosition = cameraOffset;
    }

    private IEnumerator ChangeAngle()
    {
        yield return new WaitForSeconds(0.5f);
        Main_Controller.moveClear = true;
    }
}
