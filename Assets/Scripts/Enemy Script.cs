using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class EnemyScript : MonoBehaviour
{
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
    
    bool inCombat = false;
    bool turnIsOver = false;


    public Vector3Int enemyPos;
    bool moving;

    //Public bool for checking if the Enemy has moved.
    public bool HasMoved = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerTile);

        healthText.SetText(playerControls.health.ToString());

    }

    public string ShowHUD()
    {
        string healthStatus = HealthStatus(playerControls.health);
        if (healthText != null)
        {
            healthText.SetText = $"Health: {playerControls.health} " + $"Health Status: {healthStatus}";
        }
        else
        {
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
            Enemy.SetActive(false);
            WinTextObject.SetActive(true);
        }

        //Changes health to health minus damage taken.
        Health -= damage;


    }

    void HandleTurns()
    {
        bool playerTurn = false;

        if(!turnIsOver)
        {
            if(!playerTurn)
            {
                playerControls.health -= damage;
                Debug.Log("Enemy Attacks you! Player Health: " + playerControls.health);

                TakeDamage();

                playerTurn = true;
            }
            else 
            { 
                if(Input.anyKeyDown)
                {
                    Debug.Log("You Attack!");

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

    // Update is called once per frame
    void Update()
    {
        //If statement checking if the enemy has moved on the map.
        if (!inCombat && !turnIsOver)
        {
            StartCoroutine(FollowPlayer());
        }
        else if(inCombat && !turnIsOver)
        {

            HandleTurns();            
        }


        ShowHUD();
    }

    //Method for the Enemy to follow the player with.
    public IEnumerator FollowPlayer()
    {
        if (moving) yield break;

        moving = true;


        Debug.Log($"New Position: {CheckDirectionToMove()}");
        
        yield return new WaitForSeconds(.6f);

        if (IsTileWalkable(CheckDirectionToMove()))
        {
            transform.position = CheckDirectionToMove();
        }
        
        moving = false;
    }

    ////Method for Enemy to attack the player.
    //void EnemyAttack(Vector3Int tilePosition)
    //{

    //}


    Vector3Int CheckDirectionToMove()
    {
        Vector3Int direction = new();

        Vector3Int CellPosition = tilemap.WorldToCell(transform.position);

        Vector3Int PlayerPosition = tilemap.WorldToCell(PlayerTile.position);

        Vector3Int DifferenceFromEnemyToPlayer = (PlayerPosition - CellPosition);

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

        Vector3Int NewPosition = CellPosition - direction;
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

            if (tilePosition == newPlayerPosition)
            {
                Debug.Log($"Collided with the Player");

                inCombat = true;

                return false;
            } 
            
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
