using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static bool canCollision;
    public GameObject enemySword;
    public GameObject enemySwordrigid;
    public static float enemyStrength, enemyGrip, enemySwordWeight, enemySwordVelocity;
    // Start is called before the first frame update
    void Start()
    {
        enemyStrength = 1f; // will be changed in selection scene
        enemyGrip = 1f; // will be changed in selection scene
        enemySwordVelocity = 1; // will be changed in selection scene
        enemySwordWeight = 1; // will be changed in selection scene
        FightingUI.setScore(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
