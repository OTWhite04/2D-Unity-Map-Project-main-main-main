using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControls : MonoBehaviour
{
    public int health;
    //Public Tilemap variable for declaring the tilemap.
    public Tilemap tilemap;
    //Vector3Int foe the currenttile.
    public Vector3Int currentTile;
    //Float Move speed of the player.
    public float moveSpeed = 1.0f;
    public int attack = 20;
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

    
    //Start method of the controls script.
    void Start()
    {
        //Finds the enemy with a tag and gets the enemy script component.
        enemyscript = GameObject.FindWithTag("Enemy").GetComponent<EnemyScript>();
        currentTile = tilemap.WorldToCell(transform.position);
       //makes transform.position equal the target position.
        target = transform.position;

    }

    //Update method.
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
        if(Input.GetKeyDown(KeyCode.W) && !HasMoved)
        {
            newTile = currentTile + new Vector3Int(0, 1, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.S) && !HasMoved)
        {
            newTile = currentTile + new Vector3Int(0, -1, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.D) && !HasMoved)
        {
            newTile = currentTile + new Vector3Int(1, 0, 0);
            HasMoved = true;
        }
        else if(Input.GetKeyDown(KeyCode.A) && !HasMoved)
        {
            newTile = currentTile + new Vector3Int(-1, 0, 0);
            HasMoved = true;
        }
        else 
        { 
            newTile = currentTile;
        }

        target = tilemap.CellToWorld(newTile);
        HasMoved = false;
        
    }

    //Method for moving the player if tile is walkable.
    void MovePlayer()
    {
        //If statement checking if the tile is walkable and is the player's turn.
        if (IsTileWalkable(newTile) && IsplayersTurn)
        {
            //Transforming position being equal to the new tile.
            transform.position = newTile;
            //Making current tile equal the new tile.
            currentTile = newTile; 
        
        }
 
    }
    
    //Method for enemy starting their turn.
    void StartEnemyTurn()
    {
        //Sets the player turn to false so they can't move.
        IsplayersTurn = false;
        //sets HasMoved in enemy script to false.
        enemyscript.HasMoved = false;
    }

    //Method for calling the attack from the player.
    void PlayerAttack(Vector3Int tilePosition)
    {
        enemyscript.Health -= attack; 
    }

    //Bool that returns if a tile can be walked on or not with a true or false.
    bool IsTileWalkable(Vector3Int tilePosition)
    {
        
        TileBase tile = tilemap.GetTile(tilePosition);

        
        if(tile != null)
        {
            //Combat check for player to attack an enemy.

            ////If statement for the tile position being equal to the enemy position.
            //if (tilePosition == enemyPosition)
            //{
            //    //Calls player attack method for tile position.
            //    PlayerAttack(tilePosition);
            //    return false;
            //}

            //turning tile.name into a string called tileName.
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
