using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool playerDash;
    public bool playerDoubleJump;
    public bool playerSnowball;
    public float playerMaxHealth;
    public int presentsCollected;

    public float xSpawn;
    public float ySpawn;

    public bool[] isCollected = new bool[150];
    public int size = 34;
}
