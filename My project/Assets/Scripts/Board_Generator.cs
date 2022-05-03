using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Generator : MonoBehaviour
{
    //--------------------------------------------
    [SerializeField] private GameMechanic_Controller Main_Controller;

    [SerializeField] private GameObject[] boardList;

    void Start()
    {
        Main_Controller = gameObject.GetComponent<GameMechanic_Controller>();

        if(Main_Controller.board == null)
        {
            Generate_Board();
        }
    }

    void Generate_Board()
    {
        int boardNumber = Random.Range(0, boardList.Length);

        Main_Controller.board = Instantiate(boardList[boardNumber], Vector3.zero, Quaternion.identity);
        Main_Controller.resourceTiles_OnBoard = FindObjectsOfType<ResourceTile_Behaviour>();
    }

    void Generate_Unit()
    {

    }
}
