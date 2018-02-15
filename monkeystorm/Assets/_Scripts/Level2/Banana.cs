using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Consumable")
        {
            Debug.Log("overlapping consumables");
        }
    }
}
