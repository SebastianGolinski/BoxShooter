using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallScript : MonoBehaviour
{
    Vector3 temp;
    string sceneName;
    float distanceToDestroy;
    static string Shop = "Shop";
    void Start()
    {
        distanceToDestroy = 2;
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        temp = transform.position;
    }

    void Update()
    {
        if (transform.position.z > 15 || GameManager.isGameOver && !sceneName.Equals(Shop))
        {
            Destroy(transform.gameObject);
        }
        else if (sceneName.Equals(Shop))
        {
            if (temp.x < transform.position.x - distanceToDestroy || temp.x > transform.position.x + distanceToDestroy || temp.z < transform.position.z - distanceToDestroy || temp.z > transform.position.z + distanceToDestroy)
            {

                Destroy(transform.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        Ray loadnigRay = new Ray(transform.position, Vector3.forward);
        Ray loadnigRayL = new Ray(transform.position, Vector3.left);
        Ray loadnigRayR = new Ray(transform.position, Vector3.right);

        if (Physics.Raycast(loadnigRay, out hit, 0.4f) || Physics.Raycast(loadnigRayL, out hit, 0.1f) || Physics.Raycast(loadnigRayR, out hit, 0.1f))
        {
            if (hit.collider.tag.Equals(BoxScript.sBox) || hit.transform.tag.Equals(BoxScript.sMoving_Box) || hit.transform.tag.Equals(BoxScript.sDownUp_Box))
            {

                BoxScript bs = hit.transform.GetComponent<BoxScript>();
                AddPiontToScore(bs);
                Destroy(transform.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag.Equals(BoxScript.sBox) || collision.transform.tag.Equals(BoxScript.sMoving_Box) || collision.transform.tag.Equals(BoxScript.sDownUp_Box)) 
        {  
            BoxScript bs = collision.transform.GetComponent<BoxScript>();
            AddPiontToScore(bs);
            Destroy(transform.gameObject);
        }
    }
    private void AddPiontToScore(BoxScript bs)
    {
        if (bs.pointNumber >= 0)
        {
            if (bs.pointNumber < User.Instance.powerLevel)
                GameManager.scoreUp += bs.pointNumber;
            else
                GameManager.scoreUp += User.Instance.powerLevel;

            if (BonusManager.getbonusDamageBall())
            {
                bs.pointNumber -= User.Instance.powerLevel * 2;
                bs.color();
            }
            else
            {
                bs.pointNumber -= User.Instance.powerLevel;
                bs.color();
            }
        }
    }
 

}
