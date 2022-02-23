using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public Vector3 initialSpeed;
    public Vector3 currentSpeed;
    private Vector3 predictSpeed;
    private Vector3 predictPos;
    Rigidbody rigidBody;
    public float mass;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentSpeed = initialSpeed;
        predictSpeed = currentSpeed;
        predictPos = rigidBody.position;
    }

    public void CalculateSpeed(PlanetGravity[] allPlanets, float timeStep)
    {
        foreach(var planet in allPlanets)
        {
            if(planet != this)
            {
                float distance = (planet.rigidBody.position - rigidBody.position).sqrMagnitude;
                Vector3 direction = (planet.rigidBody.position - rigidBody.position).normalized;

                Vector3 force = direction * Universe.gravitationalConstant * mass * planet.mass / distance;
                Vector3 acceleration = force / mass;

                currentSpeed += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        if (!PauseBool.isPuased)
        {
            rigidBody.position += currentSpeed * timeStep;
        }
    }
}
