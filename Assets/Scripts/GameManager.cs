using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    GameObject activePlayer;

    private const float TIME_PER_ROUND = 60;        // In seconds
    private float time = TIME_PER_ROUND;
    public Text timerDisplay;                       // Reference to timer text object in engine
    public Text playerDisplay;
    SpeechController speechController;
    Coroutine lastRoutine;

    /* Setup */
    void Start()
    {
        // Start with Player 1
        player1.GetComponent<PlayerManager>().SetActive();
        player2.GetComponent<PlayerManager>().SetInactive();
        activePlayer = player1;
        playerDisplay.text = "Turn: Player 1";

        // Set up speech controller(s)
        player1.GetComponent<PlayerManager>().SetSpeechInput(PlayerPrefs.GetInt("player1"));
        player2.GetComponent<PlayerManager>().SetSpeechInput(PlayerPrefs.GetInt("player2"));

        speechController = GetComponentInChildren<SpeechController>();
        speechController.RefreshController();

        // Start timer
        lastRoutine = StartCoroutine(CountdownRoundTimer());
    }

    /*  Method for getting the active player
        @return active player
    */
    public GameObject GetActivePlayer()
    {
        return activePlayer;
    }

    // Method for changing active player
    public void ChangePlayer()
    {
        if (activePlayer == player1)
        {
            player1.GetComponent<PlayerManager>().SetInactive();
            player2.GetComponent<PlayerManager>().SetActive();
            activePlayer = player2;
            playerDisplay.text = "Turn: Player 2";
        }
        else
        {
            player1.GetComponent<PlayerManager>().SetActive();
            player2.GetComponent<PlayerManager>().SetInactive();
            activePlayer = player1;
            playerDisplay.text = "Turn: Player 1";
        }

        if (activePlayer.GetComponent<PlayerManager>().speechInput)
        {
            speechController.RefreshController();
        }
        Debug.Log("Active player is now: " + activePlayer);
    }

    // Method for counting down and displaying it on screen
    IEnumerator CountdownRoundTimer()
    {
        while (time > 0)                            // While time left for round
        {
            timerDisplay.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        ChangePlayer();                             // Change player when round ends
        time = TIME_PER_ROUND;                      // Reset timer and start over
        lastRoutine = StartCoroutine(CountdownRoundTimer());
    }

    /* Method to end turn prematurely */
    public void EndTurn()
    {
        StopCoroutine(lastRoutine);
        timerDisplay.text = "Waiting";

        // Inactivate activePlayer
        activePlayer.GetComponent<PlayerManager>().SetInactive();
    }

    /* Method to start turn when bullet destroyed */
    public void StartTurn()
    {
        ChangePlayer();
        time = TIME_PER_ROUND;
        lastRoutine = StartCoroutine(CountdownRoundTimer());
    }

    public void EndGame()
    {
        // See who won
        int player1CharactersAlive = player1.GetComponent<PlayerManager>().GetCharactersAlive();
        if(player1CharactersAlive == 0)
        {
            PlayerPrefs.SetString("winner", "Player 2");
            Debug.Log("Winner is player 2!");
        }
        else
        {
            PlayerPrefs.SetString("winner", "Player 1");
            Debug.Log("Winner is player 1!");
        }
        SceneManager.LoadScene("EndMenu");
    }
}
