using UnityEngine;
using System.Collections;

public class Bulldozer : MonoBehaviour {

    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
    }
}
