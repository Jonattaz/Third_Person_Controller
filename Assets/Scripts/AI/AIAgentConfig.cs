using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject{
    //Second
    public float maxTime = 1.0f;
    //Meter
    public float maxDistance = 1.0f;
    public float dieForce = 7.0f;
    public float maxSightDistance = 5.0f;

}
