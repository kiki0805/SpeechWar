using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    /* Start game */
    public void BtnStartGame()
    {
        SceneManager.LoadScene("PlayingScene");
    }

    /* Show instructions */
    public void BtnShowInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    /* Start over (skip title scene) */
    public void BtnStartOver()
    {
        SceneManager.LoadScene("PlayingScene");
    }

    /* Back to main*/
    public void BtnBackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /* Quit game */
    public void BtnQuitGame()
    {
        Application.Quit();
    }
}
