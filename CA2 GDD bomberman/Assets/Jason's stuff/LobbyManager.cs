using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public int minPlayer = 2;
    public int currentPlayer = 0;

    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController[] maxPlayer = FindObjectsOfType<PlayerController>();
        Debug.Log(maxPlayer.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            currentPlayer++;
            players.Add(other.gameObject);
            CheckBeginGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            currentPlayer--;
            players.Remove(other.gameObject);
        }
    }

    public void CheckBeginGame()
    {
        if(currentPlayer == minPlayer)
        {
            StartCoroutine(InitiateCountdownCo());           
        }
    }
    
    IEnumerator InitiateCountdownCo()
    {
        Debug.Log("started countdown");
        yield return new WaitForSeconds(5f);
        foreach (GameObject player in players)
        {
            DontDestroyOnLoad(player);
            SceneManager.LoadScene("Level1");
        }
    }
}
