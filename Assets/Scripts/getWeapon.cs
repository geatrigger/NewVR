using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class getWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    private static string leftHandWeapon;
    private static string rightHandWeapon;
    public static void setLeftHandWeapon(string LHW)
    {
        leftHandWeapon = LHW;
        return;
    }

    public static void setRightHandWeapon(string RHW)
    {
        rightHandWeapon = RHW;
        return;
    }
    public static string getLeftHandWeapon()
    {
        return leftHandWeapon;
    }

    public static string getRightHandWeapon()
    {
        return rightHandWeapon;
    }
    void Start()
    {
        
    }
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction; 
    private GameObject collidingObject;
    private GameObject objectInHand;

    private void SetCollidingObject(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Env"))
            return;
        if(collidingObject || !col.transform.parent.GetComponent<Rigidbody>() || col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        collidingObject = col.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if(collidingObject)
            {
                GrabObject();
            }
        }
        if(grabAction.GetLastStateUp(handType))
        {
            if(objectInHand)
            {
                ReleaseObject();
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    public void OnTriggerExit(Collider other)
    {
        if(!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }
    private void GrabObject()
    {
        collidingObject.transform.position = gameObject.transform.position;
        collidingObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(90, 0, 0);
        objectInHand = collidingObject;
        collidingObject = null;
        if (handType == SteamVR_Input_Sources.LeftHand)
            setLeftHandWeapon(objectInHand.name);
        else if (handType == SteamVR_Input_Sources.RightHand)
            setRightHandWeapon(objectInHand.name);

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if(GetComponent<FixedJoint>())
        {
            if (handType == SteamVR_Input_Sources.LeftHand)
                setLeftHandWeapon(null);
            else if (handType == SteamVR_Input_Sources.RightHand)
                setRightHandWeapon(null);
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }
    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }
}
