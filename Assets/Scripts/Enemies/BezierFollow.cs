using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector2 bossPosition;

    private float sppedModifier;

    private bool coroutineAllowed;

    // Use this for initialization
    void Start()
    {
        
    }
}
