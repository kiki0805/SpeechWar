using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characterList;
    public bool speechInput;
    bool isActive;           // Keep track if this object active or not
    private int activeCharacter;

    private void SwitchCharacter()
    {
        for (int i = 0; i < characterList.Length; i++)
        {
            if(i == activeCharacter)
            {
                characterList[i].GetComponent<CharacterBase>().enabled = true;
            }
            else
            {
                characterList[i].GetComponent<CharacterBase>().enabled = false;
            }
        }
        activeCharacter = (activeCharacter + 1) % 4;
    }

    void Start()
    {
        activeCharacter = 0;
        if (isActive)
        {
            SwitchCharacter();
        }
        else
        {
            for (int i = 0; i < characterList.Length; i++)
            {
                characterList[i].GetComponent<CharacterBase>().enabled = false;
            }
        }
    }

    public void SetActive()
    {
        isActive = true;
        SwitchCharacter();
    }

    public void SetInactive()
    {
        isActive = false;
        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i].GetComponent<CharacterBase>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isActive)
        {
            SwitchCharacter();
        }
    }
}
