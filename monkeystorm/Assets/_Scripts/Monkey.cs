using UnityEngine;
using System.Collections;

public class Monkey : MonoBehaviour {
    //left click variable instead of put 0
    public int leftClick = 0;
    //check appropiate swipe length
    public int swipeLengh = 40;
    //save mouse positions
    public Vector2 oldMousePosition;
    public Vector2 newMousePosition;
    //moving options
    public float moveSpeed;
    public float jumpHeight;
    public float jumpS;
    public float moveS;
    //limit angle
    public float limitAngle;
    //ground check
    public Transform branchCheck;
    public float branchCheckRadius;
    public LayerMask whatIsBranch;
    private bool grounded;
    //give up grap and hold back grap
    Collider2D coll;
    //private bool pass;
    //for dragging
    private Vector2 curPosition;
    private float minX;
    private float maxX;
    public int dragSwipe = 20;
    private bool drag;
    private float sjump;

    private GameObject currentBranch;
    private Vector3 throwDirection;
    private bool carryBranch = false;
    private bool throwBanana = false;
    private bool takeDamage = false;
    private int numBananas = 0;

    public static Monkey monkeyScript;
    public AudioSource audioSource;

    public bool CarryBranch { set { carryBranch = value; } }
    public bool TakeDamage { get { return takeDamage; } set { takeDamage = value; }}

    private void Awake()
    {
        monkeyScript = this;
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        oldMousePosition = Vector2.zero;
        newMousePosition = Vector2.zero;
        moveSpeed = 11;
        jumpHeight = 16;
        jumpS = 1.5f;
        moveS = 0.5f;
        limitAngle = 70;
        branchCheck = GameObject.Find("Branch Check").transform;
        branchCheckRadius = 0.1f;
        whatIsBranch = LayerMask.GetMask("Branch");
        coll = branchCheck.GetComponent<Collider2D>();
        //pass = true;
        curPosition = Vector2.zero;
        drag = false;
        minX = 100000;
        maxX = -100000;
        sjump = 1.3f;
    }

