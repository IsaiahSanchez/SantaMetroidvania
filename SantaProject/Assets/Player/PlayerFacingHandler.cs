using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingHandler : MonoBehaviour
{
    [SerializeField] private Transform snowballThrowTransform;
    [SerializeField] private Transform graphicTransform;

    private float startingThrowX;
    private int currentFacing = 1;

    private void Awake()
    {
        startingThrowX = snowballThrowTransform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
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

    private void faceOtherDirection(int direction)
    {
        //handle snowballThrow
        snowballThrowTransform.localPosition = new Vector2(direction * startingThrowX,snowballThrowTransform.localPosition.y);
        if (direction < 0)
        {
            graphicTransform.eulerAngles = new Vector3(graphicTransform.eulerAngles.x, 180, graphicTransform.eulerAngles.z);
            snowballThrowTransform.eulerAngles = new Vector3(snowballThrowTransform.eulerAngles.x, 180, snowballThrowTransform.eulerAngles.z);
        }
        else
        {
            graphicTransform.eulerAngles = new Vector3(graphicTransform.eulerAngles.x, 0, graphicTransform.eulerAngles.z);
            snowballThrowTransform.eulerAngles = new Vector3(snowballThrowTransform.eulerAngles.x, 0, snowballThrowTransform.eulerAngles.z);
        }
    }
}
