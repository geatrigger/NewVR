using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    public GameObject rightController;
    float maxVelocity = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        hand = SteamVR_Input_Sources.RightHand;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = rightController.transform.position;
        //gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
        //최대속도를 유지해서 따라가는 것
        Vector3 direction = rightController.transform.position - gameObject.transform.position;
        if(Time.deltaTime * maxVelocity < direction.magnitude)
        {
            gameObject.transform.position = rightController.transform.position;
        }
        else
        {
            gameObject.transform.position += direction.normalized * maxVelocity;
        }
        gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
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
