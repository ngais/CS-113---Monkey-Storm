using UnityEngine;
using System.Collections;

public class EnemyC : MonoBehaviour {

	public Transform groundCheck;
	public Transform groundCheck2;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public LayerMask whatIsGround2;
	private bool grounded; // original
	private bool grounded2;
	
	private float jumpHeight = 20F;
	private float moveSpeed = 10.0F;
	private float downHeight = 5.5F;
	private float waitTime = 0.65F;
	
	private bool stopped;
	private bool jump;
	private bool down;
	
	private float leftX = -22.0f;
	private float rightX = 20.0f;
	private float heightY = 2.0f; // relative height compared to Monkey
	
	private int randomJump;	
	private Vector2 myPosition;
	private Vector2 leftLimit;
	private Vector2 rightLimit;
	
	Collider2D gorilla;	
	Collider2D gorilla2;
	
	
	public Monkey monkey; // for reference to class Monkey
	private Vector2 oldMonkeyPosition;
	//private Vector2 newMonkeyPosition;
	private Vector2 nowGorillaPosition;
	
	
	
	// Use this for initialization
	void Start () {
		gorilla = GetComponent<CircleCollider2D>();	
		gorilla2 = GetComponent<BoxCollider2D>();
		groundCheckRadius = 0.5f;
		stopped = false;
		jump = false;
		down = false;
		
		StartCoroutine (run (waitTime));		
	}
	void Update () {
		
	}
	void FixedUpdate () {

		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		grounded2 = Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, whatIsGround2);

		if (!jump) {			
			if (!grounded) {
				gorilla.isTrigger = true;
			} else { 
				gorilla.isTrigger = false;
			}
		} else {
			gorilla.isTrigger = true;
		}
		if (down && grounded2) {
			gorilla2.isTrigger = true;
			
		} else{
			gorilla2.isTrigger = false;
		}		
	}
	
	bool ckHeight() {
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
	void moveToMonkey (float speed) {
		if (speed > 0)
			moveR (speed);
		else
			moveL (speed);
	}
	void moveR(float speed) {// move to the right  
		if (grounded || grounded2) {//
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		
	}
	void moveL(float speed) {// move to the left
		if (grounded || grounded2) {//
			jump = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpHeight);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);
		}		
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
	bool ckDown()
	{
		if (down)
			return true;
		else
			return false;
	}
	void JumpDown() {
		
		if ((grounded || grounded2) && ckHeight ()) {//grounded ||  ckHeight ()
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 3.5F);
			down = true;
			
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, -downHeight);
		} 
	}
	void catchMonkey() {

		int repeat_num = 0;
		bool direction = true;		
		nowGorillaPosition 	= transform.position;
		oldMonkeyPosition 	= monkey.transform.position;		

		repeat_num =(int) ((oldMonkeyPosition.x - nowGorillaPosition.x) / 5.0F);

		if (oldMonkeyPosition.x - nowGorillaPosition.x < 0) {
			repeat_num *= -1;
			direction = false;
		}

		while (repeat_num > 0) {//
			if(direction) {
				moveToMonkey(moveSpeed);
			} else {
				moveToMonkey(-moveSpeed);
			}
			repeat_num--;
		}
		
		if (oldMonkeyPosition.y - nowGorillaPosition.y > 0)
			JumpUp ();
		else
			JumpDown ();
	}
	
	IEnumerator run(float waitTime) {		
		bool leftMove;
		bool rightMove;		
		
		while (!stopped) {
			leftMove = ckLeft();
			rightMove = ckRight();			
			
			if(rightMove && leftMove) {
				catchMonkey();
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
		down = true;
	}
	
	/*void OnTriggerStay2D(Collider2D other) {
		jump = false;
	}*/
	
	//whenever monkey pass the collider, trigger set to false
	void OnTriggerExit2D(Collider2D other){
		jump = false;
		down = false;
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Monkey") {//coll.gameObject.tag == "Enemy" || 
			//Destroy (coll.gameObject);
			//coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10,10) * 400.0f);
		}
		
	}
}
