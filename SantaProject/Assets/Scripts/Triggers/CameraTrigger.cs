using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : Trigger
{
    [SerializeField] private CinemachineVirtualCamera myTrack;

    protected override void Triggered()
    {
        myTrack.gameObject.SetActive(true);
        if (GameManager.Instance.MainCamera != null)
        {
            if (GameManager.Instance.MainCamera != myTrack)
            {
                GameManager.Instance.MainCamera.gameObject.SetActive(false);
            }
        }
        GameManager.Instance.MainCamera = myTrack;
    }
}
