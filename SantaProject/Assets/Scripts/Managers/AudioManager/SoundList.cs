using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundList
{
    public string name;

    [SerializeField]public List<Sound> sounds = new List<Sound>();
}
