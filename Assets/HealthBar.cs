using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Vector3 localScale;
    CharacterBase character;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        character = GetComponentInParent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if(character.health <= 0)
        {
            localScale.x = 0;
        }
        else
        {
            localScale.x = character.health / 5;
        }

        transform.localScale = localScale;

        // Fixed rotation of healthbar
        transform.rotation = Quaternion.Euler(Vector3.zero);    // No rotation
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + (float)0.3, transform.parent.position.z); // Fixed position
    }
}