    //check per sec; use this for physics
    void FixedUpdate()
    {
        //set grounded if it is attached to branches
        grounded = Physics2D.OverlapCircle(branchCheck.position, branchCheckRadius, whatIsBranch);
        if (grounded && takeDamage)
            takeDamage = false;

        // When the monkey is moving upwards disable the collider
        if (GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            branchCheck.GetComponent<Collider2D>().enabled = false;
        }
        else if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            branchCheck.GetComponent<Collider2D>().enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!throwBanana && !takeDamage)
        {
            //get delta mouse
            float deltaM = GetDeltaMouse();
            //swipe move
            if (deltaM >= swipeLengh)
            {
                //move
                MonkeyMove();

                //reset the delta after move
                ResetMousePosition();
            }
            //tap move
            else if (deltaM >= 0 && deltaM < swipeLengh)
            {
                //shaking
                if (!carryBranch && Time.timeScale != 0)
                    ShakingMove();

                //reset the delta after move
                ResetMousePosition();
            }
        }
    }

    // If you touch the monkey he will not move, just throw a banana
    private void OnMouseDown()
    {
        if (Application.loadedLevelName == "Level2")
        {
            throwBanana = true;
            throwDirection = Input.mousePosition;
        }
    }

    // Allows the monkey to move again, but needs to wait to prevent movement
    private void OnMouseUp()
    {
        if(numBananas > 0)
        {
            numBananas--;
            throwDirection = Input.mousePosition - throwDirection;
            if(throwDirection.x <= 0)
                GetComponentInChildren<Animator>().SetTrigger("ThrowLeft");
            else
                GetComponentInChildren<Animator>().SetTrigger("ThrowRight");
            Bananas.bananaScript.ThrowBanana(new Vector2(throwDirection.x, throwDirection.y), transform.position, numBananas);
        }
        Invoke("NoMoThrow", .1f);
    }

    // Allows the monkey to move again
    private void NoMoThrow()
    {
        throwBanana = false;
    }

    //shaking
    private void ShakingMove()
    {
        if (grounded)
        {
            SoundManager.soundManagerScript.Shake();
            GetComponentInChildren<Animator>().SetTrigger("Shake");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 3f);
            currentBranch = BranchCheck.branchCheckScript.CurrentBranch;
            if(currentBranch && currentBranch.name != "Unbreakable")
                currentBranch.GetComponent<Branch>().BranckStrength = .1f;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -jumpHeight);
            coll.isTrigger = false;
        }
    }

    //get delta vector to check it is valid swipe or not
    private float GetDeltaMouse()
    {
        //when mouse down, save the position
        if (Input.GetMouseButtonDown(leftClick))
        {
            oldMousePosition = Input.mousePosition;
            drag = true;
        }
        //when mouse up, save the position
        if (Input.GetMouseButtonUp(leftClick))
        {
            //drag set
            drag = false;
            newMousePosition = Input.mousePosition;
            //get delta
            return (Vector2.Distance(oldMousePosition,newMousePosition));
        }

        //get mouse position
        if(drag)
        GettingMousePositions();

        //when only mouse down
        return -1;        
    }
    
    //get mouse position
    private void GettingMousePositions()
    {
        if (Input.GetMouseButton(leftClick))
        {
                curPosition = Input.mousePosition;
            //get minX and maxX
            if (minX > curPosition.x)
                minX = curPosition.x;
            if (maxX < curPosition.x)
                maxX = curPosition.x;
        }
    }

    //reset old and new positions
    private void ResetMousePosition()
    {
        if ((oldMousePosition.magnitude + newMousePosition.magnitude) > 0)
        {
            newMousePosition = Vector2.zero;
            oldMousePosition = Vector2.zero;
        }

        if (!drag)
        {
            curPosition = Vector2.zero;
            minX = 100000;
            maxX = -100000;
        }
    }

    //move the monkey by delta vector
    private void MonkeyMove()
    {
        //presets for directions
        int rightD = 0;
        int leftD = 1;
        int straightD = 2;
        int upD = 3;
        int downD = 4;

        //check vector
        Vector2 checkV = newMousePosition - oldMousePosition;

        //get the directions for up or down
        int yD = GetYD(upD, downD);
        //get the directions for right, left, straight
        int xD = GetXD(rightD, leftD, straightD, checkV);

        //if xD is right and if yD is up, right jump
        //if xD is straight and if yD is up, straight up jump
        //if xD is left and if yD is up, left jump
        //if xD is left and if yD is down, left little move
        //if xD is straight and if yD is down, drop
        //if xD is right and if yD is down, right little move
        //if back drag, then swipe. it will be swing jump

        Move(rightD, leftD, straightD, upD, downD, xD, yD);
    }

    //check back drag
    private bool CheckBackDrag(int r, int l, int s, int XD)
    {
        if (XD == s)
            return false;

        else if(XD == r)
        {
            if (minX < oldMousePosition.x)
                return true;
            else
                return false;
        }
        else if(XD == l)
        {
            if (maxX > oldMousePosition.x)
                return true;
            else
                return false;
        }

        return false;
    }

    //get the directions for right, left, straight 
    private int GetXD(int r, int l, int s, Vector2 v)
    {
        //get an angle of swipe from x-axis to swipe vector
        float angle = Vector2.Angle(Vector2.right, v);

        //angle limits
        float rightLimit = limitAngle;
        float leftLimit = 180 - limitAngle;

        //angle will be from 0 to 180, need direction or checking axis
        if (angle < 0 || angle > 180)
            Debug.Log("angle error");
        //if angle from 0 to rightLimit, right
        if (angle < rightLimit)
            return r;
        //if angle from 180 to leftLimit, left
        else if (angle > leftLimit)
            return l;
        //if angle between limits, straight
        else 
            return s;

    }

    //get the directions for up or down
    private int GetYD(int u, int d)
    {
        //the vector we will compute
        Vector2 v = oldMousePosition - newMousePosition;

        //if y <0, up; if y >0 down
        if (v.y < 0)
            return u;
        else
            return d;
    }
    
    //move by directions
    private void Move(int r, int l, int s, int u, int d, int xD, int yD)
    {
        //check if it is swing
        bool swing = CheckBackDrag(r,l,s,xD);

        //add little move part
        Vector2 v = newMousePosition - oldMousePosition;
        float angle = Vector2.Angle(Vector2.right, v);
        bool littleM;
        if (angle < 15 || angle > (180 - 15))
        {
            GetComponentInChildren<Animator>().SetTrigger("Shake"); // Trigger shake animation if a small movement
            littleM = true;
        }          
        else
        {
            GetComponentInChildren<Animator>().SetTrigger("Jump"); // Trigger jump animation if jump movement
            SoundManager.soundManagerScript.PlayerJump(audioSource);
            littleM = false;
        }

        //up directions
        if (yD == u && grounded)
        {
            //if xD is right and if yD is up, right jump
            if (xD == r)
            {
                //if it is swing then swing jump
                if(swing)
                {
                    //coll.isTrigger = true;
                    //pass = false;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * sjump, jumpHeight * sjump);
                }
                else if(littleM)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * moveS, GetComponent<Rigidbody2D>().velocity.y);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, jumpHeight);
            }
            //if xD is straight and if yD is up, straight up jump
            else if (xD == s)
                StraightJump();
            //if xD is left and if yD is up, left jump
            else
            {
                //if swing then swing jump
                if (swing)
                {
                    //coll.isTrigger = true;
                    //pass = false;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed * sjump, jumpHeight * sjump);
                }
                else if(littleM)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed * moveS, GetComponent<Rigidbody2D>().velocity.y);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, jumpHeight);
            }
        }
        //down directions
        else if(yD == d && grounded)
        {
            //if xD is right and if yD is down, right little move
            if (xD == r)
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * moveS, GetComponent<Rigidbody2D>().velocity.y);
            //if xD is straight and if yD is down, drop
            else if (xD == s)
                Drop();
            //if xD is left and if yD is down, left little move
            else if(xD == l)
                GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed * moveS, GetComponent<Rigidbody2D>().velocity.y);
        }
        if(!grounded)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -1);
            coll.isTrigger = false;
        }
    }

    //drop move
    private bool Drop()
    {
        //if it attached to branches, make monkey isTrigger
        if (grounded)
            coll.isTrigger = true;
      
        //return true, if it is trigger; otherwise false
        return coll.isTrigger;
    }

    //straight jump, if jump it will pass one branch and hold back the grap
    private void StraightJump()
    {
        //coll.isTrigger = true;
        //pass = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight * jumpS);
    }

    // Looking for world collisions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Consumable")
        {
            if (other.name == "Banana_x1(Clone)")
            {
                numBananas += 1;
                Bananas.bananaScript.ConsumeBanana(numBananas);
            }
            else if (other.name == "Banana_x3(Clone)")
            {
                numBananas += 3;
                Bananas.bananaScript.ConsumeBanana(numBananas);
            }

            Destroy(other.gameObject);
        }
    }

    // I moved OnTriggerExit2D to a script on the BranchCheck object called BranchCheck.cs

}
