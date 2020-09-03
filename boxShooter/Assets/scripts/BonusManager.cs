using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{

    public static BonusManager Instance;
    private static bool bonusVector;
    public static bool getbonusVector()
    {
        return bonusVector;
    }
    private static bool bonusDamageBall;
    public static bool getbonusDamageBall()
    {
        return bonusDamageBall;
    }
    private static bool bonusSlow;
    public static bool getbonusSlow() {
        return bonusSlow;
    }
    private static bool bonusSpeedBall;
    public static bool getbonusSpeedBall()
    {
        return bonusSpeedBall;
    }
    private static bool bonusTextShow;
	private static bool bonusDestroyAllBox;
    public static bool getbonusDestroyAllBox() {
        return bonusDestroyAllBox;
    }

    float timeActivBonusVector;
    float timeActivBonusSpeedBox;
    float timeActivBonusSlow;
    float timeActivBonusSpeedBall;

    float timeActivBonusVectorOrigin;
    float timeActivBonusSpeedBoxOrigin;
    float timeActivBonusSlowOrigin;
    float timeActivBonusSpeedBallOrigin;
    public static float slowOn;
    public GameObject bonusText;
    private TextMeshPro textMesh;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        timeActivBonusVectorOrigin = 5f;
        timeActivBonusSpeedBoxOrigin = 5f;
        timeActivBonusSlowOrigin = 5f;
        timeActivBonusSpeedBallOrigin = 5f;

        timeActivBonusVector = timeActivBonusVectorOrigin;
        timeActivBonusSpeedBox = timeActivBonusSpeedBoxOrigin;
        timeActivBonusSlow = timeActivBonusSlowOrigin;
        timeActivBonusSpeedBall = timeActivBonusSpeedBallOrigin;

        bonusVector = false;
        bonusDamageBall = false;
        bonusSlow = false;
        bonusSpeedBall = false;
        bonusDestroyAllBox = false;

        slowOn = GameManager.speedBox - 1;
        bonusText.SetActive(false);
        textMesh = bonusText.GetComponent<TextMeshPro>();
        textMesh.SetText("     ");
    }

    public void setbonusDamageBall() {
        bonusTextShow = true;
        textMesh.SetText("PODWÓJNE \n USZKODZENIA");
        BonusManager.bonusDamageBall = true;
    }

    public void setbonusVector()
    {
        bonusTextShow = true;
        textMesh.SetText("PODWÓJNY \n STRZAŁ");
        BonusManager.bonusVector = true;
    }

    public void setbonusSlow()
    {
        bonusTextShow = true;
        textMesh.SetText("WOLNIEJ");
        BonusManager.bonusSlow = true;
    }

    public void setbonusSpeedBall()
    {
        bonusTextShow = true;
        textMesh.SetText("SZYBCIEJ");
        BonusManager.bonusSpeedBall = true;
    }

    public void setbonusDestroyAllBox()
    {
        bonusTextShow = true;
        textMesh.SetText("BOOM");
        BonusManager.bonusDestroyAllBox = true;
    }
   
    void Update()
    {
        if (bonusVector)
        {
            timeActivBonusVector -= Time.deltaTime;
            if (timeActivBonusVector < 0)
            {
                bonusVector = false;
                timeActivBonusVector = timeActivBonusVectorOrigin;
            }
        }
        else if (bonusDamageBall)
        {
            timeActivBonusSpeedBox -= Time.deltaTime;
            if (timeActivBonusSpeedBox < 0)
            {
                bonusDamageBall = false;
                timeActivBonusSpeedBox = timeActivBonusSpeedBoxOrigin;
            }
        }
        else if (bonusSlow)
        {
            timeActivBonusSlow -= Time.deltaTime;
            if (timeActivBonusSlow < 0)
            {
                bonusSlow = false;
                timeActivBonusSlow = timeActivBonusSlowOrigin;
            }
        }
        else if (bonusSpeedBall)
        {
            timeActivBonusSpeedBall -= Time.deltaTime;
            if (timeActivBonusSpeedBall < 0)
            {

                bonusSpeedBall = false;
                timeActivBonusSpeedBall = timeActivBonusSpeedBallOrigin;
            }
        }else if (bonusDestroyAllBox)
        {
           
            DestroyAllBox();
        }
       if (bonusTextShow) 
        {

           if(!bonusText.activeInHierarchy)
                bonusText.SetActive(true);
            bonusText.transform.localScale = Vector3.MoveTowards(bonusText.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);

            if(bonusText.transform.localScale == new Vector3(0, 0, 0))
            {
                bonusTextShow = false;
                bonusText.transform.localScale = new Vector3(1, 1, 1);
                bonusText.SetActive(false);
            }
        }
        if (GameManager.isGameOver)
        {
            bonusVector = false;
            bonusDamageBall = false;
            bonusSlow = false;
            bonusSpeedBall = false;
            bonusDestroyAllBox = false;
        }
    }
    private void DestroyAllBox()
    {
        GameObject boxToKill = GameObject.Find("DestroyBox");
        boxToKill.transform.position = Vector3.MoveTowards(boxToKill.transform.position, new Vector3(boxToKill.transform.position.x, boxToKill.transform.position.y, 21), 2f);
        if (boxToKill.transform.position.z >= 20)
        {
            boxToKill.transform.position = new Vector3(1.5f, 1.68f, -8);
            bonusDestroyAllBox = false;
        }
    }

}

