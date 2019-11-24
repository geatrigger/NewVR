using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject musicPlayerObject;
    AudioManager musicPlayer;
    public static float Strength = 1f;
    public static float Grip = 1f;
    public static float maxRot = 1f, maxVelFactor = 5f;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = musicPlayerObject.GetComponent<AudioManager>();

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
            musicPlayer.PlaySound(musicPlayer.swordToBody);
            FightingUI.addScore(false);
        }
    }
}
