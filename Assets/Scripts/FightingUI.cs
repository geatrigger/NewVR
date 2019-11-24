using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingUI : MonoBehaviour
{
    public Text scoreText, resultText;
    public GameObject enemy, player;
    private static int p_score, e_score;
    public GameObject cameraRig;
    // Start is called before the first frame update
    void Awake()
    {
        p_score = 0;
        e_score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        string text = p_score.ToString() + ":" + e_score.ToString();

        scoreText.text = text;
        
        if(p_score >= 8)
        {
            text = "You Win!";
            resultText.text = text;
            cameraRig.transform.position = new Vector3(0.0f, 6.5f, 5.0f);
            player.SetActive(false);
            enemy.SetActive(false);
        }
        else if(e_score >= 8){
            text = "You Lose!";
            resultText.text = text;
            cameraRig.transform.position = new Vector3(0.0f, 6.5f, 5.0f);
            player.SetActive(false);
            enemy.SetActive(false);
        }

    }

    public static void setScore(int p, int e)
    {
        p_score = p;
        e_score = e;
    }

    public static void addScore(bool player) // true - player addscore, false - enemy addscore
    {
        if (player)
            p_score++;
        else
            e_score++;
        return;
    }
}
