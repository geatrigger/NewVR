using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    public GameObject controller;
    public bool isRight;
    public string weapon;
    public GameObject[] weapons;
    public GameObject weaponObject;
    float maxRot = 1f, maxPos = 0.05f;
    Vector3 prevPosition;
    Quaternion prevRotation;
    bool isGrapped;
    private IEnumerator coroutine;
    private Rigidbody rigid;
    bool canCollision;
    private IEnumerator restart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.position = controller.transform.position;
        gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        weaponObject.transform.position = gameObject.transform.position;
        weaponObject.transform.rotation = gameObject.transform.rotation;
        //rigid.isKinematic = true;
        isGrapped = true;
        rigid.useGravity = false;
        coroutine = restart(2.0f);
    }
    private IEnumerator setCollisionTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canCollision = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        hand = isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        isGrapped = true;
        coroutine = restart(2.0f);
        string leftHandWeapon = getWeapon.getLeftHandWeapon();
        string rightHandWeapon = getWeapon.getRightHandWeapon();
        weapon = isRight ? leftHandWeapon : rightHandWeapon;
        switch (weapon)
        {
            case "Sabre":
                weaponObject = Instantiate(weapons[3]);
                break;
            case "Broadsword":
                weaponObject = Instantiate(weapons[1]);
                break;
            case "Rapier":
                weaponObject = Instantiate(weapons[2]);
                break;
            default:
                weaponObject = Instantiate(weapons[0]);
                break;
        }
        weaponObject.GetComponent<Sword>().hand = this;
        rigid = weaponObject.GetComponent<Rigidbody>();
        gameObject.transform.position = controller.transform.position;
        gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        prevPosition = gameObject.transform.position;
        prevRotation = gameObject.transform.rotation;
        weaponObject.transform.position = gameObject.transform.position;
        weaponObject.transform.rotation = gameObject.transform.rotation;
        canCollision = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrapped == true)
        {
            gameObject.transform.position = controller.transform.position;
            gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
            weaponObject.transform.position = gameObject.transform.position;
            weaponObject.transform.rotation = gameObject.transform.rotation;
            Vector3 deltaPos = gameObject.transform.position - prevPosition;
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
        else
        {
            prevPosition = controller.transform.position;
            prevRotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        }
        //최대속도를 유지해서 따라가는 것
        /*
        Quaternion angle = gameObject.transform.rotation * Quaternion.Euler(-90, 0, 0) * Quaternion.Inverse(controller.transform.rotation);
        if(maxAngle > angle.x / Time.deltaTime && maxAngle > angle.y / Time.deltaTime && maxAngle > angle.z / Time.deltaTime)
        {
            gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = gameObject.transform.rotation *
                Quaternion.Euler(Mathf.Max(maxAngle * Time.deltaTime, angle.x), Mathf.Max(maxAngle * Time.deltaTime, angle.y), Mathf.Max(maxAngle * Time.deltaTime, angle.z));
        }
        gameObject.transform.position = controller.transform.position;
        */
    }

    public void childOnCollisionEnter(Collision collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && canCollision)
        {
            if (collision.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Guard"))
            {
                Debug.Log("guard!");
            }
            else {
                collision.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
                vibration.Execute(0, 0.2f, 100, 1f, hand);
                Debug.Log("Sword trigger");
                canCollision = false;
                StartCoroutine(setCollisionTime(4.0f));
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            isGrapped = false;
            rigid.isKinematic = false;
            rigid.useGravity = true;
            StartCoroutine(coroutine);
        }
    }
}
