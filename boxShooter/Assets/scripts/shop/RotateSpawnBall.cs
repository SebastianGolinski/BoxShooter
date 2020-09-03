using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpawnBall : MonoBehaviour {

	private Vector3 standardDirection;
	public static RotateSpawnBall instance;
	float timeToBall;
	float timeSpawnBall;
	float x,z;
	bool isNew;
	private static string tempName;
	private static Vector3 tempDir;

	void Start () {
		instance = this;
		tempName = MoveTopScroll.tempTopItem.name;
		isNew = false;
		x = 0;
		z = 10;
		standardDirection = new Vector3(x, 0, z);
		tempDir = standardDirection;
		timeToBall = 0.1f;
		timeSpawnBall = timeToBall;
        
	}

    private void OnDisable()
    {
        isNew = true;
    }

    void Update () {
        if (MoveTopScroll.tempTopItem.name != tempName && !isNew)
            isNew = true;

		if (isNew)
        {
			tempDir = new Vector3(0, 0, 10);
			GameObject[] spawnBall = GameObject.FindGameObjectsWithTag ("SpawnBallT");
			foreach (GameObject current in spawnBall) {
				current.GetComponent<RotateSpawnBall>().standardDirection = tempDir;
			}
			tempName = MoveTopScroll.tempTopItem.name;
            isNew = false;
		}
        standardDirection = (transform.parent.parent.parent.position - transform.position);
        if (timeToBall < 0)
		{

			SpawnBall(-standardDirection);
			timeToBall = timeSpawnBall;
		}
		timeToBall -= Time.deltaTime;
	}
	private void SpawnBall(Vector3 temp)
	{
		MakeInstanteBall (new Vector3 (transform.position.x, transform.position.y, transform.position.z), temp);
    }
	void MakeInstanteBall(Vector3 tempPositon, Vector3 tempV)
	{
		GameObject prefab = (GameObject)Instantiate(Resources.Load("BoxShooter/" + User.Instance.nameBall, typeof(GameObject)) as GameObject);
        prefab.GetComponent<Renderer>().material.color = User.Instance.GetColor(User.Instance.colorBall);
		prefab.GetComponent<Renderer>().material.SetColor("_EmissionColor", User.Instance.GetColor(User.Instance.colorBall));
		prefab.transform.position = tempPositon;
		prefab.GetComponent<Rigidbody>().velocity = 10 * ( tempV.normalized);
	}
}
