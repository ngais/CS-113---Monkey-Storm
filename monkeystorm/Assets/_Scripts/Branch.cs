using UnityEngine;
using System.Collections;

public class Branch : MonoBehaviour {
    #region Constants
    private const float MAX_BRANCH_STRENGTH = 10;
    private const float MIN_BRANCH_STRENGTH = 1;
    private const float COLOR_LERP = .02F;
    #endregion

    #region Private Variables
    private bool colorChanging = false;
    private float colorToRed = 0;
    private float branchStrength = 0;
    #endregion

    #region Public Variables
    private Transform monkey;
    public bool housePart = false;
    public bool rightWall = false;
    #endregion

    #region Properites
    public float BranckStrength { set { branchStrength -= value; } }
    #endregion

    #region Unity Methods
    private void Start()
    {
        branchStrength = Random.Range(MIN_BRANCH_STRENGTH, MAX_BRANCH_STRENGTH);
        if(rightWall)
            StartCoroutine(ColorChange(Color.blue));
        if (housePart)
            branchStrength = 1;
    }

    private void FixedUpdate()
    {
        if (housePart)
        {
            if (branchStrength < 0)
            {
                GetComponent<Collider2D>().isTrigger = true;
                monkey = Monkey.monkeyScript.gameObject.transform;
                transform.position = new Vector2(monkey.position.x, monkey.position.y - 1);
                Monkey.monkeyScript.CarryBranch = true;
            }
        }
        else if(branchStrength < 1)
        {
            if(!colorChanging)
                StartCoroutine(ColorChange(Color.red));

            if(branchStrength < 0)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }

    #endregion
    #region Public Methods

    public void StartColorChange(Color c)
    {
        StartCoroutine(ColorChange(c));
    }

    #endregion
    #region Private Methods
    /// <summary>
    /// Makes a branch blink with a red color.
    /// </summary>
    private IEnumerator ColorChange(Color c)
    {
        colorChanging = true;
        while(colorChanging)
        {
            if ((int)(colorToRed % 2) == 0)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, c, COLOR_LERP);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, COLOR_LERP);
            }
            colorToRed += COLOR_LERP;
            yield return null;
        }
    }

    #endregion   
}
