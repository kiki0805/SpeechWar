using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void BtnStartGame()
    {
        SceneManager.LoadScene("PlayingScene");
    }

    public void BtnStartOver()
    {
        PlayerPrefs.SetString("test", "HELLO");
        SceneManager.LoadScene("MainMenu");
    }
}
