using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenuScript : MonoBehaviour
{
    public Text winnerText;
    private string winner;

    // Start is called before the first frame update
    void Start()
    {
        winner = PlayerPrefs.GetString("winner");
        winnerText.text = winner;
    }
}
