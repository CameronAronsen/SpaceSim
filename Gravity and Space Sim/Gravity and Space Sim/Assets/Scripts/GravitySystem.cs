using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    PlanetGravity[] planets;

    private void Awake()
    {
        planets = FindObjectsOfType<PlanetGravity>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        for (int i = 0; i < planets.Length; i++)
        {
            if (!PauseBool.isPuased)
            {
                planets[i].CalculateSpeed(planets, Universe.physicsTimeStep);
            }
        }

        for (int i = 0; i < planets.Length; i++)
        {
            if (!PauseBool.isPuased)
            {
                planets[i].UpdatePosition(Universe.physicsTimeStep);
            }
        }

    }
}
