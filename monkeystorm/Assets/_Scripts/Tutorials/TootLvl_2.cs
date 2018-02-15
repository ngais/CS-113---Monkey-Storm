using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TootLvl_2 : MonoBehaviour
{
    private int tootStep = 0;

    public GameObject tootMenu;
    public UIButton pauseBtn;
    public UILabel topTootTxt;
    public UILabel botTootTxt;

    // Use this for initialization
    void Start()
    {
        if (GameManager.gameManagerScript.TootLvl_2)
        {
            tootMenu.SetActive(true);
            pauseBtn.enabled = false;
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }

    private void Update()
    {
        if (tootStep < 4 && Input.GetMouseButtonDown(0))
        {
            TootStep();
        }
    }

    public void TootStep()
    {
        if (tootStep == 0)
        {
            tootStep++;
            topTootTxt.text = "Avoid the EMFA monkeys, as they try to knock you out.";
            botTootTxt.text = "Collect bananas to throw at them, until you drive them away.";
        }
        else if (tootStep == 1)
        {
            tootStep++;
            topTootTxt.text = "Touch Chacco and swipe towards your target.";
            botTootTxt.text = "Chacco will throw a banana in the direction of your swipe.";
        }
        else if (tootStep == 2)
        {
            tootStep++;
            topTootTxt.text = "Touch anywhere to begin";
            botTootTxt.text = "";
        }
        else if (tootStep == 3)
        {
            tootStep++;
            GameManager.gameManagerScript.TootLvl_2 = false;
            tootMenu.SetActive(false);
            pauseBtn.enabled = true;
            Time.timeScale = 1;
        }
    }
}