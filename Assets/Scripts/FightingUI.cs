using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingUI : MonoBehaviour
{
    public Text scoreText;
    private static int p_score, e_score;
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
