using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;

public class VoiceMovement : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        // Keywords for up
        actions.Add("up", Up);
        actions.Add("app", Up);
        actions.Add("upp", Up);
        actions.Add("north", Up);

        // Keywords for right
        actions.Add("write", Right);
        actions.Add("rite", Right);
        actions.Add("east", Right);
        actions.Add("eest", Right);

        // Keyowrds for down
        actions.Add("down", Down);
        actions.Add("dawn", Down);
        actions.Add("don", Down);
        actions.Add("south", Down);

        // Keywords for left
        actions.Add("lift", Left);
        actions.Add("lepht", Left);
        actions.Add("left", Left);
        actions.Add("west", Left);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());  // Keys need to be in an array
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();  
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
    }

    private void Right()
    {
        transform.Translate(1, 0, 0);
    }

    private void Down()
    {
        transform.Translate(0, -1, 0);
    }

    private void Left()
    {
        transform.Translate(-1, 0, 0);
    }
}
