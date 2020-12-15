using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechController : MonoBehaviour
{
    GameManager gameManager;

    PlayerManager controller;
    // Start is called before the first frame update
    public string[] keywords = new string[] { "up", "below", "left", "right", "stop" };
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    private KeywordRecognizer recognizer;
    private void Start()
    {
        recognizer = new KeywordRecognizer(keywords, confidence);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
        gameManager = GetComponentInParent<GameManager>();
    }

    public void RefreshController()
    {
        controller = gameManager.GetComponent<GameManager>().GetActivePlayer().GetComponent<PlayerManager>();
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (controller is null) return;
        if (!controller.speechInput) return;
        switch (args.text)
        {
            case MoveStatus.Still:
                controller.UpdateCharacterMoveStatus(MoveStatus.Still);
                break;
            case MoveStatus.Right:
                controller.UpdateCharacterMoveStatus(MoveStatus.Right);
                break;
            case MoveStatus.Left:
                controller.UpdateCharacterMoveStatus(MoveStatus.Left);
                break;
            case MoveStatus.Up:
                controller.UpdateCharacterMoveStatus(MoveStatus.Up);
                break;
            case MoveStatus.Down:
                controller.UpdateCharacterMoveStatus(MoveStatus.Down);
                break;
            default:
                break;
        }
        Debug.Log("Keyword result: " + args.text);
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}