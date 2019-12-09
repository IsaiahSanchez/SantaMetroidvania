using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockKicker : MonoBehaviour
{
    [SerializeField] GameObject RockToSpawn;
    [SerializeField] float ChanceToSpawn;
    [SerializeField] float KickSpeedMin;
    [SerializeField] float KickSpeedMax;
    [SerializeField] float maxScale;
    [SerializeField] float minScale;

    public void TryToKickRock()
    {
        float rand = Random.Range(0, 1f);
        if (rand <= ChanceToSpawn)
        {
            int numberOfRocks = Random.Range(1,3);

            while (numberOfRocks > 0)
            {
                numberOfRocks--;
                GameObject temp = Instantiate(RockToSpawn, transform.position, transform.rotation);
                temp.transform.parent = null;
                temp.GetComponent<Rigidbody2D>().velocity = transform.right * Random.Range(KickSpeedMin,KickSpeedMax);
                float scale = Random.Range(minScale, maxScale);
                temp.transform.localScale = new Vector3(scale, scale);
            }
        }
    }
}
