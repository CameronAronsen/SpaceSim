using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Universe
{
    public static float gravitationalConstant = 5f;
    public static float physicsTimeStep = 0.01f;
    public static bool cheatsEnabled = true;
    public static planetTypes planetSelected = planetTypes.None;

    public enum planetTypes
    {
        None,
        Terrestrial,
        Gas,
        Sun,
    }
}
