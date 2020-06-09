using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereNavigator : MonoBehaviour
{
    public float R = 10;
    public Vector3 StartingVector;

    private Vector3 _v;

    void Start()
    {
        _v = StartingVector;
    }

    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        var p1 = p + _v;
        
        var p2 = (R / p1.magnitude) * p1;
        _v = p2 - p;

        transform.position = p2;
    }
}
