﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : Trigger
{
    protected override void Triggered()
    {
        GameManager.Instance.StartWin();
    }

}
