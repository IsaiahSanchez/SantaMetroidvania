using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacingHandler : FacingHandler
{
    
    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void directionHasChanged(int direction)
    {
        faceOtherDirection(direction);
    }
}
