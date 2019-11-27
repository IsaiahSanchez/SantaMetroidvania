﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem myfeetDust;

    public void ThrowSnowball()
    {
        GetComponentInParent<PlayerMain>().actuallyThrow();
    }

    public void playerFootStepSound()
    {
        createDust();
        GetComponentInParent<PlayerMain>().KickRock();
        AudioManager.instance.PlaySound("Footstep");
    }

    public void createDust()
    {
        myfeetDust.Play();
    }
}
