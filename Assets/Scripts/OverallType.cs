using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallType : CharacterBase
{
    [HideInInspector] public bool isActive;
    public Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            MoveCharacter();
        }
    }
}
