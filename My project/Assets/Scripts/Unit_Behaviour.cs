using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Behaviour : MonoBehaviour
{
    [SerializeField] private GameMechanic_Controller Main_Controller;
    [SerializeField] private LayerMask layerMask;


    /*[HideInInspector] */public GameObject tile;
    [HideInInspector] public bool selected;

    public string piece;
    
    // Start is called before the first frame update
    void Start()
    {
        Main_Controller = GameObject.Find("Game Controller").GetComponent<GameMechanic_Controller>();

        if(Physics.Raycast(transform.position, -Vector3.up, out RaycastHit tileHit, 5.0f, layerMask))
        {
            tile = tileHit.transform.gameObject.transform.parent.gameObject;
            tile.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(selected && Main_Controller.selectedUnit == this.gameObject)
        //{
        //    Ray ray = CameraBase_Controller.MainCam_Instance.camera.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit rayHit, float.MaxValue, layerMask))
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(rayHit.transform.gameObject.transform.position.x, transform.position.y, rayHit.transform.gameObject.transform.position.z), float.MaxValue);
        //            Debug.Log("Move to : " + rayHit.transform.gameObject.transform.parent.name);

        //            if(rayHit.transform.gameObject != tile)
        //            {
        //                tile = rayHit.transform.gameObject;

        //                if(gameObject.tag == "Red Unit")
        //                {
        //                    Main_Controller.playing_Turn = GameMechanic_Controller.Turn.bluePlayer;
        //                    selected = false;
        //                }
        //                if(gameObject.tag == "Blue Unit")
        //                {
        //                    Main_Controller.playing_Turn = GameMechanic_Controller.Turn.redPlayer;
        //                    selected = false;
        //                }
        //            }
        //        }
        //    }
        //}
    }

    private void OnMouseDown()
    {
        if (selected)
        {
            selected = false;
            Main_Controller.selectedUnit = null;
            Debug.Log("Deselected : " + gameObject.name);
        }
        else
        {
            selected = true;
            Main_Controller.selectedUnit = this;
            Debug.Log("Selecting : " + gameObject.name);
        }
    }
}
