using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;

    public float maxRoundTime = 150;
    public float currentRoundTime;
    public Text countdownTxt;

    public bool gameOver = false;
    public GameObject gameOverScreen;
    public Text winnerTxt;

    // Start is called before the first frame update
    void Start()
    {
        currentRoundTime = maxRoundTime;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRoundTime > 0 && !gameOver)
        {
            currentRoundTime -= 1 * Time.deltaTime;
            countdownTxt.text = currentRoundTime.ToString("00");
        }
        //else currentRoundTime = 0;
           

        if (player1.playerCurrentHealth <= 0)
        {
            CalculateWinner();
        }
        //if (player2.playerCurrentHealth <= 0)
        //{
        //    CalculateWinner();
        //}
        if(currentRoundTime <= 0)
        {
            CalculateWinner();
        }
    }

    public void CalculateWinner()
    {
        gameOver = true;
        gameOverScreen.gameObject.SetActive(true);
       

        if(player1.playerCurrentHealth > player2.playerCurrentHealth)
        {
            Player1Wins();
        }
        else if (player2.playerCurrentHealth > player1.playerCurrentHealth)
        {
            Player2Wins();
        }
        else if (player1.playerCurrentHealth == player2.playerCurrentHealth)
        {
            Draw();
        }
        


    }

    public void Player1Wins()
    {
        winnerTxt.text = ("Player 1 Wins!");
    }
    public void Player2Wins()
    {
        winnerTxt.text = ("Player 2 Wins!");
    }
    public void Draw()
    {

        winnerTxt.text = ("Draw!");
    }
}
