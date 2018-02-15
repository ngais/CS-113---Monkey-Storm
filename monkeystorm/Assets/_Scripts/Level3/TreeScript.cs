using UnityEngine;
using System.Collections;

public class TreeScript : MonoBehaviour {

    public GameObject[] branches;
    public GameObject forcePt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bulldozer")
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(-5, 1), forcePt.transform.position);
            foreach (GameObject branch in branches)
            {
                if (branch.tag == "Darkness")
                    branch.gameObject.SetActive(false);
                else
                {
                    branch.transform.parent = null;
                    branch.GetComponent<Rigidbody2D>().isKinematic = false;
                    branch.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
}
