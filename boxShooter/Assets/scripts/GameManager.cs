using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject panelUpgrade;
    public GameObject buttonSpeedUpGO;
    public GameObject buttonPowerUpGO;
    public Text scoreText;
    public Text hightScoreText;
    public Text lastScoreText;
    public static bool isGameOver;
    public static bool canAdd;
    public static long scoreUp;
    public static float speedBox = 3.5f;
    public Text levelPower;
    public Text levelSpeed;
    public Text allScore;
    bool isFail;
    private void Awake()
    {
        User.Instance.loadUser();
    }
    void Start()
    {
        isFail = false;
        canAdd = true;
        isGameOver = true;
        Refresh();
        gameOver.SetActive(true);
        if (User.Instance.hightScore < 1)
        {
            hightScoreText.enabled = false;
        }
        lastScoreText.enabled = false;
        scoreUp = 0;
        scoreText.text = Conventer(User.Instance.overallScore);
        allScore.text = scoreText.text;
        hightScoreText.text = "Najwyzszy wynik \n" + Conventer(User.Instance.hightScore);
        lastScoreText.text = "Ostatni wynik\n" + Conventer(scoreUp);
    }
    void Update()
    {
        if (isGameOver)
        {
            if (canAdd)
            {
                canAdd = false;
                if (isFail)
                {
                    StartCoroutine(ShowPaneUpgradAfterFail());
                }
            }
            Refresh();
        }
        else
        {
            scoreText.text = Conventer(scoreUp);
        }
    }


    IEnumerator ShowPaneUpgradAfterFail()
    {
        isFail = false;
        yield return new WaitForSeconds(1);

        User.Instance.overallScore += scoreUp;
        scoreText.text = Conventer(User.Instance.overallScore);
        if (User.Instance.hightScore < scoreUp)
        {
            User.Instance.hightScore = scoreUp;
        }
        User.Instance.saveUser();
        hightScoreText.text = "Najwyższy wynik \n" + Conventer(User.Instance.hightScore);
        lastScoreText.text = "Ostatni wynik \n" + Conventer(scoreUp);
        hightScoreText.enabled = true;
        lastScoreText.enabled = true;
        gameOver.SetActive(true);
        if (scoreUp > 0)
        {
            panelUpgrade.SetActive(true);
        }
    }
    public void restart()
    {
        hightScoreText.enabled = false;
        lastScoreText.enabled = false;
        SpawnBox.countOfSpawnedLines = 0;
        scoreUp = 0;
        gameOver.SetActive(false);
        isGameOver = false;
        isFail = true;
        canAdd = true;

    }

    public void SpeedUpButton()
    {
        User.Instance.buySpeedUp();
        Refresh();
    }
    public void PowerUpButton()
    {
        User.Instance.buyPowerUp();
        Refresh();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public void ShopButton()
    {
        SceneManager.LoadScene("Shop");
    }

    public void Upgrade()
    {

        panelUpgrade.SetActive(true);
        Refresh();
    }
    void Refresh()
    {
        if (User.Instance.overallScore >= User.Instance.getPowerUpCost())
        {
            buttonPowerUpGO.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonPowerUpGO.GetComponent<Button>().interactable = false;
        }
        if (User.Instance.overallScore >= User.Instance.getSpeedUpCost())
        {
            buttonSpeedUpGO.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonSpeedUpGO.GetComponent<Button>().interactable = false;
        }
        levelSpeed.text = "Szybkosc\n strzalow\n Level\n" + User.Instance.speedLevel.ToString() + "\n\n" + Conventer(User.Instance.getSpeedUpCost());
        levelPower.text = "Moc\n strzalow\n Level\n" + User.Instance.powerLevel.ToString() + "\n\n" + Conventer(User.Instance.getPowerUpCost());
        allScore.text = Conventer(User.Instance.overallScore);

    }
    public void ExitUpgradeButton()
    {

        panelUpgrade.SetActive(false);
    }
    public string Conventer(long sc)
    {
        string scT = "";
        if (sc >= 10000 && sc < 1000000)
        {
            scT = (sc / 1000).ToString() + "." + ((sc % 1000) / 100).ToString() + "K";
        }
        else if (sc >= 1000000)
        {
            scT = (sc / 1000000).ToString() + "MLN";
        }
        else
        {
            scT = sc.ToString();
        }
        if (isGameOver)
        {
            scT += " pts";
        }

        return scT;

    }

}
