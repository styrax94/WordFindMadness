using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField]
    GameLoop gameLoop;
    public bool playerTurn= true;
    public Tile previousTile;
    List<Tile> currentlySelected = new List<Tile>();
    public bool paused = false;
    // Update is called once per frame
    void Update()
    {
        if (!playerTurn || paused)
        {
            return;
        }
       
        if (Input.GetMouseButton(0))
        {
            DrawRay();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gameLoop.CheckSelected();
            currentlySelected.Clear();
        }
    }

    void DrawRay()
    {
        //Cast a ray from screen to game world.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider.tag == "Tile")
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                if (!tile.selected)
                {
                    if (gameLoop.AddToSelection(tile))
                    {
                        previousTile = tile;
                        currentlySelected.Add(tile);
                    }
                }
                //Quality of life addition, able to go back from a selected option
                else if(tile.selected && previousTile!= tile && !tile.tileFound)
                {
                    if (currentlySelected[currentlySelected.Count-2] == tile)
                    {
                        gameLoop.RemoveFromSelection(previousTile);
                        currentlySelected.Remove(previousTile);
                        previousTile = tile;
                        
                    }
                    
                }
            }
        }           
    }
}
