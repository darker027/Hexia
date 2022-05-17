using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMechanic_Controller : MonoBehaviour
{
    [Header("Board")]
    public GameObject board;

    public ResourceTile_Behaviour[] resourceTiles_OnBoard;

    // - - - - - Turn Base System - - - - -
    public enum Turn {redPlayer, bluePlayer, endTurn, Gameover}
    public Turn playing_Turn;
    public Turn previous_Turn;

    private GameObject[] redUnits;
    private GameObject[] blueUnits;
    [SerializeField] private GameObject[] advanceRedPieces;
    [SerializeField] private GameObject[] advanceBluePieces;

    [Header("Player turn")]
    public Unit_Behaviour selectedUnit;

    private bool enableRed;
    private bool disableRed;
    private bool enableBlue;
    private bool disableBlue;

    public int redPoint;
    public int bluePoint;

    public bool completedPoint;

    private bool playUI;
    private bool moveUnit;
    private bool upgradeUnit;

    [Header("Unit Movement")]
    [SerializeField] private LayerMask raycastMask;

    public enum Phase { Selecting, Playing }
    public Phase playing_Phase;

    [Header("Unit Indicator")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private LayerMask movementMask;

    private List<GameObject> walkableTiles = new List<GameObject>();
    private List<GameObject> indicatorTiles = new List<GameObject>();

    [HideInInspector] public bool moveClear;
    private bool moveShowing;

    private Button[] unitButton;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI victoryText;
    [SerializeField] private TMPro.TextMeshProUGUI bluePointText;
    [SerializeField] private TMPro.TextMeshProUGUI redPointText;
    [SerializeField] private Image turnColor;
    [SerializeField] private GameObject upgradeCanvas;


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
        if(playing_Turn == Turn.redPlayer)
        {
            previous_Turn = playing_Turn;
            turnColor.color = Color.red;

            if(!enableRed)
            {
                redUnits = GameObject.FindGameObjectsWithTag("Red Unit");
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
                blueUnits = GameObject.FindGameObjectsWithTag("Blue Unit");
                foreach (GameObject units in blueUnits)
                {
                    if (units != null)
                        units.layer = 2;
                }
                disableBlue = true;
                disableRed = false;
            }

            victoryText.text = "Red Victory";

            if (playing_Phase == Phase.Playing && selectedUnit != null)
            {
                if(selectedUnit.piece == "Pawn")
                {
                    if (!playUI)
                    {
                        unitButton = selectedUnit.gameObject.GetComponentsInChildren<Button>(includeInactive: gameObject);

                        foreach (Button buttons in unitButton)
                        {
                            buttons.gameObject.SetActive(true);
                        }

                        playUI = true;
                    }
                }
                else
                {
                    moveUnit = true;
                }

                if (moveUnit && !upgradeUnit)
                {
                    if (!moveShowing)
                    {
                        Unit_MoveShowing(selectedUnit.moveRange);
                        moveShowing = true;
                    }
                    else
                    {
                        if (moveClear)
                        {
                            if (indicatorTiles != null && indicatorTiles.Count != 0)
                            {
                                foreach (GameObject movetile in indicatorTiles)
                                {
                                    Destroy(movetile);
                                }
                            }
                            walkableTiles.Clear();
                            moveClear = false;
                            indicatorTiles.Clear();
                            moveShowing = false;
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && !moveClear)
                    {
                        Unit_Moving();
                    }
                }

                if(upgradeUnit && !moveUnit)
                {
                    upgradeCanvas.SetActive(true);
                }
                
            }
            else
            {
                if (indicatorTiles != null && indicatorTiles.Count != 0)
                {
                    foreach (GameObject movetile in indicatorTiles)
                    {
                        Destroy(movetile);
                    }
                    walkableTiles.Clear();
                    indicatorTiles.Clear();
                    moveShowing = false;
                }

                if(unitButton != null)
                {
                    foreach (Button buttons in unitButton)
                    {
                        if(buttons != null)
                        {
                            buttons.gameObject.SetActive(false);
                        }
                    }
                    playUI = false;
                }
                moveUnit = false;
            }
        }
        else if(playing_Turn == Turn.bluePlayer)
        {
            previous_Turn = playing_Turn;
            turnColor.color = Color.blue;

            if (!enableBlue)
            {
                blueUnits = GameObject.FindGameObjectsWithTag("Blue Unit");
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
                redUnits = GameObject.FindGameObjectsWithTag("Red Unit");
                foreach (GameObject units in redUnits)
                {
                    if(units != null)
                    units.layer = 2;
                }
                disableRed = true;
                disableBlue = false;
            }

            victoryText.text = "Blue Victory";

            if (playing_Phase == Phase.Playing && selectedUnit != null)
            {
                if (selectedUnit.piece == "Pawn")
                {
                    if (!playUI)
                    {
                        unitButton = selectedUnit.gameObject.GetComponentsInChildren<Button>(includeInactive: gameObject);

                        foreach (Button buttons in unitButton)
                        {
                            buttons.gameObject.SetActive(true);
                        }

                        playUI = true;
                    }
                }
                else
                {
                    moveUnit = true;
                }

                if (moveUnit && !upgradeUnit)
                {
                    if (!moveShowing)
                    {
                        Unit_MoveShowing(selectedUnit.moveRange);
                        moveShowing = true;
                    }
                    else
                    {
                        if (moveClear)
                        {
                            if (indicatorTiles != null && indicatorTiles.Count != 0)
                            {
                                foreach (GameObject movetile in indicatorTiles)
                                {
                                    Destroy(movetile);
                                }
                            }
                            walkableTiles.Clear();
                            moveClear = false;
                            indicatorTiles.Clear();
                            moveShowing = false;
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && !moveClear)
                    {
                        Unit_Moving();
                    }
                }

                if (upgradeUnit && !moveUnit)
                {
                    upgradeCanvas.SetActive(true);
                }
            }
            else
            {
                if (indicatorTiles != null && indicatorTiles.Count != 0)
                {
                    foreach (GameObject movetile in indicatorTiles)
                    {
                        Destroy(movetile);
                    }
                    walkableTiles.Clear();
                    indicatorTiles.Clear();
                    moveShowing = false;
                }

                if (unitButton != null)
                {
                    foreach (Button buttons in unitButton)
                    {
                        if(buttons != null)
                        {
                            buttons.gameObject.SetActive(false);
                        }
                    }
                    playUI = false;
                }
                moveUnit = false;
            }
        }
        else if(playing_Turn == Turn.endTurn)
        {
            foreach(ResourceTile_Behaviour resourceTile in resourceTiles_OnBoard)
            {
                if(resourceTile.earnPoint)
                {
                    completedPoint = true;
                }
                else
                {
                    completedPoint = false;
                    break;
                }
            }

            if(completedPoint)
            {
                if(previous_Turn == Turn.redPlayer)
                {
                    playing_Turn = Turn.bluePlayer;

                    foreach (ResourceTile_Behaviour resourceTile in resourceTiles_OnBoard)
                    {
                        resourceTile.earnPoint = false;
                    }
                }

                if(previous_Turn == Turn.bluePlayer)
                {
                    playing_Turn = Turn.redPlayer;

                    foreach (ResourceTile_Behaviour resourceTile in resourceTiles_OnBoard)
                    {
                        resourceTile.earnPoint = false;
                    }
                }
            }
        }
        else
        {
            victoryText.enabled = true;
        }

        // - - - Debugging - - -
        bluePointText.text = bluePoint.ToString();
        redPointText.text = redPoint.ToString();

        // - - - Cheat Code - - -
        if (Input.GetKeyDown(KeyCode.P))
        {
            redPoint++;
            bluePoint++;
        }
    }

    private void Unit_MoveShowing(int unitMoveRange)
    {
        for (int i = 0; i < unitMoveRange; i++)
        {
            Vector3 moveableTile = selectedUnit.transform.position + (CameraBase_Controller.MainCam_Instance.transform.forward * (i + 1));
;
            if (Physics.Raycast(moveableTile, Vector3.down, out RaycastHit tileHit, float.MaxValue, movementMask))
            {
                if (tileHit.transform.GetComponentInParent<Tile_Behaviour>().currentUnit == null)
                {
                    walkableTiles.Add(tileHit.transform.parent.gameObject);

                    GameObject instantiateIndicator = Instantiate(indicatorPrefab, new Vector3(tileHit.transform.parent.position.x, tileHit.transform.parent.position.y + 0.25f, tileHit.transform.parent.position.z), Quaternion.identity);
                    indicatorTiles.Add(instantiateIndicator);
                }
                else
                {
                    if(tileHit.transform.GetComponentInParent<Tile_Behaviour>().currentUnit.tag != selectedUnit.tag)
                    {
                        walkableTiles.Add(tileHit.transform.parent.gameObject);

                        GameObject instantiateIndicator = Instantiate(indicatorPrefab, new Vector3(tileHit.transform.parent.position.x, tileHit.transform.parent.position.y + 0.25f, tileHit.transform.parent.position.z), Quaternion.identity);
                        indicatorTiles.Add(instantiateIndicator);
                    }
                }
            }
        }
    }

    private void Unit_Moving()
    {
        Ray rayCast = CameraBase_Controller.MainCam_Instance.camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayCast, out RaycastHit rayHit, float.MaxValue, raycastMask))
        {
            if (walkableTiles.Contains(rayHit.transform.parent.gameObject) && rayHit.transform.parent.gameObject != selectedUnit.tile)
            {
                selectedUnit.transform.position = new Vector3(rayHit.transform.parent.position.x, selectedUnit.transform.position.y, rayHit.transform.parent.position.z);

                selectedUnit.tile.GetComponent<Tile_Behaviour>().currentUnit = null;
                selectedUnit.tile = rayHit.transform.parent.gameObject;

                if (rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit != null && rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit != selectedUnit)
                {
                    Unit_Attacking(rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit);
                }
                else
                {
                    rayHit.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit = selectedUnit.gameObject;

                    playing_Turn = Turn.endTurn;
                    playing_Phase = Phase.Selecting;
                    selectedUnit = null;
                }
            }
            else
            {
                moveClear = true;
                moveShowing = false;
                playing_Phase = Phase.Selecting;
                selectedUnit = null;
            }
        }
    }

    private void Unit_Attacking(GameObject AttackedUnit)
    {
        if(AttackedUnit.GetComponent<Unit_Behaviour>().piece == "King")
        {
            Destroy(AttackedUnit);
            playing_Turn = Turn.Gameover;
        }
        else
        {
            Destroy(AttackedUnit);
            playing_Turn = Turn.endTurn;
            playing_Phase = Phase.Selecting;
            selectedUnit = null;
        }
    }

    public void MoveButton()
    {
        moveUnit = true;

        foreach (Button buttons in unitButton)
        {
            buttons.gameObject.SetActive(false);
        }
    }

    public void UpgradeButton()
    {
        upgradeUnit = true;

        foreach (Button buttons in unitButton)
        {
            buttons.gameObject.SetActive(false);
        }
    }

    public void KnightPiece(int index)
    {
        if(playing_Turn == Turn.redPlayer)
        {
            if (redPoint >= 3)
            {
                Instantiate(advanceRedPieces[index], selectedUnit.transform.position, Quaternion.identity);
                Destroy(selectedUnit.gameObject);
                redPoint -= 3;

                playing_Turn = Turn.endTurn;
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
            else
            {
                upgradeCanvas.SetActive(false);
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
        }

        if (playing_Turn == Turn.bluePlayer)
        {
            if (bluePoint >= 3)
            {
                Instantiate(advanceBluePieces[index], selectedUnit.transform.position, Quaternion.identity);
                Destroy(selectedUnit.gameObject);
                bluePoint -= 3;

                playing_Turn = Turn.endTurn;
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
            else
            {
                upgradeCanvas.SetActive(false);
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
        }
    }

    public void RookPiece(int index)
    {
        if (playing_Turn == Turn.redPlayer)
        {
            if (redPoint >= 5)
            {
                Instantiate(advanceRedPieces[index], selectedUnit.transform.position, Quaternion.identity);
                Destroy(selectedUnit.gameObject);
                redPoint -= 5;

                playing_Turn = Turn.endTurn;
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
            else
            {
                upgradeCanvas.SetActive(false);
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
        }

        if (playing_Turn == Turn.bluePlayer)
        {
            if (bluePoint >= 5)
            {
                Instantiate(advanceBluePieces[index], selectedUnit.transform.position, Quaternion.identity);
                Destroy(selectedUnit.gameObject);
                bluePoint -= 5;

                playing_Turn = Turn.endTurn;
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
            else
            {
                upgradeCanvas.SetActive(false);
                playing_Phase = Phase.Selecting;
                upgradeUnit = false;
                selectedUnit = null;
            }
        }
    }
}
