using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public static class Commands
{
    public const string TurnLeft = "back";
    public const string TurnRight = "turn";
    public const string Shoot = "shoot";
    public const string Switch = "switch";
}

public class SpeechController : MonoBehaviour
{
    GameManager gameManager;        // Reference this object
    PlayerManager controller;       // Get controller of active player

    // Start is called before the first frame update
    public string[] keywords = new string[] { "up", "below", "left", "right", "stop", "switch",
        "turn", "back",
        "shoot"};
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    private KeywordRecognizer recognizer;

    /* Setup */
    private void Start()
    {
        recognizer = new KeywordRecognizer(keywords, confidence);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
        gameManager = GetComponentInParent<GameManager>();
    }

    /* Get PlayerManager-script of active player*/
    public void RefreshController()
    {
        if (gameManager is null)
        {
            gameManager = GetComponentInParent<GameManager>();
        }
        controller = gameManager.GetComponent<GameManager>().GetActivePlayer().GetComponent<PlayerManager>();
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (controller is null || !controller.speechInput) return;         // If controller (player) don't exist or not using speech input, do nothing

        // Else switch MoveStatus accordingly
        switch (args.text)
        {
            case MoveStatus.Stop:
                controller.UpdateCharacterMoveStatus(MoveStatus.Stop);
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
            case Commands.Switch:
                controller.SwitchCharacter();
                break;
            case Commands.TurnLeft:
                controller.UpdateCharacterMoveStatus(MoveStatus.TurnLeft);
                //controller.GetActiveCharacter().TurnLeft();
                break;
            case Commands.TurnRight:
                controller.UpdateCharacterMoveStatus(MoveStatus.TurnRight);
                //controller.GetActiveCharacter().TurnRight();
                break;
            case Commands.Shoot:
                controller.GetActiveCharacter().ShootBullet();
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
}
