using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewBallShop : MonoBehaviour
{
    public Transform[] place;
    private void Start()
    {
        NewBall();
    }

    private void OnEnable()
    {
        NewBall();
    }
    public void NewBall()
    {
        foreach (Transform placeSpawn in transform.GetComponentInChildren<Transform>())
        {
            foreach (Transform ps in placeSpawn.GetComponentInChildren<Transform>())
            {
               Destroy(ps.gameObject);
            }

        }
        foreach (Transform t in place)
        {
               MakeInstanteBall(t.transform.position, t);
        }
    }
    void MakeInstanteBall(Vector3 tempPositon, Transform t)
    {
        GameObject prefab = (GameObject)Instantiate(Resources.Load("BoxShooter/" + User.Instance.nameBall, typeof(GameObject)) as GameObject);
        prefab.GetComponent<BallScript>().enabled = false;
        prefab.transform.tag ="Ball_Gun";
        prefab.GetComponent<Renderer>().material.color = User.Instance.GetColor(User.Instance.colorBall);
        prefab.GetComponent<Renderer>().material.SetColor("_EmissionColor", User.Instance.GetColor(User.Instance.colorBall));

        prefab.transform.position = tempPositon;
        prefab.transform.SetParent(t);
        prefab.GetComponent<Rigidbody>().isKinematic = true;
    }
}
