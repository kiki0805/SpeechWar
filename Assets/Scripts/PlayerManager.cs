using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characterList;
    public bool speechInput;
    bool isActive;           // Keep track if this object active or not
    private int activeCharacter = 0;

    public void UpdateCharacterMoveStatus(string status)
    {
        characterList[activeCharacter].GetComponent<CharacterBase>().UpdateMoveStatus(status);
    }

    private void SwitchCharacter()
    {
        characterList[activeCharacter].GetComponent<CharacterBase>().SetInactive();
        activeCharacter = (activeCharacter + 1) % 4;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetActive();
    }

    void Start()
    {
        //SwitchCharacter();
    }

    public void SetActive()
    {
        isActive = true;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetActive();
    }

    public void SetInactive()
    {
        isActive = false;
        characterList[activeCharacter].GetComponent<CharacterBase>().SetInactive();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isActive)
        {
            SwitchCharacter();
        }
    }
}
