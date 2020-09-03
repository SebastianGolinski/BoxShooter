using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject ball_0;
    public GameObject ball_1;
    public GameObject ball_2;

    Vector3 targetPos;
    Vector3 TempPos;
    
    Vector3 standardDirection;
    Vector3 bonusDirectionL;
    Vector3 bonusDirectionR;
    Vector3 tempX;

    public float endRightMove;
    private GameObject spawnBallplace;
    float nextShoot;
    float timeSpawnBall;
    private float speedMove = 1.5f;
    public GameObject effectBox;
    void Start()
    {
        endRightMove = 2.65f;
        targetPos = new Vector3(4, transform.position.y, transform.position.z);
        standardDirection = new Vector3(0, 0, 10);
        bonusDirectionL = new Vector3(-2, 0, 10);
        bonusDirectionR = new Vector3(2, 0, 10);
        nextShoot = 0f;
        setBallTime(1);
        spawnBallplace = transform.Find("Gun/Gun_" + User.Instance.countShotPlayer).gameObject;
    }

    float setBallTime(int multiply) {
        float lvl = User.Instance.speedLevel - 1;
        if (lvl <= 20)
        {
            timeSpawnBall = 0.4f - (0.0125f * lvl);
        }
        else
        {
            timeSpawnBall = 0.4f - ((0.025f * 10f) + ((lvl - 10f) * 0.0088f)* multiply);
        }
        if (timeSpawnBall < 0.0f)
            timeSpawnBall = 0.0f;
        
        return timeSpawnBall;
    }

    void Update()
    {

        if(!GameManager.isGameOver && Time.time > nextShoot)
        {
            nextShoot = Time.time + timeSpawnBall;
            if (BonusManager.getbonusVector())
            {
                SpawnBall(bonusDirectionR);
                SpawnBall(bonusDirectionL);
            }
            else
            {
                SpawnBall(standardDirection);

                if (BonusManager.getbonusDamageBall())
                    setBallTime(4);
                else
                    setBallTime(1);

            }
        }
        if (GameManager.isGameOver )
        {
                Destroy(gameObject);
                Instantiate(effectBox, transform.position, Quaternion.identity);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.isGameOver)
            {
                TempPos = transform.position;
                float distance = transform.position.z - Camera.main.transform.position.z;
                targetPos = new Vector3(Input.mousePosition.x, 0, distance);
                targetPos = Camera.main.ScreenToWorldPoint(targetPos);
                Vector3 followXonly = new Vector3(targetPos.x * speedMove, transform.position.y, transform.position.z);
                tempX = followXonly - TempPos;

            }

        }
            if (Input.GetMouseButton(0) &&  !GameManager.isGameOver)
            {
                move();
            } 
    }

   
    private void SpawnBall(Vector3 temp)
    {
        foreach (Transform placeSpawn in spawnBallplace.GetComponentInChildren<Transform>())
        {
            MakeInstanteBall(placeSpawn.transform.position, temp);
        }
    }
    void move()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        targetPos = new Vector3(Input.mousePosition.x, 0, distance);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        Vector3 followXonly = new Vector3(targetPos.x * speedMove, transform.position.y, transform.position.z);
        transform.position = followXonly - tempX;
        if (transform.position.x < -0.5f)
        {
            transform.position = new Vector3(-0.5f, 0.3f, 0);
        }
        else if (transform.position.x > endRightMove)
        {
            transform.position = new Vector3(endRightMove, 0.3f, 0);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals(BoxScript.sBox) || collision.transform.tag.Equals(BoxScript.sMoving_Box) || collision.transform.tag.Equals(BoxScript.sDownUp_Box)) //WsyztkiePudelka
        {
            GameManager.isGameOver = true;
        }
    }

 
    void MakeInstanteBall(Vector3 tempPositon, Vector3 tempV)
    {
        GameObject ball = ball_0;
        if (User.Instance.nameBall == ball_0.transform.name)
            ball = ball_0;
        else if (User.Instance.nameBall == ball_1.transform.name)
            ball = ball_1;
        else if (User.Instance.nameBall == ball_2.transform.name)
            ball = ball_2;

        GameObject prefab = (GameObject)Instantiate(ball);
        prefab.GetComponent<Renderer>().material.color = User.Instance.GetColor(User.Instance.colorBall);
        prefab.GetComponent<Renderer>().material.SetColor("_EmissionColor", User.Instance.GetColor(User.Instance.colorBall));
        prefab.transform.position = tempPositon;
        prefab.GetComponent<Rigidbody>().velocity = 10 * (tempV.normalized);
    }
   
}
