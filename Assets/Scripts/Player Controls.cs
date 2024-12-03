using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
public class PlayerControls : MonoBehaviour
{
    public int health;
    //Public Tilemap variable for declaring the tilemap.
    public Tilemap tilemap;
    //Vector3Int foe the currenttile.
    public Vector3Int currentTile;
    //Float Move speed of the player.
    public float moveSpeed = 1.0f;
    //Public game tile for the player.
    public GameObject PlayerTile;
    //Public bools used for checking if the player of enemy has moved.
    public bool IsplayersTurn = true;
    public bool HasMoved = false;
    //Public variable for calling from the enemyscript.
    public EnemyScript enemyscript;
    //Private Vectors for the target position and new tile positions.
    private Vector3Int newTile;
    private Vector3 target;

    

    void Start()
    {
        enemyscript = GameObject.FindWithTag("Enemy").GetComponent<EnemyScript>();
        currentTile = tilemap.WorldToCell(transform.position);
        target = transform.position;

    }

    
    void Update()
    {
        //Calling the input method.
        HandleInput();
        //If statement for if the tile you move to is walkable.
        if (IsTileWalkable(newTile))
        {
            //Calling move player method.
            MovePlayer();
            
        }
        //If statement for the player moving.
        if (HasMoved)
        {
            //Calling this method to start the enemy's turn.
            StartEnemyTurn();
        }
            
    }

    //Method for moving the player on the map.
    void HandleInput()
    {
        
        //Keycodes for the four different WASD directions.
        if(Input.GetKeyDown(KeyCode.W))
        {
            newTile = currentTile + new Vector3Int(0, 1, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            newTile = currentTile + new Vector3Int(0, -1, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            newTile = currentTile + new Vector3Int(1, 0, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            newTile = currentTile + new Vector3Int(-1, 0, 0);
            HasMoved = true;
        }
        else 
        { 
            newTile = currentTile;
        }

        target = tilemap.CellToWorld(newTile);

        
    }

    //Method for moving the player if tile is walkable.
    void MovePlayer()
    {
        //If statement that allows the player
        if (IsTileWalkable(newTile) && IsplayersTurn)
        {
            transform.position = newTile;

            currentTile = newTile; 
        
        }
 
    }
    
    //Method for enemy starting their turn.
    void StartEnemyTurn()
    {
        IsplayersTurn = false;

        enemyscript.HasMoved = false;
    }

    //Method for calling the attack from the player.
    void PlayerAttack(Vector3Int tilePosition)
    {
        
    }

    //Bool that returns if a tile can be walked on or not with a true or false.
    bool IsTileWalkable(Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);

        
        if(tile != null)
        {
            //Combat check for player to attack an enemy.
            
            //tile. before tilePosition.
            //if (tilePosition == enemyPosition)
            //{
            //    PlayerAttack(tilePosition);
            //    return false;
            //}

            //
            string tileName = tile.name;
           //If statement for tiles that I don't want the player to walk on.
            if(tileName == "Wall Tile" || tileName == "Chest Tile" || tileName == "DoorTile" || tileName == "Enemy Tile")
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        else
        {
            return false;
        }
    
    
    
    }


}
