using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMechanic_Controller : MonoBehaviour
{
    public static GameMechanic_Controller _Instance;

    public enum Turn {redPlayer, bluePlayer, Gameover}
    public Turn playing_Turn;

    private GameObject[] redUnits;
    private GameObject[] blueUnits;

    [Header("Player turn")]
    public Unit_Behaviour selectedUnit;

    private bool enableRed;
    private bool disableRed;
    private bool enableBlue;
    private bool disableBlue;

    [Header("Unit Movement")]
    [SerializeField] private LayerMask layerMask;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI victoryText;
    [SerializeField] private Image turnColor;

    // Start is called before the first frame update
    void Start()
    {
        redUnits = GameObject.FindGameObjectsWithTag("Red Unit");
        blueUnits = GameObject.FindGameObjectsWithTag("Blue Unit");

        playing_Turn = Turn.redPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playing_Turn);

        if(playing_Turn == Turn.redPlayer)
        {
            turnColor.color = Color.red;

            if(!enableRed)
            {
                foreach (GameObject units in redUnits)
                {
                    if(units != null)
                    {
                        units.layer = 0;
                    }
                }
                enableRed = true;
                enableBlue = false;
            }
            if(!disableBlue)
            {
                foreach(GameObject units in blueUnits)
                {
                    if (units != null)
                        units.layer = 2;
                }
                disableBlue = true;
                disableRed = false;
            }
            victoryText.text = "Red Victory";
            if (selectedUnit != null)
            {
                Unit_Moving();
            }
        }
        else if(playing_Turn == Turn.bluePlayer)
        {
            turnColor.color = Color.blue;

            if (!enableBlue)
            {
                foreach (GameObject units in blueUnits)
                {
                    if (units != null)
                        units.layer = 0;
                }
                enableBlue = true;
                enableRed = false;
            }
            if (!disableRed)
            {
                foreach (GameObject units in redUnits)
                {
                    if(units != null)
                    units.layer = 2;
                }
                disableRed = true;
                disableBlue = false;
            }
            victoryText.text = "Blue Victory";
            if (selectedUnit != null)
            {
                Unit_Moving();
            }
        }
        else
        {
            victoryText.enabled = true;
        }
    }

    private void Unit_Moving()
    {
        Ray rayCast = CameraBase_Controller.MainCam_Instance.camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayCast, out RaycastHit rayHit, float.MaxValue, layerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (rayHit.transform.gameObject.transform.parent.gameObject != selectedUnit.tile)
                {
                    selectedUnit.transform.position = Vector3.MoveTowards(transform.position, new Vector3(rayHit.transform.gameObject.transform.position.x, selectedUnit.transform.position.y, rayHit.transform.gameObject.transform.position.z), float.MaxValue);
                    Debug.Log("Move to : " + rayHit.transform.gameObject.transform.parent.name);

                    selectedUnit.tile.GetComponent<Tile_Behaviour>().currentUnit = null;
                    selectedUnit.tile = rayHit.transform.gameObject.transform.parent.gameObject;

                    if (rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit != null && rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit != selectedUnit)
                    {
                        Unit_Attacking(rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit);
                    }
                    else
                    {
                        rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit = selectedUnit.gameObject;

                        if (selectedUnit.tag == "Red Unit" && playing_Turn == Turn.redPlayer)
                        {
                            playing_Turn = Turn.bluePlayer;
                            selectedUnit.selected = false;
                            selectedUnit = null;
                        }
                        else if (selectedUnit.tag == "Blue Unit" && playing_Turn == Turn.bluePlayer)
                        {
                            playing_Turn = Turn.redPlayer;
                            selectedUnit.selected = false;
                            selectedUnit = null;
                        }
                    }
                }
            }
        }
    }

    private void Unit_Attacking(GameObject AttackedUnit)
    {
        Destroy(AttackedUnit);

        if(AttackedUnit.GetComponent<Unit_Behaviour>().piece == "King")
        {
            playing_Turn = Turn.Gameover;
        }
        else
        {
            if (selectedUnit.tag == "Red Unit")
            {
                playing_Turn = Turn.bluePlayer;
                selectedUnit.selected = false;
                selectedUnit = null;
            }
            else if (selectedUnit.tag == "Blue Unit")
            {
                playing_Turn = Turn.redPlayer;
                selectedUnit.selected = false;
                selectedUnit = null;
            }
        }
    }
}
