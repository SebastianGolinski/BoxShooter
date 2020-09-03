using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour {

    public GameObject background1;
    public GameObject background2;
    private Rigidbody rb1, rb2;
    float speedBackground = GameManager.speedBox;
    private void Start()
    {
        rb1 = background1.GetComponent<Rigidbody>();
        rb2 = background2.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!GameManager.isGameOver)
        {
            rb1.velocity = -speedBackground * (new Vector3(0, 0, 1).normalized);
            rb2.velocity = -speedBackground * (new Vector3(0, 0, 1).normalized);

            if (BonusManager.getbonusSlow())
            {
                speedBackground = BonusManager.slowOn;
            }
            else
            {
                speedBackground = GameManager.speedBox;
            }
            if (background1.transform.position.z < -25)
            {
                background1.transform.position = new Vector3(background1.transform.position.x, background1.transform.position.y, 35.9f);
            }
            else if (background2.transform.position.z < -25)
            {
                background2.transform.position = new Vector3(background2.transform.position.x, background2.transform.position.y, 35.9f);
            }
        }
        else
        {
            rb1.velocity = new Vector3(0, 0, 0);
            rb2.velocity = new Vector3(0, 0, 0);
        }
            
    }
}
