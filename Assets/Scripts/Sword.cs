using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    public GameObject rightController;
    float maxRot = 1f, maxPos = 0.1f;
    Vector3 prevPosition, tmpPosition;
    Quaternion prevRotation, tmpRotation;
    bool isGrapped;
    private IEnumerator coroutine;
    private Rigidbody rigid;
    private IEnumerator restart(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            gameObject.transform.position = rightController.transform.position;
            gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
            prevPosition = gameObject.transform.position;
            prevRotation = gameObject.transform.rotation;
            rigid.isKinematic = true;
            rigid.useGravity = false;
            isGrapped = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = rightController.transform.position;
        gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
        prevPosition = gameObject.transform.position;
        prevRotation = gameObject.transform.rotation;
        hand = SteamVR_Input_Sources.RightHand;
        isGrapped = true;
        rigid = gameObject.GetComponent<Rigidbody>();
        coroutine = restart(20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrapped == true)
        {
            gameObject.transform.position = rightController.transform.position;
            gameObject.transform.rotation = rightController.transform.rotation * Quaternion.Euler(90, 0, 0);
            Vector3 deltaPos = tmpPosition - prevPosition;
            //Quaternion deltaRot = gameObject.transform.rotation *= tmpRotation * Quaternion.Inverse(prevRotation);
            if (deltaPos.magnitude > maxPos)
            {
                Debug.Log("deltaPos : " + deltaPos.magnitude);
                isGrapped = false;
                rigid.isKinematic = false;
                rigid.useGravity = true;
                StartCoroutine(coroutine);
            }
            prevPosition = gameObject.transform.position;
            prevRotation = gameObject.transform.rotation;
        }
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
