using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Screen : MonoBehaviour
{
    public static Screen instance;
    public Text display;
    [SerializeField] public char[,] screenMemory = new char[40,25];

    private void Awake()
    {
        instance = this;
        display = GetComponent<Text>();
    }

}
