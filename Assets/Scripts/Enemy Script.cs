using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    
    //Tilemap variable for enemy,
    public Tilemap tilemap;
    //Game objects for the enemy tile and the win text after defeating the enemy.
    public GameObject Enemy;
    public GameObject WinTextObject;
    //TextMeshProUGUI for displaying Enemy health and health statuses.
    public TextMeshProUGUI textmeshpro;
    //Variable for setting enemy health to 100.
    public int Health = 100;
    //Public string for the health status of the enemy depending on how much health it has.
    public string healthStatus;
    //Variable for the Player Tile.
    public Transform PlayerTile;
    //Damage amount Enemy can do to the player.
    private int damage = 20;


    //Public bool for checking if the Enemy has moved.
    public bool HasMoved = true;
    
    // Start is called before the first frame update
    void Start()
    {
        textmeshpro = GetComponent<TextMeshProUGUI>();

        if (textmeshpro != null)
        {
            textmeshpro.text = ShowHUD();
        }
        else
        {
            Debug.LogError("Component not found");
        }

    }

    public string ShowHUD()
    {
        string healthStatus = HealthStatus(health);
        if (textmeshpro != null)
        {
            textmeshpro.text = $"Health: {Health} " + $"Health Status: {healthStatus}";
        }
        else
        {
            Debug.LogError("Component not found");
        }


        return $"Health: {Health} " + $"Health Status: {healthStatus}";

    }

    //Method for player taking damage.
    public void TakeDamage()
    {

        if (damage < 0)
        {
            Debug.LogWarning("Damage can't be negative");
            return;
        }

        if (Health <= 0)
        {
            Debug.LogWarning("Player is dead, no more damage can be taken");
            return;
        }

        //If statement for player health reaching zero.
        if (Health == 0)
        {
            Enemy.SetActive(false);
            WinTextObject.SetActive(true);
        }

        //Changes health to health minus damage taken.
        health -= damage;


    }

    //Method for the health status that lets the player know how badly they are injured depending on the range of their health.
    public string HealthStatus(int hp)
    {
        //If statements that return different health statuses depending on how high or low the player's health is.
        if (hp <= 100 && hp > 90)
        {
            return "Perfect Health";
        }
        else if (hp <= 90 && hp > 75)
        {
            return "Healthy";
        }
        else if (hp <= 75 && hp > 50)
        {
            return "Hurt";
        }
        else if (hp <= 50 && hp > 10)
        {
            return "Badly Hurt";
        }
        else if (hp <= 10 && hp >= 1)
        {
            return "Imminent Danger";
        }

        //Returns out of range incase the health goes outside the 0 - 100 range for whatever reason.
        return "Out of Range";

    }

    // Update is called once per frame
    void Update()
    {
        //If statement checking if the enemy has moved on the map.
        if (HasMoved)
        {
            return;
        }
        else
        {
           
            FollowPlayer();
        }
        


    }

    //Method for the Enemy to follow the player with.
    public void FollowPlayer()
    {
        if(HasMoved)
        {
            return;
        }
        else
        {
            //Gives the direction of the player with normalizing the transform position of the player.
            Vector3Int CellPosition = tilemap.WorldToCell(transform.position);
            //
            Vector3Int PlayerPosition = tilemap.WorldToCell(PlayerTile.position);
            //
            Vector3Int DirectionToPlayer = PlayerPosition - CellPosition;
            //
            Vector3Int NewPosition = CellPosition + new Vector3Int(-1, 0, 0);
            //

            transform.position = tilemap.CellToWorld(NewPosition);
        
            HasMoved = true;
            return;
        }
        
    }

    ////Method for Enemy to attack the player.
    //void EnemyAttack(Vector3Int tilePosition)
    //{

    //}
    
    //bool for checking if certain tiles are walkable for the Enemy.
    bool IsTileWalkable(Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);


        if (tile != null)
        {
            //Combat Check
            //if (tile.tileposition == playerposition)
            //{
            //    EnemyAttack(tilePosition);
            //    return false;
            //}

            //A string for making tileName equal the name of the tiles.
            string tileName = tile.name;
            //If statement for tiles that I don't want the player to walk on.
            if (tileName == "Wall Tile" || tileName == "Chest Tile" || tileName == "DoorTile" || tileName == "PlayerTile")
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
