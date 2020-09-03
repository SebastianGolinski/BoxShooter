using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxEffect : MonoBehaviour {
    float czas = 3f;
    void Update()
    {
        if (czas < 0)
        {
            
            Destroy(transform.gameObject);
        }
        czas -= Time.deltaTime;
    }
}
