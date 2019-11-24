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
    private IEnumerator coroutine;
    public bool isShield;

    public void vibrateHand()
    {
        vibration.Execute(0, 0.2f, 100, 1f, hand);
    }
    private IEnumerator restart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.position = controller.transform.position;
        gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        //weaponObject.transform.position = gameObject.transform.position;
        //weaponObject.transform.rotation = gameObject.transform.rotation;
        //rigid.isKinematic = true;
        //isGrapped = true;
        //rigid.useGravity = false;
        coroutine = restart(2.0f);
    }
    private IEnumerator setCollisionTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ScoreManager.canCollision = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        hand = isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        //isGrapped = true;
        //coroutine = restart(2.0f);
        weapon = isRight ? getWeapon.getRightHandWeapon() : getWeapon.getLeftHandWeapon();
        switch (weapon)
        {
            case "Shield":
                weaponObject = Instantiate(weapons[4]);
                break;
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
        //weaponObject.GetComponent<Sword>().hand = this;
        gameObject.transform.position = controller.transform.position;
        gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        //weaponObject.transform.position = gameObject.transform.position;
        //weaponObject.transform.rotation = gameObject.transform.rotation;
        weaponObject.GetComponent<Sword>().OnFirstGrap(this);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = controller.transform.position;
        gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
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

}
