using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void ThrowSnowball()
    {
        GetComponentInParent<PlayerMain>().actuallyThrow();
    }
}
