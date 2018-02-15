using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    private enum dir { left, right, count};
    private Vector2 startPos;
    private float groundHeight = -8f;

    public float boundsMinX;
    public float boundsMaxX;

    void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        // If the player goes too far out of bounds, reset him
        if (transform.position.x < boundsMinX || transform.position.x > boundsMaxX)
        {
            transform.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = startPos;
            if (UIEvents.uiEventsScript.RemoveHeart())
                HitTheGround();
        }

        // If the player hits the ground, reverse velocity 
        if (transform.position.y < groundHeight)
        {
            if(UIEvents.uiEventsScript.RemoveHeart())
                HitTheGround();
        }
    }

    /// <summary>
    /// When the player falls to the ground this reverses the velocity to get him back on a branch.
    /// </summary>
    private void HitTheGround()
    {
        Monkey.monkeyScript.TakeDamage = true;
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

        // When there's no x velocity this adds x velocity in a random direction,
        // so the player can grab a branch when there's no branch above them.
        if (vel.x == 0)
        {
            dir ran_dir = (dir)Random.Range(0, 2);
            if (ran_dir == dir.left)
                vel = new Vector2(-2f, vel.y);
            else
                vel = new Vector2(2f, vel.y);
        }

        GetComponent<Rigidbody2D>().velocity = -(vel * 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && UIEvents.uiEventsScript.LoseHeart)
        {
            GetComponent<Rigidbody2D>().velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity * 1.5f;
            other.gameObject.GetComponent<Rigidbody2D>().velocity =
                -other.gameObject.GetComponent<Rigidbody2D>().velocity * 2.5f;
            UIEvents.uiEventsScript.RemoveHeart();
        }
        else if(other.tag == "Bulldozer")
        {
            UIEvents.uiEventsScript.InstantKill();
        }
    }
}
