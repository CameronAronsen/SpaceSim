using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{

    public Vector3 rotation;
    private Vector3 axialTilt;
    float randomDirection;

    // Start is called before the first frame update
    void Awake()
    {
        float randomAxialChange = Random.Range(-25f, 25f);
        randomDirection = Random.Range(-1f, 1f);
        axialTilt = new Vector3(randomAxialChange, 0, 0);
        transform.Rotate(axialTilt);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PauseBool.isPuased)
        {
            if (randomDirection < 0)
            {
                transform.Rotate(-rotation);
            }
            else
            {
                transform.Rotate(rotation);
            }
        }

    }
}
