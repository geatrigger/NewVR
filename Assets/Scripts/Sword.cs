using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sword : MonoBehaviour
{
    private Hand initHand;
    static Animator enemyAnimator;
    public float swordWeight;
    public bool isShield;
    AudioManager musicPlayer;
    GameObject musicPlayerObject;
    public GameObject enemySword;
    public GameObject enemySwordrigid;
    Vector3 prevPosition;
    Quaternion prevRotation;
    public Hand nowHand;
    public PickWeapon nowWeapon;
    private Rigidbody rigid;
    bool isGrapped;
    Vector3 velocity;

    private IEnumerator restart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.position = initHand.controller.transform.position;
        gameObject.transform.rotation = initHand.controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        rigid.velocity = new Vector3(0, 0, 0);
        OnGrap(initHand);
        //rigid.isKinematic = true;
        //isGrapped = true;
        //rigid.useGravity = false;
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        enemyAnimator = GameObject.Find("enemy").GetComponent<Animator>();
        musicPlayerObject = GameObject.Find("AudioSystem");
        enemySword = GameObject.Find("enemysword");
        musicPlayer = musicPlayerObject.GetComponent<AudioManager>();
        StartCoroutine(restart(2.0f));
        StartCoroutine(setCollisionTime(2.0f));
    }
    private void Update()
    {

        //Debug.Log(nowHand);
        //Debug.Log(nowWeapon);
        //Debug.Log(transform.position);
        if (isGrapped == true)
        {
            //Debug.Log(weaponObject.transform.position);
            gameObject.transform.position = nowHand.controller.transform.position;
            gameObject.transform.rotation = nowHand.controller.transform.rotation * Quaternion.Euler(90, 0, 0);
            //transform.position = nowHand.controller.transform.position;
            //transform.rotation = nowHand.controller.transform.rotation * Quaternion.Euler(90, 0, 0);
            velocity = (gameObject.transform.position - prevPosition) * (Time.deltaTime * 90) * 100;
            //Quaternion deltaRot = gameObject.transform.rotation *= tmpRotation * Quaternion.Inverse(prevRotation);
            if (!isShield && velocity.magnitude > Player.maxVelFactor / swordWeight)
            {
                //Debug.Log("velocity:" + velocity.magnitude + " maxVel/weight:" + maxVel / swordWeight + " playerStrength*weight" + playerStrength * swordWeight);

                enemyAnimator.SetBool("attacknow", true);
                OffGrap(false);
                //StartCoroutine(coroutine);
            }
            prevPosition = gameObject.transform.position;
            prevRotation = gameObject.transform.rotation;
        }
        else
        {
            prevPosition = this.transform.position;
            prevRotation = this.transform.rotation * Quaternion.Euler(90, 0, 0);
        }
    }
    public void OnFirstGrap(Hand hand)
    {
        OnGrap(hand);
        initHand = hand;
    }
    public void OnGrap(Hand hand)
    {
        nowHand = hand;
        nowWeapon = hand.getPickWeapon();
        gameObject.transform.position = nowHand.controller.transform.position;
        gameObject.transform.rotation = nowHand.controller.transform.rotation * Quaternion.Euler(90, 0, 0);
        prevPosition = gameObject.transform.position;
        prevRotation = gameObject.transform.rotation;
        //rigid.isKinematic = true;
        isGrapped = true;
        rigid.useGravity = false;
        Debug.Log("OnGrap!" + nowHand);
        //coroutine = restart(2.0f);
    }
    public void OffGrap(bool col)
    {
        OffGrap();
        if (col)
            rigid.velocity = -velocity;
        else
            rigid.velocity = velocity;
    }
    public void OffGrap()
    {
        nowWeapon.ReleaseObject();
        Debug.Log("OffGrap!" + nowHand);
        nowHand = null;
        isGrapped = false;
        rigid.isKinematic = false;
        rigid.useGravity = true;
        StartCoroutine(restart(2.0f));
    }
    private IEnumerator setCollisionTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ScoreManager.canCollision = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && ScoreManager.canCollision)
        {
            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("leftguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("upguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("rightguard") ||
                enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("frontguard"))
            {
                Debug.Log("guard!");
                musicPlayer.PlaySound(musicPlayer.defend);
            }
            else if (!isShield)
            {
                musicPlayer.PlaySound(musicPlayer.swordToBody);
                enemyAnimator.SetTrigger("hit");
                FightingUI.addScore(true);
                //collision.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
                if(nowHand != null)
                {
                    nowHand.vibrateHand();
                }
                //Debug.Log("Sword trigger");
                ScoreManager.canCollision = false;
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
                    OffGrap(true);
                    //StartCoroutine(coroutine);
                }
                else if (!isShield && ScoreManager.enemySwordVelocity * ScoreManager.enemyStrength > Player.Grip * swordWeight)
                {
                    musicPlayer.PlaySound(musicPlayer.swordToSword);
                    enemyAnimator.SetBool("attacknow", true);
                    OffGrap(true);
                    //StartCoroutine(coroutine);
                }
                else if (isShield || velocity.magnitude * Player.Strength > ScoreManager.enemyGrip * ScoreManager.enemySwordWeight)
                {
                    if (isShield)
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
                else if (!isShield && ScoreManager.enemySwordVelocity * ScoreManager.enemyStrength <= Player.Grip * swordWeight && velocity.magnitude * Player.Strength <= ScoreManager.enemyGrip * ScoreManager.enemySwordWeight)
                {
                    musicPlayer.PlaySound(musicPlayer.swordToSword);
                    enemyAnimator.SetBool("attacknow", true);
                    OffGrap(true);
                    //StartCoroutine(coroutine);
                }
            }

        }
    }
}
