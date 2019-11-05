using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    private string nameLH, nameRH;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nameLH);
        Debug.Log(nameRH);
        Debug.Log(time);
        time += Time.deltaTime;
        nameLH = getWeapon.getLeftHandWeapon();
        nameRH = getWeapon.getRightHandWeapon();
        if(nameLH == null || nameRH == null)
        {
            time = 0;
        }
        if(time >= 5.0f)
        {
            StartingFight();
        }

    }

    void StartingFight()
    {
        SceneManager.LoadScene("playerSword");
    }
    
}
