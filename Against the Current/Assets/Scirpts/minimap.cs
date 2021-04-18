using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    Camera minimapC;
    public float size;
    void Start()
    {
        minimapC = this.GetComponent<Camera>();
    }
    void Update()
    {
        minimapC.orthographicSize = size;
    }

    public void SetMiniMapSize(float s)
    {
        size = s;
    }
}
