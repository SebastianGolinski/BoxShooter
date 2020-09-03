using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour
{
    public static string sBox = "Box";
    public static string sMoving_Box = "Moving_Box";
    public static string sDownUp_Box = "DownUp_Box";
    public static string sEmptyBox = "EmptyBox";

    bool wait;
    private Rigidbody rb;
    public GameObject effectBox;
    GameObject numberGO;
    public GameObject number;
    Vector3 followXonly;
    int colorIndex;
    int colorIndexSave;
    float speedBox;
    public long pointNumber;
    bool first;
    bool downTransform;

    public static long randomPoint = 1;
    public static System.Random rand = new System.Random();// randomPoint = 1;

    float moveXY = -1.5f;   //Do transformacji golra/dol/lewo/prawo
    float tempY;        //Do transformacji opadanie klocka
    public Material e1;
    public Material e2;
    public Material e3;
    public Material e4;
    public Material c1;
    public Material c2;
    public Material c3;
    public Material c4;
    private Renderer rendererMain;
    private float range;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        downTransform = false;
        colorIndex = 1;
        colorIndexSave = 1;
        rendererMain = gameObject.GetComponent<Renderer>();
        
        range = ((SpawnBox.countOfSpawnedLines / 5) * getCurrentCounterLvl());
        if (range <= 0)
        {
            range = getCurrentCounterLvl();
        }
        range += range * 1.1f * (User.Instance.countShotPlayer - 1);
        pointNumber = (long)(rand.Next(( 1 ), (int)range));
        pointNumber += (int)((SpawnBox.countOfSpawnedLines * (getCurrentCounterLvl())) / 100);
        
        
        if (transform.tag.Equals(BoxScript.sBox) || transform.tag.Equals(BoxScript.sMoving_Box) || transform.tag.Equals(BoxScript.sDownUp_Box))
        {
            number.GetComponent<TextMeshProUGUI>().SetText("temp");           
            first = true;
            color();
        }
        speedBox = GameManager.speedBox;
        wait = false;
        StartCoroutine(waitForSpawn());
    }
    IEnumerator waitForSpawn()
    {
        yield return new WaitForSeconds(1);
        wait = true;
    }

    void Update()
    {
     
        if (!GameManager.isGameOver)
        {
            if (transform.tag.Equals(BoxScript.sMoving_Box))
            {
                if (transform.position.x >= SpawnBox.sizeLine - 1.5f)
                {
                    moveXY = -1.5f;
                }
                else if (transform.position.x <= -0.5f)
                {
                    moveXY = SpawnBox.sizeLine + 0.5f;
                }
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveXY, transform.position.y, transform.position.z), 0.015f);
            }
            else if (transform.tag.Equals(BoxScript.sDownUp_Box))
            {
                if (transform.position.y >= 4.5f)
                {
                    moveXY = -2.5f;
                }
                else if (transform.position.y <= 0.5f)
                {
                    moveXY = 6.5f;
                }
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, moveXY, transform.position.z), 0.015f);
            }

            rb.velocity = -speedBox * (new Vector3(0, 0, 1).normalized);
            if (BonusManager.getbonusSlow())
            {
                speedBox = BonusManager.slowOn;
            }
            else
            {
                speedBox = GameManager.speedBox;
            }

        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z), 0);
        }
        if (pointNumber <= 0 ||BonusManager.getbonusDestroyAllBox())
        {
            if(!transform.tag.Equals(sEmptyBox))
                finishBox();
        }

    }
    private void FixedUpdate()
    {
        bool isBox = transform.tag.Equals(BoxScript.sBox) || transform.tag.Equals(BoxScript.sMoving_Box) || transform.tag.Equals(BoxScript.sDownUp_Box);
        if (isBox && wait)
        {
            float temp = (int)(transform.position.x * 10);
            temp /= 10;
            if (transform.position.y > 1 && !downTransform && temp % 0.5f == 0 && temp % 1 != 0 && !transform.tag.Equals(BoxScript.sDownUp_Box))
            {
                RaycastHit hit;
                Ray loadnigRay = new Ray(transform.position, Vector3.down);
                if (!Physics.Raycast(loadnigRay, out hit, 1))
                {
                    tempY = transform.position.y - 1;
                    transform.gameObject.tag = BoxScript.sBox;
                    downTransform = true;
                }
            }
            if (downTransform)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(temp, tempY, transform.position.z), 0.5f);
                if (transform.position.y <= tempY + 0.01f)
                {
                    transform.position = new Vector3(transform.position.x, tempY, transform.position.z);
                    downTransform = false;
                }
            }
        }
    }
    public long getCurrentCounterLvl()
    {
        long c = 1 + (User.Instance.powerLevel / 5) + ((User.Instance.powerLevel * User.Instance.speedLevel / 4) / 2);
        return c;
    }

    void finishBox()
    {
        if (BonusManager.getbonusDestroyAllBox())
            GameManager.scoreUp += pointNumber ;

        GameObject effect = effectBox;
        if (effectBox != null)
        {
            if (colorIndexSave == 1)
            {
                effect.GetComponent<Renderer>().material = e1;
            }
            else if (colorIndexSave == 2)
            {
                effect.GetComponent<Renderer>().material = e2;
            }
            else if (colorIndexSave == 3)
            {
                effect.GetComponent<Renderer>().material = e3;
            }
            else
            {
                effect.GetComponent<Renderer>().material = e4;
            }
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        Destroy(transform.gameObject);
    }   
    public void color(){
        number.GetComponent<TextMeshProUGUI>().SetText(pointNumber.ToString());
        float cunter = getCurrentCounterLvl();
        float percent = (pointNumber - cunter) / range;
        if ((pointNumber == 1 ||percent < 0.25) && rendererMain.material != c1)
        {
            rendererMain.material = c1;
            colorIndex = 1;
        }
        else if (percent <  0.5 && rendererMain.material != c2)
        {
            rendererMain.material = c2;
            colorIndex = 2;
        }
        else if (percent < 0.75 && rendererMain.material != c4)
        {
            rendererMain.material = c3;
            colorIndex = 3;
        }
        else if (rendererMain.material != c4)
        {
            rendererMain.material = c4;
            colorIndex = 4;
        }
        if (first)
        {
            colorIndexSave = colorIndex;
            first = false;
        }
    }
}
