using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTopScroll : MonoBehaviour {

    public GameObject[] arrow;
    Vector3 tempPos;
    Vector3 targetPos;
    Vector3 tempX;
    public static GameObject tempTopItem;
    float index;
    float oldIndex;
	int countModel;
    float lengthModel;
    public GameObject activSpawn;
    private float hight = 0f;

    void Start () {
        if (transform.name == "Players")
        {
            string s = User.Instance.namePlayer.Substring(User.Instance.namePlayer.Length - 2);
            int qw = Int32.Parse(s);
            transform.position = new Vector3(-2.5f * qw, transform.position.y, transform.position.z);
        }
        else
        {
            int qw = (int)char.GetNumericValue(User.Instance.nameBall[User.Instance.nameBall.Length - 1]);
            transform.position = new Vector3(-2.5f * qw, transform.position.y, transform.position.z);
        }
        countModel = 5;
		lengthModel = (countModel - 1) * 2.5f;
        index = 0;
        oldIndex = 0;
        arrow[0].SetActive(true);
        arrow[1].SetActive(true);
    }

    void Update()
    {
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
            if (ManagerChangePlayer.whichMove == ManagerChangePlayer.whichMoveType.player)
            {
                move();
            }
        }
        if ( !(transform.position.x % 2.5f == 0))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(PositionChange(transform.position.x), transform.position.y, transform.position.z), Time.deltaTime *2);
        }
        TransformZ(transform.position);
    }
    public void ButtonMoveLeft()
    {
        transform.position = new Vector3(transform.position.x+ 2.5f, transform.position.y, transform.position.z);
    }
    public void ButtonMoveRight()
    {
        transform.position = new Vector3(transform.position.x -2.5f, transform.position.y, transform.position.z);
    }
    void move()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        targetPos = new Vector3(Input.mousePosition.x, hight, distance);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        Vector3 followXonly = new Vector3(targetPos.x, transform.position.y, transform.position.z);
        transform.position = followXonly - tempX;
		if (transform.name == "Players") {
			if (transform.position.x <= -lengthModel)
			{
				transform.position = new Vector3(-lengthModel, hight, 0);
			}
			else if (transform.position.x >= 0)
			{
				transform.position = new Vector3(0, hight, 0);
			}
		} else if (transform.name == "Ball") {
			if (transform.position.x <= -5)
			{
				transform.position = new Vector3(-5, hight, 0);
			}
			else if (transform.position.x >= 0)
			{
				transform.position = new Vector3(0, hight, 0);
			}
		}
    }
    float PositionChange(float posX)
    {
        float left = 0;
        float right = 0;
        float length = 0;
		if (transform.name == "Players") 
        {
            length = lengthModel;
		} 
        else if (transform.name == "Ball") 
        {
            length = 5;
		}
        for (float i = 0; i <= length; i += 2.5f)
        {
            if (-posX >= i && -posX < i + 2.5f)
            {
                left = i;
                right = i + 2.5f;
            }
        }
        float sL = -posX - left;
        float sR = right + posX;
        if (sL < sR)
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
        float posTopItem = PositionChange(tempp.x);
        oldIndex = index;
        index = -(posTopItem / 2.5f);
        if(transform.name == "Players")
        {
            ChangePlayer(oldIndex, index);
        }
        else if (transform.name == "Ball")
        {
            ChangeBall(oldIndex,index);
        }
        ControlViewArrow(index);
        if (posTopItem == 0)
        {
            distance = -tempp.x - 1.5f;
        }
        else if (posTopItem < 0)
        {
            if (tempp.x > posTopItem)
            {
                distance = -(1.5f + (posTopItem - tempp.x));
            }
            else
            {
                distance = -tempp.x - (-posTopItem + 1.5f);
            }
        }
        float y = distance;
        if (y < 0)
        {
            y = -y;
        }
        y += 0.5f;
        refreshOtherPosytion();
        if (tempTopItem != null)
        {
            if (transform.name == "Players")
                tempTopItem.transform.position = new Vector3(tempTopItem.transform.position.x, 4 - (y * 1.4f) + hight, distance * 6);
            else
                tempTopItem.transform.position = new Vector3(tempTopItem.transform.position.x, 4 - (y * 1.4f) + hight, (distance * 6) + 1);
        }
    }
    private void ChangePlayer(float oldIndex, float index)
    {
        if (oldIndex != index)
        {
            if (tempTopItem != null)
                try
                {
                    activSpawn = transform.Find(tempTopItem.name + "/Gun/Gun_" + MoveDownScroll.tempDownItem.transform.childCount).gameObject;
                }
                catch (Exception e) { Debug.Log(e); }
            if (activSpawn != null)
                activSpawn.SetActive(false);
            tempTopItem.transform.eulerAngles = new Vector3(-10, 0, 0);
            tempTopItem.transform.Rotate(new Vector3(-14, 0, 0));
        }
        tempTopItem = GameObject.Find("Player_0" + index);
        if (tempTopItem != null)
            if (tempTopItem.tag.Equals("Shop_player") && MoveDownScroll.tempDownItem.transform.childCount > 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (i != MoveDownScroll.tempDownItem.transform.childCount)
                    {
                        Transform tt = transform.Find(tempTopItem.name + "/Gun/Gun_" + i);
                        if (tt != null)
                        {
                            if (tt.gameObject != null)
                            {
                                tt.gameObject.SetActive(false);
                            }
                        }
                    }
                }
                Transform t = transform.Find(tempTopItem.name + "/Gun/Gun_" + MoveDownScroll.tempDownItem.transform.childCount);
                if (t != null)
                {
                    if (t.gameObject != null)
                    {
                        activSpawn = t.gameObject;
                        if (activSpawn != null)
                            activSpawn.SetActive(true);
                    }
                }
            }
        tempTopItem.transform.Rotate(new Vector3(0, 1, 0));
    }
    private void ChangeBall(float oldIndex, float index)
    {
        if (oldIndex != index)
        {
            GameObject temp = GameObject.Find("Ball_" + oldIndex);
            temp.GetComponent<Renderer>().material.color = User.Instance.GetColor(User.Instance.colorBall);
            temp.GetComponent<Renderer>().material.SetColor("_EmissionColor", User.Instance.GetColor(User.Instance.colorBall));
        }
        try
        {
            tempTopItem = GameObject.Find("Ball_" + index);
        }
        catch (Exception e) { Debug.Log(e); }
    }
    private void ControlViewArrow(float index)
    {
        if (index == 0)
        {
            arrow[0].SetActive(false);
        }
        else if (index == countModel - 1 && transform.name == "Players" || index == 2 && transform.name == "Ball")
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
        GameObject[] otherObject = GameObject.FindGameObjectsWithTag("Shop_player");
        foreach (GameObject go in otherObject)
        {
            if (tempTopItem != go)
            {
                go.transform.position = new Vector3(go.transform.position.x, 2.85f + hight, -1.75f);
            }
        }
    }
}
