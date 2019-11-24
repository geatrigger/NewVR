using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image myImage;
    public GameObject musicPlayerObject;
    AudioManager musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myImage.enabled = false;
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
            StartCoroutine("fadeInOut");
        }
    }
    IEnumerator fadeIn()
    {
        Color fadeColor = myImage.color;
        fadeColor.a = 1.0f;
        float time = 0f;
        while (fadeColor.a > 0.0f)
        {
            time += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1.0f, 0.0f, time * 5.0f);
            myImage.color = fadeColor;
            yield return null;
        }
        fadeColor.a = 0.0f;
        myImage.color = fadeColor;
        myImage.enabled = false;
    }
    IEnumerator fadeOut()
    {
        Color fadeColor = myImage.color;
        fadeColor.a = 0.0f;
        float time = 0f;
        while (fadeColor.a < 1.0f)
        {
            time += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0.0f, 1.0f, time * 5.0f);
            myImage.color = fadeColor;
            yield return null;
        }
        fadeColor.a = 1.0f;
        myImage.color = fadeColor;
        //myImage.enabled = false; //페이드아웃 하고 화면 까만 상태 유지하려면 이거 키면 됨.
        //yield return new WaitForSeconds(2.0f);

        //StartCoroutine("fadeIn");
    }
    IEnumerator fadeInOut()
    {
        myImage.enabled = true;

        StartCoroutine("fadeOut");
        yield return new WaitForSeconds(0.3f);
        StartCoroutine("fadeIn");
    }

}
