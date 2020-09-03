using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBox : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals(BoxScript.sBox) || collision.transform.tag.Equals(BoxScript.sMoving_Box) || collision.transform.tag.Equals("BonusP") || collision.transform.tag.Equals(BoxScript.sDownUp_Box)) //wszystkiePudelka|| collision.transform.tag.Equals(BoxScript.sEmptyBox)
        {
            Destroy(collision.gameObject);
        }
    }
}
