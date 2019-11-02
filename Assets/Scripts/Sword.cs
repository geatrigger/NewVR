using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    // Start is called before the first frame update
    void Start()
    {
        hand = SteamVR_Input_Sources.RightHand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Animator>().SetTrigger("Damage");
            vibration.Execute(0, 0.2f, 100, 1f, hand);
            Debug.Log("Sword trigger");
        }
    }
}
