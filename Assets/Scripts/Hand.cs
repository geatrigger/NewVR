using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    Animator enemyAnimator;
    SteamVR_Input_Sources hand;
    public SteamVR_Action_Vibration vibration;
    public GameObject controller;
    public bool isRight;
    public string weapon;
    public GameObject[] weapons;
    public GameObject weaponObject;
    float maxRot = 1f, maxVelFactor = 5f;
    Vector3 prevPosition;
    Quaternion prevRotation;
    bool isGrapped;
    private IEnumerator coroutine;
    private Rigidbody rigid;
    public static bool canCollision;
    Vector3 velocity;
    float swordWeight, playerStrength, playerGrip;
    float enemyStrength, enemyGrip, enemySwordWeight, enemySwordVelocity;
    public bool isShield;
    public GameObject enemySword;
    public GameObject enemySwordrigid;
    public GameObject musicPlayerObject;
    AudioManager musicPlayer;

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
        enemyAnimator = GameObject.Find("enemy").GetComponent<Animator>();
        enemyStrength = 1f; // will be changed in selection scene
        enemyGrip = 1f; // will be changed in selection scene
        enemySwordVelocity = 1; // will be changed in selection scene
        enemySwordWeight = 1; // will be changed in selection scene
        playerStrength = 1f; // will be changed in selection scene
        playerGrip = 1f;
        hand = isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        isGrapped = true;
        coroutine = restart(2.0f);
        string leftHandWeapon = getWeapon.getLeftHandWeapon();
        string rightHandWeapon = getWeapon.getRightHandWeapon();
        weapon = isRight ? rightHandWeapon : leftHandWeapon;
        switch (weapon)
        {
            case "Shield":
                weaponObject = Instantiate(weapons[4]);
                swordWeight = 100;
                isShield = true;
                break;
            case "Sabre":
                weaponObject = Instantiate(weapons[3]);
                swordWeight = 1;
                isShield = false;
                break;
            case "Broadsword":
                weaponObject = Instantiate(weapons[1]);
                swordWeight = 2;
                isShield = false;
                break;
            case "Rapier":
                weaponObject = Instantiate(weapons[2]);
                swordWeight = 0.5f;
                isShield = false;
                break;
            default:
                weaponObject = Instantiate(weapons[0]);
                swordWeight = 2;
                isShield = false;
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
        musicPlayer = musicPlayerObject.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrapped == true)
        {
            //Debug.Log(weaponObject.transform.position);
            gameObject.transform.position = controller.transform.position;
            gameObject.transform.rotation = controller.transform.rotation * Quaternion.Euler(90, 0, 0);
            weaponObject.transform.position = gameObject.transform.position;
            weaponObject.transform.rotation = gameObject.transform.rotation;
            velocity = (gameObject.transform.position - prevPosition) * (Time.deltaTime * 90) * 100;
            //Quaternion deltaRot = gameObject.transform.rotation *= tmpRotation * Quaternion.Inverse(prevRotation);
            if (!isShield && velocity.magnitude > maxVelFactor / swordWeight)
            {
                //Debug.Log("velocity:" + velocity.magnitude + " maxVel/weight:" + maxVel / swordWeight + " playerStrength*weight" + playerStrength * swordWeight);

                enemyAnimator.SetBool("attacknow", true);
                isGrapped = false;
                rigid.isKinematic = false;
                rigid.useGravity = true;
                rigid.velocity = velocity;
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
        //Debug.Log("trigger");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && canCollision)
        {
            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("leftguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("upguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("rightguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("frontguard"))
            {
                Debug.Log("guard!");
                musicPlayer.PlaySound(musicPlayer.defend);
            }
            else if(!isShield)
            {
                musicPlayer.PlaySound(musicPlayer.swordToBody);
                enemyAnimator.SetTrigger("hit");
                FightingUI.addScore(true);
                //collision.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
                vibration.Execute(0, 0.2f, 100, 1f, hand);
                //Debug.Log("Sword trigger");
                canCollision = false;
                StartCoroutine(setCollisionTime(2.0f));
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            //Debug.Log("Hit enemy weapon!");
            Debug.Log("velocity:" + velocity.magnitude);

            enemyAnimator.SetBool("collision", true);
            if (isGrapped)
            {
                if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("leftguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("upguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("rightguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("frontguard"))
                {
                    musicPlayer.PlaySound(musicPlayer.defend);
                    Debug.Log("guard!");
                    enemyAnimator.SetBool("attacknow", true);
                    isGrapped = false;
                    rigid.isKinematic = false;
                    rigid.useGravity = true;
                    rigid.velocity = -velocity;
                    StartCoroutine(coroutine);
                }
                else if (!isShield && enemySwordVelocity * enemyStrength > playerGrip * swordWeight)
                {
                    musicPlayer.PlaySound(musicPlayer.swordToSword);
                    enemyAnimator.SetBool("attacknow", true);
                    isGrapped = false;
                    rigid.isKinematic = false;
                    rigid.useGravity = true;
                    rigid.velocity = -velocity;
                    StartCoroutine(coroutine);
                }
                else if (isShield || velocity.magnitude * playerStrength > enemyGrip * enemySwordWeight)
                {
                    if(isShield)
                        musicPlayer.PlaySound(musicPlayer.defend);
                    else
                        musicPlayer.PlaySound(musicPlayer.swordToSword);
                    enemySword.GetComponent<MeshRenderer>().enabled = false;
                    enemySword.GetComponent<BoxCollider>().enabled = false;
                    GameObject flyingSword = Instantiate(enemySwordrigid);
                    flyingSword.transform.position = enemySword.transform.position;
                    flyingSword.transform.rotation = enemySword.transform.rotation;
                    flyingSword.GetComponent<Rigidbody>().velocity = velocity;
                    flyingSword.GetComponent<Rigidbody>().isKinematic = false;
                }
                else if (!isShield && enemySwordVelocity * enemyStrength <= playerGrip * swordWeight && velocity.magnitude * playerStrength <= enemyGrip * enemySwordWeight)
                {
                    musicPlayer.PlaySound(musicPlayer.swordToSword);
                    enemyAnimator.SetBool("attacknow", true);
                    isGrapped = false;
                    rigid.isKinematic = false;
                    rigid.useGravity = true;
                    rigid.velocity = -velocity;
                    StartCoroutine(coroutine);
                }
            }
            
        }
    }
}
