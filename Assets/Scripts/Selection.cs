using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    private string nameLH, nameRH;
    private float time;
    public Text leftHand, rightHand, timeText;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        nameLH = getWeapon.getLeftHandWeapon();
        nameRH = getWeapon.getRightHandWeapon();
        

        if(nameLH == null || nameRH == null)
        {
            time = 0;
            timeText.text = "두 무기를 선택해 주세요.";
        }
        if(time >= 5.0f)
        {
            timeText.text = time.ToString() + "초 후 시작합니다.";
            StartingFight();
        }
        if (nameLH == null)
            nameLH = "Hand";
        if (nameRH == null)
            nameRH = "Hand";
        leftHand.text = "현재 왼손 무기 : " + nameLH;
        rightHand.text = "현재 오른손 무기 : " + nameRH;

    }

    void StartingFight()
    {
        SceneManager.LoadScene("playerSword");
    }
    
}
