using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Behaviour : MonoBehaviour
{
    [SerializeField] private GameMechanic_Controller Main_Controller;
    [SerializeField] private LayerMask TileMask;

    [Header("Unit Stat")]
    /*[HideInInspector] */ public GameObject tile;

    public string piece;

    public int moveRange;



    // Start is called before the first frame update
    void Start()
    {
        Main_Controller = GameObject.Find("Game Controller").GetComponent<GameMechanic_Controller>();
        TileMask = LayerMask.GetMask("Tiles");
    }

    // Update is called once per frame
    void Update()
    {
        if(tile == null)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit tileHit, 5.0f, TileMask))
            {
                tile = tileHit.transform.gameObject.transform.parent.gameObject;
                tile.transform.gameObject.GetComponentInParent<Tile_Behaviour>().currentUnit = this.gameObject;
            }
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(GamePhase());
    }

    private IEnumerator GamePhase()
    {
        if (Main_Controller.playing_Phase == GameMechanic_Controller.Phase.Selecting && Main_Controller.selectedUnit == null)
        {
            Main_Controller.selectedUnit = this;
            yield return new WaitForSeconds(0.25f);
            Main_Controller.playing_Phase = GameMechanic_Controller.Phase.Moving;
        }
        else
        {
            if (Main_Controller.playing_Phase == GameMechanic_Controller.Phase.Moving && Main_Controller.selectedUnit != this)
            {
                Main_Controller.moveClear = true;
                yield return new WaitForSeconds(0.25f);
                Main_Controller.selectedUnit = this;
                Main_Controller.playing_Phase = GameMechanic_Controller.Phase.Moving;
            }
            else
            {
                Main_Controller.selectedUnit = null;
                yield return new WaitForSeconds(0.25f);
                Main_Controller.playing_Phase = GameMechanic_Controller.Phase.Selecting;
            }
        }
    }
}
