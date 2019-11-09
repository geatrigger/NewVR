using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{

    Animator myanimator;
    // Start is called before the first frame update
    void Start()
    {
        myanimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myanimator.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
        {
            if (Input.GetKey(KeyCode.Alpha1) == true)
            {
                myanimator.SetInteger("Attack", 1);
            }
            else if (Input.GetKey(KeyCode.Alpha2) == true)
            {
                myanimator.SetInteger("Attack", 2);

            }
            else if (Input.GetKey(KeyCode.Alpha3) == true)
            {
                myanimator.SetInteger("Attack", 0);

            }
        }
    }
}
