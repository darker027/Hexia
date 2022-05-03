using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile_Behaviour : Tile_Behaviour
{
    [SerializeField] private GameMechanic_Controller Main_Controller;

    private GameMechanic_Controller.Turn previousTurn;

    public bool earnPoint;

    private int turnCount;

    private bool redControl;
    private bool blueControl;

    // Start is called before the first frame update
    void Start()
    {
        Main_Controller = GameObject.Find("Game Controller").GetComponent<GameMechanic_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Main_Controller.playing_Turn == GameMechanic_Controller.Turn.endTurn)
        {
            Unit_Checking();
        }
    }

    void Unit_Checking()
    {
        if(currentUnit != null && !earnPoint)
        {
            if(redControl && currentUnit.CompareTag("Red Unit"))
            {
                if(turnCount > 0)
                {
                    Main_Controller.redPoint++;
                    turnCount++;
                    earnPoint = true;
                    return;
                }
                else
                {
                    turnCount++;
                    earnPoint = true;
                    return;
                }
            }
            else if(!redControl && currentUnit.CompareTag("Red Unit"))
            {
                if(blueControl)
                {
                    blueControl = false;
                }

                turnCount = 0;
                redControl = true;
                return;
            }

            if(blueControl && currentUnit.CompareTag("Blue Unit"))
            {
                if (turnCount > 0)
                {
                    Main_Controller.bluePoint++;
                    turnCount++;
                    earnPoint = true;
                    return;
                }
                else
                {
                    turnCount++;
                    earnPoint = true;
                    return;
                }
            }
            else if(!blueControl && currentUnit.CompareTag("Blue Unit"))
            {
                if (redControl)
                {
                    redControl = false;
                }

                turnCount = 0;
                blueControl = true;
                return;
            }
        }
        else
        {
            earnPoint = true;
            redControl = false;
            blueControl = false;
            turnCount = 0;
            return;
        }
    }
}
