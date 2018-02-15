using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TootLvl_1 : MonoBehaviour {

    private int tootStep = 0;
    private int tootStepMax = 6;
    public GameObject tootMenu;
    public UIButton pauseBtn;
    public UILabel topTootTxt;
    public UILabel botTootTxt;

	// Use this for initialization
	void Start ()
    {
        if (GameInstance.level >= 4)
        {
            tootStep = 6;
            tootStepMax = 9;
        }
        tootMenu.SetActive(true);
        pauseBtn.enabled = false;
        Time.timeScale = 0;
 
    }

    private void Update()
    {
        if (tootStep < tootStepMax && Input.GetMouseButtonDown(0))
        {
            TootStep();
        }
    }
	
    public void TootStep()
    {
        // Start tutorial from level 1
        if(tootStep == 0)
        {
            tootStep++;
            topTootTxt.text = "To move Chacco from branch to branch";
            botTootTxt.text = "Simply swipe in the direction that you want him to move";
        }
        else if(tootStep == 1)
        {
            tootStep++;
            topTootTxt.text = "When Chacco spots a wall or ceiling branch for his new home";
            botTootTxt.text = "That branch will turn blue.";
        }
        else if (tootStep == 2)
        {
            tootStep++;
            topTootTxt.text = "While hanging onto a blue branch, tap the screen to break it,";
            botTootTxt.text = "Then bring it back to the start and add it to Chacco's home.";
        }
        else if (tootStep == 3)
        {
            tootStep++;
            topTootTxt.text = "Chacco can jump further if you swipe backward,";
            botTootTxt.text = "And then swipe forwards, without lifting your finger.";
        }
        else if (tootStep == 4)
        {
            tootStep++;
            topTootTxt.text = "Touch anywhere to begin";
            botTootTxt.text = "";
        }
        else if(tootStep == 5)
        {
            tootStep++;
            GameManager.gameManagerScript.TootLvl_1 = false;
            tootMenu.SetActive(false);
            pauseBtn.enabled = true;
            Time.timeScale = 1;
        }
        // Start tutorial from level 4
        else if(tootStep == 6)
        {
            tootStep++;
            topTootTxt.text = "The EMFA are in the area.";
            botTootTxt.text = "You must avoid them and build a hideout shelter.";
        }
        else if (tootStep == 7)
        {
            tootStep++;
            topTootTxt.text = "Touch anywhere to begin.";
            botTootTxt.text = "";
        }
        else if (tootStep == 8)
        {
            tootStep++;
            GameManager.gameManagerScript.TootLvl_1 = false;
            tootMenu.SetActive(false);
            pauseBtn.enabled = true;
            Time.timeScale = 1;
        }
    }
}
