using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject planetPrefab;
    public void PauseGame()
    {
        PauseBool.isPuased = true;
    }

    public void PlayGame()
    {
        PauseBool.isPuased = false;
    }

    public void FastForward()
    {
        Universe.physicsTimeStep /= 2;
    }

    public void SlowTime()
    {
        Universe.physicsTimeStep *= 2;
    }

    public void SpawnPlanet()
    {
        Universe.planetSelected = Universe.planetTypes.Terrestrial;
    }
}
