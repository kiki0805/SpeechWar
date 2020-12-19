using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void Player1ToggleChanged(bool newValue)
    {
        PlayerPrefs.SetInt("player1", newValue ? 1 : 0);
        Debug.Log("Player 1 speech: " + newValue);
    }

    public void Player2ToggleChanged(bool newValue)
    {
        PlayerPrefs.SetInt("player2", newValue ? 1 : 0);
        Debug.Log("Player 1 speech: " + newValue);
    }
}
