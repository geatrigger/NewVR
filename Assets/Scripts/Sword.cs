using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    public GameObject rightController;
    float maxAngle = 10f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = rightController.transform.position;
        gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
        hand = SteamVR_Input_Sources.RightHand;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = rightController.transform.position;
        gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
        //최대속도를 유지해서 따라가는 것
        /*
        Quaternion angle = gameObject.transform.rotation * Quaternion.Euler(-90, 0, 0) * Quaternion.Inverse(rightController.transform.rotation);
        if(maxAngle > angle.x / Time.deltaTime && maxAngle > angle.y / Time.deltaTime && maxAngle > angle.z / Time.deltaTime)
        {
            gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = gameObject.transform.rotation *
                Quaternion.Euler(Mathf.Max(maxAngle * Time.deltaTime, angle.x), Mathf.Max(maxAngle * Time.deltaTime, angle.y), Mathf.Max(maxAngle * Time.deltaTime, angle.z));
        }
        gameObject.transform.position = rightController.transform.position;
        */
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
