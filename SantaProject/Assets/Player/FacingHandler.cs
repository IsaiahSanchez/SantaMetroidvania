using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingHandler : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform graphicTransform;

    private float aimTransformX;
    private int currentFacing = 1;

    private void Awake()
    {
        if (aimTransform != null)
        {
            aimTransformX = aimTransform.localPosition.x;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            int facing = (int)Input.GetAxisRaw("Horizontal");
            if (currentFacing != facing)
            {
                currentFacing = facing;
                faceOtherDirection(facing);
            }
        }
    }

    protected void faceOtherDirection(int direction)
    {
        //handle snowballThrow
        if (aimTransform != null)
        {
            aimTransform.localPosition = new Vector2(direction * aimTransformX, aimTransform.localPosition.y);
        }

        if (direction < 0)
        {
            graphicTransform.eulerAngles = new Vector3(graphicTransform.eulerAngles.x, 180, graphicTransform.eulerAngles.z);
            if (aimTransform != null)
            {
                aimTransform.eulerAngles = new Vector3(aimTransform.eulerAngles.x, 180, aimTransform.eulerAngles.z);
            }
        }
        else
        {
            graphicTransform.eulerAngles = new Vector3(graphicTransform.eulerAngles.x, 0, graphicTransform.eulerAngles.z);
            if (aimTransform != null)
            {
                aimTransform.eulerAngles = new Vector3(aimTransform.eulerAngles.x, 0, aimTransform.eulerAngles.z);
            }
        }
    }
}
