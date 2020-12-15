using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    GameObject activePlayer;

    private const float TIME_PER_ROUND = 20;        // In seconds
    private float time = TIME_PER_ROUND;
    public Text timerDisplay;                       // Reference to timer text object in engine
    public Text playerDisplay;
    SpeechController speechController;

    // Start is called before the first frame update
    void Start()
    {
        // Start with Player 1
        player1.GetComponent<PlayerManager>().SetActive();
        player2.GetComponent<PlayerManager>().SetInactive();
        activePlayer = player1;

        speechController = GetComponentInChildren<SpeechController>();
        speechController.RefreshController();

        playerDisplay.text = "Turn: Player 1";

        // Start timer
        StartCoroutine(CountdownRoundTimer());
    }

    public GameObject GetActivePlayer()
    {
        return activePlayer;
    }

    // Method for changing active player
    private void ChangePlayer()
    {
        if(activePlayer == player1)
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
        speechController.RefreshController();
    }

    // Method for counting down and displaying it on screen
    IEnumerator CountdownRoundTimer()
    {
        while (time > 0)
        {
            timerDisplay.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        ChangePlayer();
        time = TIME_PER_ROUND;                      // Reset timer and start over
        StartCoroutine(CountdownRoundTimer());  
    }
}
