using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{
    int whichBonus;
    float speedBonus;
    private Rigidbody rb;
    Vector3 tempRb;
    void Start()
    {
        whichBonus = Random.Range(0, 5);
        speedBonus = GameManager.speedBox;
        rb = GetComponent<Rigidbody>();
        tempRb = -rb.velocity;
    }

    void Update()
    {
        if (!GameManager.isGameOver)
        {
            if (BonusManager.getbonusSlow())
            {
                speedBonus = BonusManager.slowOn;
            }
            else 
            {
                speedBonus = GameManager.speedBox;
            }
            rb.velocity = -speedBonus * (tempRb.normalized);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (transform.tag.Equals("BonusP") && other.transform.tag.Equals("Player"))
        {
            switch (whichBonus)
            {
                case 0:
                    BonusManager.Instance.setbonusVector();
                    break;
                case 1:
                     BonusManager.Instance.setbonusDamageBall();
                    break;
                case 2:
                     BonusManager.Instance.setbonusSlow();
                    break;
                case 3:
                    BonusManager.Instance.setbonusSpeedBall();
                    break;
                case 4:
                    BonusManager.Instance.setbonusDestroyAllBox();
                    break;
            }
            Destroy(transform.gameObject);
        }
    }
}
