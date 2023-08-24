using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public HealthBar player1Hp;
    public HealthBar player2Hp;

    [Header("SpawnPoint")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    [Header("playerController")]
    public PlayerController player1;
    public PlayerController player2;

    [Header("CdUI")]
    public GameObject player1UI;
    public GameObject player2UI;

    public float warmUpTime = 15;

    [Header("Rounds")]
    public float maxRoundTime = 150;
    public float currentRoundTime;
    public Text countdownTxt;

    private bool intermission = false;

    public bool warmUpBeign;
    public bool gameBegin;

    public bool startCheck = false;

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

        



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(3);
        }



        if (!warmUpBeign && startCheck== false)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            warmUpBeign = (players.Length != 2) ? false : true;
            

            foreach(PlayerController player in players)
            {
                if(player.currentPlayer.ToString() == "P2")
                {
                    player2 = player;
                }
            }
        }


        if (warmUpBeign && !gameBegin)
        {
            warmUpTime -= 1 * Time.deltaTime;
            countdownTxt.text = warmUpTime.ToString("00");
            startCheck = true;
        }

        if(warmUpTime <= 0 && !intermission)
        {
            warmUpBeign = false;
            Debug.Log(warmUpBeign);
            StartCoroutine(BeginBrawl());
        }

        if (!gameBegin)
        {
            player1.playerCurrentHealth = player1.playerMaxHealth;
            player1.playerCurrentHealth = player1.playerMaxHealth;
        }
        else
        {
            StartGame();
        }

        if (!gameBegin) return;

        if (player1.playerCurrentHealth <= 0)
        {
            CalculateWinner();
        }
        if (player2.playerCurrentHealth <= 0)
        {
            CalculateWinner();
        }
        if(currentRoundTime <= 0)
        {
            CalculateWinner();
        }
    }

    
	IEnumerator BeginBrawl()
    {
        intermission = true;

        string textBrawl = "Prepare to brawl";
        countdownTxt.text = textBrawl;
        yield return new WaitForSeconds(2f);

        int countdownFrom = 3;
        for(int i = 0; i<4; i++)
        {
            countdownTxt.text = countdownFrom.ToString();
            yield return new WaitForSeconds(1f);
            countdownFrom--;
        }
        gameBegin = true;
    }

	
    public void StartGame()
    {
        if (currentRoundTime > 0 && !gameOver)
        {
            currentRoundTime -= 1 * Time.deltaTime;
            countdownTxt.text = currentRoundTime.ToString("00");
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



    public void moveScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
