using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenuScript : MonoBehaviour
{
    public Text winnerText;
    private string winner;

    /* Show winner taken from player prefs */
    void Start()
    {
        winner = PlayerPrefs.GetString("winner");
        winnerText.text = winner;
    }
}
