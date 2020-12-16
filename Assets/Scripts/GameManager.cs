using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    GameObject activePlayer;

    private const float TIME_PER_ROUND = 120;        // In seconds
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

        // Set up speech controller
        if (player1.GetComponent<PlayerManager>().speechInput)
        {
            speechController = GetComponentInChildren<SpeechController>();
            speechController.RefreshController();                               // Give speechController this player as controller
        }

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
    private void ChangePlayer()
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
}
