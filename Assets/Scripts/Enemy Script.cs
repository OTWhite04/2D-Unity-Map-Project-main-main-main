using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    //Variable for the player controls script.
    public PlayerControls playerControls;
    //Tilemap variable for enemy.
    public Tilemap tilemap;
    //Game objects for the enemy tile and the win text after defeating the enemy.
    public GameObject Enemy;
    public GameObject WinTextObject;
    //TextMeshProUGUI for displaying Enemy health and health statuses.
    public TextMeshProUGUI healthText;
    //Variable for setting enemy health to 100.
    public int Health = 100;
    //Public string for the health status of the enemy depending on how much health it has.
    public string healthStatus;
    //Variable for the Player Tile.
    public Transform PlayerTile;
    //Damage amount Enemy can do to the player.
    private int damage = 20;
    //Bools for the enemy being in combat and telling if the turn is over and moving.
    bool inCombat = false;
    bool turnIsOver = false;
    //Bool for moving.
    bool moving;
    //Vector3Int for the enemy position.
    public Vector3Int enemyPos;
   

    //Public bool for checking if the Enemy has moved.
    public bool HasMoved = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerTile);

        healthText.SetText(playerControls.health.ToString());

    }


    // Update is called once per frame
    void Update()
    {
        //If statement checking if the enemy has moved on the map.
        if (!inCombat && !turnIsOver)
        {
            StartCoroutine(FollowPlayer());
        }
        else if (inCombat && !turnIsOver)
        {

            HandleTurns();
        }

        //Updates hud for damage done to enemy.
        ShowHUD();
    }

    //Public string for displaying the enemy's health and health status.
    public string ShowHUD()
    {
        //Calls the health status method.
        string healthStatus = HealthStatus(playerControls.health);
        //If statement for checking if health text isn't null.
        if (healthText != null)
        {
            //Sets the health and health text to the text mesh pro in the scene.
            healthText.SetText($"Health: {playerControls.health} " + $"Health Status: {healthStatus}");
        }
        else
        {
            //Displays error for the health text component not being found.
            Debug.LogError("Component not found");
        }


        return $"Health: {playerControls.health} " + $"Health Status: {healthStatus}";

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
            //Sets enemy to inactive and displays the text for winning.
            Enemy.SetActive(false);
            WinTextObject.SetActive(true);
        }

        //Changes health to health minus damage taken.
        Health -= damage;


    }

    //Method for handling the turns taken.
    void HandleTurns()
    {
        //Sets player turn to false.
        bool playerTurn = false;


        if(!turnIsOver)
        {
            if(!playerTurn)
            {
                playerControls.health -= damage;
                Debug.Log("Enemy Attacks you! Player Health: " + playerControls.health);
                //Calls the take damage method.
                TakeDamage();
                //Sets player turn to true sp the player can move.
                playerTurn = true;
            }
            else 
            { 

                if(Input.anyKeyDown)
                {
                    
                    Debug.Log("You Attack!");
                    //Makes it so the player can't move.
                    playerTurn = false;
                }
            
            }
        }
        else 
        {

            turnIsOver = false;
        }
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


    //Method for the Enemy to follow the player with.
    public IEnumerator FollowPlayer()
    {
        if (moving) yield break;
        //Sets moving to true.
        moving = true;


        Debug.Log($"New Position: {CheckDirectionToMove()}");
        
        yield return new WaitForSeconds(.6f);

        if (IsTileWalkable(CheckDirectionToMove()))
        {
            transform.position = CheckDirectionToMove();
            Debug.Log(moving);
        }
        
        moving = false;
    }

    //Method for Enemy to attack the player.
    void EnemyAttack(Vector3Int tilePosition)
    {

    }

    //Vector3Int method for checking the direction for the enemy to move.
    Vector3Int CheckDirectionToMove()
    {
        Vector3Int direction = new();

        Vector3Int CellPosition = tilemap.WorldToCell(transform.position);

        Vector3Int PlayerPosition = tilemap.WorldToCell(PlayerTile.position);

        Vector3Int DifferenceFromEnemyToPlayer = (PlayerPosition - CellPosition);

        //If and else if statements for moving the enemy in the direction of the player depending on where they are on the tilemap.
        if(DifferenceFromEnemyToPlayer.x > 0)
        {
            if(Mathf.Abs(DifferenceFromEnemyToPlayer.y) <= DifferenceFromEnemyToPlayer.x)
            {
                direction = Vector3Int.left;
            }
        }
        else if(DifferenceFromEnemyToPlayer.x < 0)
        {
            if(Mathf.Abs(DifferenceFromEnemyToPlayer.y) <= Mathf.Abs(DifferenceFromEnemyToPlayer.x))
            {
                direction = Vector3Int.right;
            }
        }
        if(DifferenceFromEnemyToPlayer.y > 0)
        {
            if(Mathf.Abs(DifferenceFromEnemyToPlayer.x) <= DifferenceFromEnemyToPlayer.y)
            {
                direction = Vector3Int.down;
            }
        }
        else if(DifferenceFromEnemyToPlayer.y < 0)
        {
            if(Mathf.Abs(DifferenceFromEnemyToPlayer.x) <= Mathf.Abs(DifferenceFromEnemyToPlayer.y))
            {
                direction = Vector3Int.up;
            }
        }
        //Getting the new position by minusing the cell position and direction.
        Vector3Int NewPosition = CellPosition - direction;
        //Returning the new position of the enemy.
        return NewPosition;

    }
    
    //bool for checking if certain tiles are walkable for the Enemy.
    bool IsTileWalkable(Vector3Int tilePosition)
    {

        TileBase tile = tilemap.GetTile(tilePosition);

        Vector3Int newPlayerPosition = tilemap.WorldToCell(new Vector3(PlayerTile.position.x, PlayerTile.position.y, PlayerTile.position.z));
        
        if (tile != null)
        {
            //Combat Check
            //if (tile.tileposition == playerposition)
            //{
            //    EnemyAttack(tilePosition);
            //    return false;
            //}
            //If statement for seeing if the tile position is the player position.
            if (tilePosition == newPlayerPosition)
            {
                Debug.Log($"Collided with the Player");

                inCombat = true;

                return false;
            } 
            //Checking tile names so the enemy doesn't go through doors, chests or walls.
            else if (tile.name == "Wall Tile" || tile.name == "Chest Tile" || tile.name == "DoorTile" || tile.name == "PlayerTile")
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        return false;
    }
}
