using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeColor : MonoBehaviour {

	void Start () {
        transform.GetComponent<Renderer>().material.color = transform.parent.GetComponent<Renderer>().material.color;
        transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", transform.parent.GetComponent<Renderer>().material.color);
    }

    void Update () {
        if(SceneManager.GetActiveScene().name =="Shop")
        transform.GetComponent<Renderer>().material.color = transform.parent.GetComponent<Renderer>().material.color;
        transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", transform.parent.GetComponent<Renderer>().material.color);
    }
}
