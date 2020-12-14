using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechController : MonoBehaviour
{
    public GameObject character;
    protected DictationRecognizer dictationRecognizer;
    public bool speechMode;

    CharacterBase controller;

    void Start()
    {
        StartDictationEngine();     // Start listening for commands
    }
    private void DictationRecognizer_OnDictationHypothesis(string text)
    {
        Debug.Log("Dictation hypothesis: " + text);
    }
    private void DictationRecognizer_OnDictationComplete(DictationCompletionCause completionCause)
    {
        switch (completionCause)
        {
            case DictationCompletionCause.TimeoutExceeded:
            case DictationCompletionCause.PauseLimitExceeded:
            case DictationCompletionCause.Canceled:
            case DictationCompletionCause.Complete:
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.UnknownError:
            case DictationCompletionCause.AudioQualityFailure:
            case DictationCompletionCause.MicrophoneUnavailable:
            case DictationCompletionCause.NetworkFailure:
                // Error
                CloseDictationEngine();
                break;
        }
    }
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {
        if (!speechMode) return;
        switch (text)
        {
            case MoveStatus.Still:
                controller.UpdateMoveStatus(MoveStatus.Still);
                break;
            case MoveStatus.Right:
                controller.UpdateMoveStatus(MoveStatus.Right);
                break;
            case MoveStatus.Left:
                controller.UpdateMoveStatus(MoveStatus.Left);
                break;
            case MoveStatus.Up:
                controller.UpdateMoveStatus(MoveStatus.Up);
                break;
            case MoveStatus.Down:
                controller.UpdateMoveStatus(MoveStatus.Down);
                break;
            default:
                break;
        }
        Debug.Log("Dictation result: " + text);
    }
    private void DictationRecognizer_OnDictationError(string error, int hresult)
    {
        Debug.Log("Dictation error: " + error);
    }
    private void OnApplicationQuit()
    {
        CloseDictationEngine();
    }
    private void StartDictationEngine()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
    }
    private void CloseDictationEngine()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis;
            dictationRecognizer.DictationComplete -= DictationRecognizer_OnDictationComplete;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            dictationRecognizer.DictationError -= DictationRecognizer_OnDictationError;
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}