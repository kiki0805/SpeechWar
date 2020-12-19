using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Vector3 localScale;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = GetComponentInParent<CharacterBase>().health / 5;
        transform.localScale = localScale;

        // Fixed rotation of healthbar
        transform.rotation = Quaternion.Euler(Vector3.zero);    // No rotation
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + (float)0.3, transform.parent.position.z); // Fixed position
    }
}
