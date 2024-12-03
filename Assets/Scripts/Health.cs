using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public GameObject Player;
    public GameObject LosetextObject;
    //Public textmeshpro for displaying the health, health status and lives.
    public TextMeshProUGUI textmeshpro;
    //Public variables for setting the values of the health and lives, which is declared on the variable.
    public int health = 100;
    public int lives = 3;
    //Private integer for how much damage the player can take.
    private int damage = 20;
    //Public string for writing the health status of the player.
    public string healthStatus;



    public void Start()
    {
        
        textmeshpro = GetComponent<TextMeshProUGUI>();

        if(textmeshpro != null)
        {
            textmeshpro.text = ShowHUD();
        }
        else
        {
            Debug.LogError("Component not found");
        }


    }

    //Method that prints the health, health status and number of lives the player has into the game.
    public string ShowHUD()
    {
        string healthStatus = HealthStatus(health);
        if(textmeshpro != null)
        {
            textmeshpro.text = $"Health: {health} " + $"Lives: {lives} " + $"Health Status: {healthStatus}";
        }
        else 
        {
            Debug.LogError("Component not found");
        }
        
        
        return $"Health: {health} " + $"Lives: {lives} " + $"Health Status: {healthStatus}";

    }

    //Method for player taking damage.
    public void TakeDamage()
    {
        
        if(damage < 0)
        {
            Debug.LogWarning("Damage can't be negative");
            return;
        }

        if (health <= 0)
        {
            Debug.LogWarning("Player is dead, no more damage can be taken");
            return;
        }

        //If statement for player health reaching zero.
        if (health == 0)
        {
            //Calls revive method
            Revive();
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

    //Method for reviving the player after they lose a life, resets health to 100.
    public void Revive()
    {
        //Takes away one life after death and resets the health to 100.
        lives = lives - 1;
        health = 100;

        
        if(lives == 0)
        {
            Player.SetActive(false);
            LosetextObject.SetActive(true);
        }
    }

   


}
