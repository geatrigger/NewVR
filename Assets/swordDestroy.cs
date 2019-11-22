using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordDestroy : MonoBehaviour
{
    GameObject enemySword;
    float waitTime;
    float bringswordTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = 0.0f;
        enemySword = GameObject.Find("enemysword");
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if(waitTime > bringswordTime)
        {
            enemySword.GetComponent<MeshRenderer>().enabled = true;
            enemySword.GetComponent<BoxCollider>().enabled = true;
            Destroy(gameObject);
        }
    }
}
