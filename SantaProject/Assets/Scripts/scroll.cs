using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scroll : MonoBehaviour
{
    private Material mat;

    [SerializeField] private float speed = 0.5f;

    private void Start()
    {
        mat = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        mat.mainTextureOffset += new Vector2(Time.deltaTime * -speed, 0);
    }
}
