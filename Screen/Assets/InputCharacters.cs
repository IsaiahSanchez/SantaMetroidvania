using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCharacters : MonoBehaviour
{
    [SerializeField] InputField xMod, yMod, charInput;

    public void submit()
    {
        Screen.instance.screenMemory[int.Parse(xMod.text), int.Parse(yMod.text)] = char.Parse(charInput.text);
    }
}
