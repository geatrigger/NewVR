using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    public Hand hand;
    void OnCollisionEnter(Collision collision)
    {
        hand.childOnCollisionEnter(collision);
    }
}
