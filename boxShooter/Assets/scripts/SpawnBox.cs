using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnBox : MonoBehaviour
{
    public GameObject box;
    public GameObject bonus;
    public GameObject emptyBox;
    public static int countOfSpawnedLines = 1;
    bool isNewGame;
    GameObject[] toDelete;
    GameObject[] toDelete2;
    GameObject[] toDelete3;
    GameObject[] toDelete4;
    GameObject[] toDelete5;
    GameObject[] playerGO;
    int zSpawnBox = 19;
    public static int sizeLine;
    bool upSizeLine;
    bool transformMap;
    GameObject[] background;
    GameObject cam;
    Vector3 sizeOriginalBackground;
    Vector3 posOriginalCamera;
    Vector3 newPos;
    void Start()
    {
        sizeLine = 4;
        isNewGame = true;
        upSizeLine = false;
        transformMap = false;
        countOfSpawnedLines = 1;
        cam = GameObject.Find("Main Camera");
        background = GameObject.FindGameObjectsWithTag("Background");
    }
    void Update()
    {
        if (!GameManager.isGameOver && isNewGame)
        {
            sizeLine = 4;
            ClearBox();
            SpawnPlayer();
            Spawnbox();
            
            isNewGame = false;
            foreach (GameObject b in background)
            {
                b.transform.localScale = new Vector3(0.67f, b.transform.localScale.y, b.transform.localScale.z);
            }
            cam.transform.position = new Vector3(1.05f, 2.84f, -3.2f);
            upSizeLine = false;
            transformMap = false;
        }
        if (GameManager.isGameOver && !isNewGame)
        {
            isNewGame = true;
            
        }
            
        if(countOfSpawnedLines % 15 == 0 && !transformMap && !GameManager.isGameOver && sizeLine < 7)
        {
            upSizeLine = true;
            if (CheckToUpSize())
            {
                sizeOriginalBackground = background[0].transform.localScale;
                posOriginalCamera = cam.transform.position;
                newPos = new Vector3(posOriginalCamera.x + 0.5f, cam.transform.position.y, cam.transform.position.z - 0.9f);
                transformMap = true;
            }
        }
        if (transformMap && upSizeLine)
        {
            UpSizeLine();
        }
    }
    private bool CheckToUpSize()
    {
        toDelete = GameObject.FindGameObjectsWithTag(BoxScript.sBox);
        toDelete2 = GameObject.FindGameObjectsWithTag("BonusP");
        toDelete3 = GameObject.FindGameObjectsWithTag(BoxScript.sMoving_Box);
        toDelete4 = GameObject.FindGameObjectsWithTag(BoxScript.sDownUp_Box);
        int countBoxOnGame = toDelete.Length + toDelete2.Length + toDelete3.Length + toDelete4.Length;
        if (countBoxOnGame <= 0)
        {
            return true;
        }
        return false;
    }
    private void UpSizeLine()
    {
        foreach(GameObject b in background)
        {
            if(b.transform.localScale != new Vector3(sizeOriginalBackground.x+0.17f, b.transform.localScale.y, b.transform.localScale.z))
                b.transform.localScale += new Vector3(0.01f, 0, 0);
        }
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, newPos, 0.07f);
        if (cam.transform.position == newPos && background[0].transform.localScale == new Vector3(sizeOriginalBackground.x + 0.17f, background[0].transform.localScale.y, background[0].transform.localScale.z))
        {
            GameObject.Find("stoper/RightBox").transform.position += new Vector3(1, 0, 0);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().endRightMove += 1;
            countOfSpawnedLines++;
            sizeLine++;
            upSizeLine = false;
            transformMap = false;
        }
    }
    private void SpawnEmptyBox()
    {
        countOfSpawnedLines++;
        GameObject prefab = (GameObject)Instantiate(emptyBox);
        prefab.transform.position = new Vector3(-0.5f, 0.5f, zSpawnBox);
        prefab.GetComponent<Rigidbody>().velocity = -10 * (new Vector3(0, 0, 1).normalized);
    }
    private void Spawnbox()
    {
        SpawnEmptyBox();
        for (int x = 0; x < sizeLine; x++)
        {
            GameObject prefab = (GameObject)Instantiate(box);
            prefab.transform.position = new Vector3(x - 0.5f, 0.5f, zSpawnBox);
            prefab.GetComponent<Rigidbody>().velocity = -10 * (new Vector3(0, 0, 1).normalized);
        }
    }
    private void SpawnMovingbox(float x, float y, float z)
    {
        GameObject prefab = (GameObject)Instantiate(box);
        prefab.transform.position = new Vector3(x - 0.5f, y, zSpawnBox + z);
        prefab.gameObject.tag = BoxScript.sMoving_Box;
        prefab.GetComponent<Rigidbody>().velocity = -10 * (new Vector3(0, 0, 1).normalized);
    }
    private void Spawnbonus()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        GameObject prefab = (GameObject)Instantiate(bonus);
        prefab.transform.position = new Vector3(Random.Range(0, sizeLine-1) - 0.5f, 0.5F, zSpawnBox - 4); 
        prefab.GetComponent<Rigidbody>().velocity = -10 * (new Vector3(0, 0, 1).normalized);
    }
    private void SpawnUpLine(float y)
    {
        for (int x = 0; x < sizeLine; x++)
        {
            GameObject prefab = (GameObject)Instantiate(box);
            prefab.transform.position = new Vector3(x - 0.5f, y, zSpawnBox);
            prefab.GetComponent<Rigidbody>().velocity = -10 * (new Vector3(0, 0, 1).normalized);
        }
    }

    [System.Obsolete]
    private void ChangeSpeedBox()
    {
        if (countOfSpawnedLines < 1)
            GameManager.speedBox = 3.5f;
        else if (countOfSpawnedLines < 4 && User.Instance.powerLevel > 1 && User.Instance.speedLevel > 1)
            GameManager.speedBox = 2.5f;
        else if (countOfSpawnedLines < 21)
            GameManager.speedBox = 3.5f;
        else
            GameManager.speedBox += 0.005f;
    }
    private void CreateBoxLine_30_100()
    {
        int whichMod = Random.RandomRange(0, 8);
        switch (whichMod)
        {
            case 0:
                Spawnbox();
                SpawnUpLine(1.5f);
                break;
            case 1:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnMovingbox(0, 2.5f, 0);
                break;
            case 2:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnMovingbox(3, 2.5f, 0);
                break;
            case 3:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                break;
            case 4:
                SpawnEmptyBox();
                SpawnMovingbox(0, 0.5f, 2);
                SpawnMovingbox(3, 0.5f, 1);
                break;
            case 5:
                SpawnEmptyBox();
                SpawnMovingbox(3, 0.5f, 1);
                SpawnMovingbox(0, 0.5f, 2);
                break;
            case 6:
                SpawnEmptyBox();
                SpawnMovingbox(3, 0.5f, -1);
                SpawnMovingbox(0, 0.5f, 0);
                SpawnMovingbox(3, 0.5f, 1);
                SpawnMovingbox(0, 0.5f, 2);
                break;
            case 7:
                SpawnEmptyBox();
                SpawnMovingbox(0, 0.5f, -1);
                SpawnMovingbox(1, 0.5f, 0);
                SpawnMovingbox(2, 0.5f, 1);
                SpawnMovingbox(3, 0.5f, 2);
                break;
        }
    }
    private void CreateBoxLine_100()
    {
        int whichMod = Random.RandomRange(0, 7);
        switch (whichMod)
        {
            case 0:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnMovingbox(0, 2.5f, 0);
                break;
            case 1:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnMovingbox(3, 2.5f, 0);
                break;
            case 2:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                SpawnMovingbox(0, 3.5f, 0);
                break;
            case 3:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                SpawnMovingbox(3, 3.5f, 0);
                break;
            case 4:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                SpawnUpLine(3.5f);
                SpawnUpLine(4.5f);
                break;
            case 5:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                SpawnUpLine(3.5f);
                SpawnMovingbox(3, 4.5f, 0);
                break;
            case 6:
                Spawnbox();
                SpawnUpLine(1.5f);
                SpawnUpLine(2.5f);
                SpawnUpLine(3.5f);
                SpawnMovingbox(0, 4.5f, 0);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(BoxScript.sEmptyBox))
        {
            Destroy(other.transform.gameObject);
            ChangeSpeedBox();

            Random.InitState(System.DateTime.Now.Millisecond);
            if (!upSizeLine)
            {
                if (countOfSpawnedLines < 3 && User.Instance.powerLevel > 1 && User.Instance.speedLevel > 1) 
                {
                    Spawnbox();
                }
                else
                {
                    if (countOfSpawnedLines % 2 == 0 && countOfSpawnedLines < 30 && countOfSpawnedLines > 3)
                    {
                        int whichMod = Random.RandomRange(0, 3);
                        switch (whichMod)
                        {
                            case 0:
                                Spawnbox();
                                SpawnMovingbox(3, 1.5f, 0);
                                break;
                            case 1:
                                Spawnbox();
                                SpawnUpLine(1.5f);
                                break;
                            case 2:
                                Spawnbox();
                                SpawnMovingbox(0, 1.5f, 0);
                                break;
                        }
                    }
                    else if (countOfSpawnedLines < 100 && countOfSpawnedLines >= 30)
                    {
                        CreateBoxLine_30_100();
                    }
                    else if (countOfSpawnedLines >= 100)
                    {
                        CreateBoxLine_100();
                    }
                    else
                    {
                        Spawnbox();
                    }
                }
                if (countOfSpawnedLines % Random.Range(3,5) == 0)
                {
                    Spawnbonus();
                }
            }
            else
            {
                SpawnEmptyBox();
                countOfSpawnedLines--;
            }
        }
    }

    void ClearBox()
    {
        toDelete = GameObject.FindGameObjectsWithTag(BoxScript.sBox);
        toDelete2 = GameObject.FindGameObjectsWithTag("BonusP");
        toDelete3 = GameObject.FindGameObjectsWithTag(BoxScript.sMoving_Box);
        toDelete4 = GameObject.FindGameObjectsWithTag(BoxScript.sDownUp_Box);
        toDelete5 = GameObject.FindGameObjectsWithTag(BoxScript.sEmptyBox);
        for (int i = 0; i < toDelete.Length; i++)
        {
            Destroy(toDelete[i]);
            toDelete[i] = null;
        }
        for (int i = 0; i < toDelete2.Length; i++)
        {
            Destroy(toDelete2[i]);
            toDelete2[i] = null;
        }
        for (int i = 0; i < toDelete3.Length; i++)
        {
            Destroy(toDelete3[i]);
            toDelete3[i] = null;
        }
        for (int i = 0; i < toDelete4.Length; i++)
        {
            Destroy(toDelete4[i]);
            toDelete4[i] = null;
        }
        for (int i = 0; i < toDelete5.Length; i++)
        {
            Destroy(toDelete5[i]);
            toDelete5[i] = null;
        }
    }
    void SpawnPlayer()
    {
        playerGO = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerGO.Length; i++)
        {
            Destroy(playerGO[i]);
        }
        GameObject prefab = (GameObject)Instantiate(Resources.Load("BoxShooter/" + User.Instance.namePlayer, typeof(GameObject)) as GameObject);
        prefab.transform.position = new Vector3(1.05f, 0.3f, 0);
    }
}
