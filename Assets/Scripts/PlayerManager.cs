using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;         // Need to set this in Inspector
    public GameObject[] characterList;
    public bool speechInput;                // true = speechInput on
    bool isActive;                          // Keep track if (this) player active or not
    int charactersAlive;                    // Keep track of how many characters are alive
    private int activeCharacter = 0;


    void Start()
    {
        charactersAlive = 4;
    }

    public void SetSpeechInput(int hasSpeechInput)
    {
        if(hasSpeechInput == 1)
        {
            speechInput = true;
        }
        else
        {
            speechInput = false;
        }
    }

    public void UpdateCharacterMoveStatus(string status)
    {
        characterList[activeCharacter].GetComponent<CharacterBase>().UpdateMoveStatus(status);
    }

    public void SwitchCharacter()
    {
        characterList[activeCharacter].GetComponent<CharacterBase>().SetInactive();
        activeCharacter = (activeCharacter + 1) % 4;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetActive();
        if (characterList[activeCharacter].GetComponent<CharacterBase>().dead)
        {
            SwitchCharacter();
        }
    }

    public CharacterBase GetActiveCharacter()
    {
        return characterList[activeCharacter].GetComponent<CharacterBase>();
    }
    
    public void SetActive()
    {
        isActive = true;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetActive();
        if (characterList[activeCharacter].GetComponent<CharacterBase>().dead)
        {
            SwitchCharacter();
        }
    }

    public void SetInactive()
    {
        isActive = false;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetInactive();
    }

    public void RemoveCharacter()
    {
        charactersAlive--;
        if(charactersAlive <= 0)
        {
            gameManager.GetComponent<GameManager>().EndGame();      // Go to end scene
        }
        Debug.Log("Removed character");
    }

    public int GetCharactersAlive()
    {
        return charactersAlive;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isActive && !speechInput)
        {
            SwitchCharacter();
        }
    }
}
