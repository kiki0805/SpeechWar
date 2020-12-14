using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankType : CharacterBase
{
    [HideInInspector] public bool isActive;
    public Rigidbody2D body;

    public void Enable() {
        isActive = !isActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mode = true;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            MoveCharacter();
        }
    }
}
