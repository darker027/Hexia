using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawntest : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    bool selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selected)
        {
            Ray ray = CameraBase_Controller.MainCam_Instance.camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit rayHit, float.MaxValue, layerMask))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(rayHit.transform.gameObject.transform.position.x, transform.position.y, rayHit.transform.gameObject.transform.position.z), float.MaxValue);
                    Debug.Log("Move to : " + rayHit.transform.gameObject.transform.parent.name);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if(selected)
        {
            selected = false;
            Debug.Log("Deselected : " + gameObject.name);
        }
        else
        {
            selected = true;
            Debug.Log("Selecting : " + gameObject.name);
        }
    }
}
