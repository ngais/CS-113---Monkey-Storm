using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HouseBuilder : MonoBehaviour {
    private bool checkInput = false;

    public GameObject placeHere;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject ceiling;
    public GameObject leftBranch;
    public GameObject rightBranch;
    public GameObject ceilingBranch;
    public GameObject endLvlTxt;
    public UILabel topTxt;
    public UILabel botTxt;

    private void Update()
    {
        if (checkInput)
            if (Input.GetMouseButtonDown(0))
                Application.LoadLevel("level2");
    }

    /// <summary>
    /// Checks for a branch and then uses it to build a piece of the house
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Branch" && other.name != "Unbreakable")
        {
            UIEvents.uiEventsScript.Score = 50;
            SoundManager.soundManagerScript.Construct();

            if (leftWall.activeInHierarchy)
            {
                SoundManager.soundManagerScript.Celebrate();
                ceiling.SetActive(true);
                endLvlTxt.SetActive(true);
                topTxt.text = "Yay, you did it!! Now Chocco can live happily ever after...";
                botTxt.text = "...Or can he?";
                checkInput = true;
                GameManager.gameManagerScript.Level = 2;
                StartCoroutine(UIEvents.uiEventsScript.MinusTimer());
                GameInstance.level++;
                Time.timeScale = 0;
            }
            else if (rightWall.activeInHierarchy)
            {
                leftWall.SetActive(true);
                ceilingBranch.GetComponent<Branch>().StartColorChange(Color.blue);
            }
            else
            {
                placeHere.SetActive(false);
                rightWall.SetActive(true);
                leftBranch.GetComponent<Branch>().StartColorChange(Color.blue);
            }

            Monkey.monkeyScript.CarryBranch = false;
            other.gameObject.SetActive(false);
        }
    }
}
