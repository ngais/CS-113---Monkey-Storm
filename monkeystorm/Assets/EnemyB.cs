using UnityEngine;
using System.Collections;

public class EnemyB : MonoBehaviour {

	public Transform groundCheck;
	public Transform groundCheck2;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public LayerMask whatIsGround2;
	private bool grounded; // original
	private bool grounded2;
	
	private float jumpHeight = 20F;
	private float moveSpeed = 10.0F;
	private float waitTime = 0.65F;
	
	private bool stopped;
	private bool jump;
	
	private float leftX = -22.0f;
	private float rightX = 20.0f;

	private int randomJump;	
	private Vector2 myPosition;
	private Vector2 leftLimit;
	private Vector2 rightLimit;
	
	Collider2D gorilla;	
	Collider2D gorilla2;
	
	
	// Use this for initialization
	void Start () {
		gorilla = GetComponent<Collider2D>();
		gorilla2 = GetComponent<BoxCollider2D>();
		
		groundCheckRadius = 0.5f;
		stopped = false;
		jump = false;	
		
		StartCoroutine (run (waitTime));
	}
	void Update () {
		
	}
	void FixedUpdate () {		
		
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		grounded2 = Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, whatIsGround2);		

		/*if (!jump) {
			if (!grounded) {
				gorilla.isTrigger = true;
			} else {
				gorilla.isTrigger = false;
			}
		} else {
			gorilla.isTrigger = true;
		}		
		if(!grounded2) {
			gorilla2.isTrigger = true;
		} else {
			gorilla2.isTrigger = false;
		}*/
		if (!grounded && grounded2) {
			gorilla.isTrigger = true;
			if (!jump)			
				gorilla2.isTrigger = false;
			else
				gorilla2.isTrigger = true;
		} else if (grounded && !grounded2) {
			if (!jump) {
				gorilla.isTrigger = false;
			} else {
				gorilla.isTrigger = true;
			}
			gorilla2.isTrigger = true;
		} else if (grounded && grounded2) {
			gorilla.isTrigger = true;
			gorilla2.isTrigger = true;
		} else {
			gorilla.isTrigger = false;
			gorilla2.isTrigger = true;
		}
	}

	
	bool ckHeight(float heightY) {
		myPosition = transform.position;
		if (myPosition.y > heightY) {//  && Random.Range(1,100)%10 == 1 
			//Debug.Log (myPosition.y);
			//gorilla.isTrigger = true;
			return true;
		} else {
			//gorilla.isTrigger = false;
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
		//yield return new WaitForSeconds (waitTime);
		if (grounded || grounded2) {//|| grounded2
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
		if (grounded || grounded2) {
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
		}
	}
	
	IEnumerator run(float waitTime) {		
		bool leftMove;// = false;
		bool rightMove;// = false;
		
		
		while (!stopped) {	
			leftMove = ckLeft();
			rightMove = ckRight();
			randomJump = Random.Range(1, 10);			
			
			if(rightMove && leftMove) {
				if(randomJump%2 == 0) {
					if(randomJump%3 == 0) {
						JumpUp();
					} else {
						JumpR();
					}
				} else {
					if(randomJump%3 == 0) {
						JumpUp();									
					} else {
						JumpL();
					}
				}
			} else if (leftMove) {
				JumpL();			
			} else if (rightMove) {
				JumpR();
			} 
			
			yield return new WaitForSeconds (waitTime);				
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		jump = true;
	}
	
	/*void OnTriggerStay2D(Collider2D other) {

	}*/
	
	//whenever monkey pass the collider, trigger set to false
	void OnTriggerExit2D(Collider2D other){
		jump = false;
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Monkey") {//coll.gameObject.tag == "Enemy" || 
			//Destroy (coll.gameObject);
			//coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10,10) * 400.0f);
		}
		
	}
}
