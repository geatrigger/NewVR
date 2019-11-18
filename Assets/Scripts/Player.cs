using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit player collider");
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            //Debug.Log("ENEMY HIT PLAYER");
            FightingUI.addScore(false);
        }
    }
}
