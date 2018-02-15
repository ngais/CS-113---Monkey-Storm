using UnityEngine;
using System.Collections;

public class EnemyA : MonoBehaviour {

    private enum dir { left, right, count };
    private const float GROUND_HEIGHT = -8;

    private Vector2 startPos;
    private bool hitGround = false;
    private int damage = 0;

    public GameObject deadEnemy;
	public Transform groundCheck;
	public Transform groundCheck2;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public LayerMask whatIsGround2;
	private bool grounded; // original
	private bool grounded2;
	
	private float jumpHeight = 20F;
	private float moveSpeed = 5.0F;
	private float downHeight = 5.5F;
	private float waitTime = 0.65F;
	
	private bool stopped;
	private bool jump;
	
	private float leftX = -22.0f;
	private float rightX = 20.0f;
	private float heightY = 2.0f;
	
	private int randomJump;	
	private Vector2 myPosition;
	private Vector2 leftLimit;
	private Vector2 rightLimit;
	
	Collider2D gorilla;	
	Collider2D gorilla2;
	
	
	// Use this for initialization
	void Start () {

        startPos = transform.position;

		gorilla = GetComponent<CircleCollider2D>();	
		gorilla2 = GetComponent<BoxCollider2D>();
		groundCheckRadius = 1.0f;
		stopped = false;
		jump = false;

        StartCoroutine(RandomSounds());
		StartCoroutine (run (waitTime));		
	}

	void FixedUpdate () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        grounded2 = Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, whatIsGround2);

        if ((grounded || grounded2) && hitGround)
            hitGround = false;

        if (!hitGround)
        {
            if (!grounded && grounded2)
            {
                gorilla.isTrigger = true;
                if (!jump)
                    gorilla2.isTrigger = false;
                else
                    gorilla2.isTrigger = true;
            }
            else if (grounded && !grounded2)
            {
                if (!jump)
                {
                    gorilla.isTrigger = false;
                }
                else
                {
                    gorilla.isTrigger = true;
                }
                gorilla2.isTrigger = true;
            }
            else if (grounded && grounded2)
            {
                gorilla.isTrigger = true;
                gorilla2.isTrigger = true;
            }
            else
            {
                gorilla.isTrigger = false;
                gorilla2.isTrigger = true;
            }
        }

        // If this enemy hits the ground he is knocked back into the trees
        if (transform.position.y < GROUND_HEIGHT)
        {
            hitGround = true;
            grounded = false;
            grounded = false;
            HitTheGround();
        }
        // If this enemy goes too far out of bounds then he is reset to start position
        else if(transform.position.x < -37.5f || transform.position.x > 37.5f || transform.position.y > 24f)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hitGround = false;
            transform.position = startPos;
        }

}
    /// <summary>
    /// When the player falls to the ground this reverses the velocity to get him back on a branch.
    /// </summary>
    private void HitTheGround()
    {
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

        // When there's no x velocity this adds x velocity in a random direction,
        // so the player can grab a branch when there's no branch above them.
        if (vel.x == 0)
        {
            dir ran_dir = (dir)Random.Range(0, 2);
            if (ran_dir == dir.left)
                vel = new Vector2(2f, vel.y);
            else
                vel = new Vector2(2f, vel.y);
        }

        GetComponent<Rigidbody2D>().velocity = -(vel * 1.2f);
    }

    bool ckHeight(float heightY) {
		myPosition = transform.position;
		if (myPosition.y > heightY) {
			return true;
		} else {
			return false;
		}
	}
	bool ckPlace() {
		myPosition = transform.position;
		if (myPosition.x < leftX || myPosition.x > rightX) {
			return false;
		} else
			return true;
	}
	bool ckRight() {
		rightLimit = transform.position;
		if (rightLimit.x > rightX) {
			return false;
		} else
			return true;
	}
	bool ckLeft() {
		leftLimit = transform.position;
		if (leftLimit.x < leftX) {		
			return false;
		} else
			return true;
	}	
	
	void JumpR() {// move to the right
		//yield return new WaitForSeconds (waitTime);
		if (grounded || grounded2) {//
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}	
	}
	void JumpL() {// move to the left

		if (grounded || grounded2) {//
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}	
	}
	bool ckJump()
	{
		if (jump)
			return true;
		else
			return false;
	}
	void JumpUp() {
		if (grounded || grounded2) {// 
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
		} 		
	}
	void JumpDown() {
		if (ckHeight (heightY)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, -downHeight);
		} 
	}
	IEnumerator run(float waitTime) {		
		bool leftMove;// = false;
		bool rightMove;// = false;
		int number = Random.Range(1, 100); // creates a number between 1 and 100 randomly
		
		while (!stopped) {

			leftMove = ckLeft();
			rightMove = ckRight();			
			
			if(rightMove && leftMove) {
				if(number%2 == 0) {
					JumpR();					
				} else {
					JumpL();					
				}
			} else if (leftMove) {
				JumpL();
				number = 1;
			} else if (rightMove) {
				JumpR();
				number = 2;
			}

			yield return new WaitForSeconds (waitTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		jump = true;	
        if(other.tag == "Bullet")
        {
            Rigidbody2D bulletBody = other.GetComponent<Rigidbody2D>();
            Rigidbody2D enemyBody = gameObject.GetComponent<Rigidbody2D>();
            other.GetComponent<Collider2D>().enabled = false;
            enemyBody.velocity = Vector2.zero;
            enemyBody.velocity = bulletBody.velocity.normalized * 5;
            bulletBody.velocity = Vector2.zero;
            TakeDamage();
        }	
	}

	/// <summary>
    /// When this enemy is hit by a bullet object, he takes damage
    /// </summary>
    private void TakeDamage()
    {
        damage++;
        if(damage == 1)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if(damage == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if(damage == 3)
        {
            Instantiate(deadEnemy, transform.position, Quaternion.identity);
            EnemyManagerLvl2.enemyManagerScript.EnemyDead();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Determines if the enemy will make a noise and what noise they will make
    /// </summary>
    private IEnumerator RandomSounds()
    {
        float clipLength = 0;
        bool gorillaActive = true;
        bool makeNoise = true;

        while(gorillaActive)
        {
            makeNoise = Random.Range(0, 100) < 50 ? false : true;

            if (makeNoise && Time.timeScale == 1)
                clipLength = SoundManager.soundManagerScript.GorillaNoise(GetComponent<AudioSource>());
            yield return new WaitForSeconds(clipLength + 2f);
        }

        yield return null;
    }

	/*void OnTriggerStay2D(Collider2D other)
	{
		jump = false;
	}*/
	
	//whenever monkey pass the collider, trigger set to false
	void OnTriggerExit2D(Collider2D other)
	{
		jump = false;
	}
}
