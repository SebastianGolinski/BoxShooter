using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownScroll : MonoBehaviour {
    public GameObject[] arrow;

    private Vector3 tempPos;
    private Vector3 targetPos;
    private Vector3 tempX;
    public static GameObject tempDownItem;
    ManagerChangePlayer mcp;
    private float hight = -3.5f;
    float length;
    void Start()
    {
        mcp = GameObject.Find("Manager").GetComponent<ManagerChangePlayer>();
        if (transform.name == "ColorPlayers")
        {
            int i = User.Instance.countShotPlayer-1;
             transform.position = new Vector3(-2.5f * i, transform.position.y, transform.position.z);
        }
        else if (transform.name == "ColorBall")
        {
            int i = mcp.GetIndexBallColorBall();
            transform.position = new Vector3(-2.5f * i, transform.position.y, transform.position.z);
        }
        arrow[0].SetActive(true);
        arrow[1].SetActive(true);
    }

    void Update()
    {
        if (transform.name == "ColorPlayers")
            length = (User.Instance.Player_Gun.Count - 1) * 2.5f;
        else
            length = (User.Instance.listBallColor.Count - 1) * 2.5f;

        if (Input.GetMouseButtonDown(0))
        {
            tempPos = transform.position;
            float distance = transform.position.z - Camera.main.transform.position.z;
            targetPos = new Vector3(Input.mousePosition.x, 0, distance);
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            Vector3 followXonly = new Vector3(targetPos.x, transform.position.y, transform.position.z);
            tempX = followXonly - tempPos;
        }
        if (Input.GetMouseButton(0))
        {
            if ( ManagerChangePlayer.whichMove != ManagerChangePlayer.whichMoveType.player)
            {
                move();
            }
        }
        if (!(transform.position.x % 2.5f == 0))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(PositionChange(transform.position.x), transform.position.y, transform.position.z), Time.deltaTime * 2);
        }
        TransformZ(transform.position);
    }
    public void ButtonMoveLeft()
    {
        transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
    }
    public void ButtonMoveRight()
    {
        transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z);
    }
    void move()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        targetPos = new Vector3(Input.mousePosition.x, hight, distance);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        Vector3 followXonly = new Vector3(targetPos.x, transform.position.y, transform.position.z);
        transform.position = followXonly - tempX;
        if (transform.position.x <= -length)
        {
            transform.position = new Vector3(-length, hight, 0);

        }
        else if (transform.position.x >= 0)
        {
            transform.position = new Vector3(0, hight, 0);
        }
    }
    float PositionChange(float posX)
    {
        float left = 0;
        float right = 0;
        for (float i = 0; i <= length; i += 2.5f)
        {
            if (-posX >= i && -posX < i + 2.5f)
            {
                left = i;
                right = i + 2.5f;
            }
        }
        float wynikL = -posX - left;
        float wynikR = right + posX;
        if (wynikL < wynikR)
        {
            return -left;
        }
        else
        {
            return -right;
        }
    }
    void TransformZ(Vector3 tempp)
    {
        float distance = 0;
        float posPlayer = PositionChange(tempp.x);
        float index = -(posPlayer / 2.5f);
        if(transform.name == "ColorPlayers")
        {
            tempDownItem = GameObject.Find("Gun_" + index);
        }
        else if(transform.name == "ColorBall")
        {
            tempDownItem = GameObject.Find("ColorBall_" + index);
        }
        ControlViewArrow(index);
        if (posPlayer == 0)
        {
            distance = -tempp.x - 1.5f;
        }
        else if (posPlayer < 0)
        {
            if (tempp.x > posPlayer)
            {
                distance = -(1.5f + (posPlayer - tempp.x));
            }
            else
            {
                distance = -tempp.x - (-posPlayer + 1.5f);
            }
        }
        float y = distance;
        if (y < 0)
        {
            y = -y;
        }
        y -= 0.5f;
        refreshOtherPosytion();
        if (transform.name == "ColorPlayers")
            tempDownItem.transform.position = new Vector3(tempDownItem.transform.position.x,   (y * 2.0f)-2.5f, distance * 6);
        else
            tempDownItem.transform.position = new Vector3(tempDownItem.transform.position.x, (y * 2.0f) - 2.5f, (distance * 6) +0.5f);
        if (transform.name == "ColorBall")
            if (MoveTopScroll.tempTopItem != null && tempDownItem != null) 
            {
                if (MoveTopScroll.tempTopItem.GetComponent<Renderer>().material.color != tempDownItem.GetComponent<Renderer>().material.color)
                {
                    MoveTopScroll.tempTopItem.GetComponent<Renderer>().material.color = tempDownItem.GetComponent<Renderer>().material.color;
                    MoveTopScroll.tempTopItem.GetComponent<Renderer>().material.SetColor("_EmissionColor", tempDownItem.GetComponent<Renderer>().material.color);
                    MoveTopScroll.tempTopItem.transform.eulerAngles = new Vector3(0, 0, 0);
                    MoveTopScroll.tempTopItem.transform.Rotate(new Vector3(-14, 0, 0));
                }
                MoveTopScroll.tempTopItem.transform.Rotate(new Vector3(0, 1, 0));
            } 
    }
    private void ControlViewArrow(float index)
    {
        if (index == 0)
        {
            arrow[0].SetActive(false);
        }
        else if (index == 4 && transform.name == "ColorPlayers" || index == 3 && transform.name == "ColorBall")
        {
            arrow[1].SetActive(false);
        }
        else
        {
            arrow[0].SetActive(true);
            arrow[1].SetActive(true);
        }
    }
    void refreshOtherPosytion()
    {
        GameObject[] otherObject;
        if (transform.name == "ColorPlayers")
            otherObject = GameObject.FindGameObjectsWithTag("Shop_Gun");
        else
            otherObject = GameObject.FindGameObjectsWithTag("Shop_color");

        foreach (GameObject go in otherObject)
        {
            if(tempDownItem != go)
            {
                go.transform.position = new Vector3(go.transform.position.x, hight+0.5f, -1.924f);
            }
        }  
    }
}
