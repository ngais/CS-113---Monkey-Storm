using UnityEngine;
using System.Collections;

public class TootLvl_3 : MonoBehaviour
{
    private int tootStep = 0;

    public GameObject tootMenu;
    public UIButton pauseBtn;
    public UILabel topTootTxt;
    public UILabel botTootTxt;

    // Use this for initialization
    void Start()
    {
        if (GameManager.gameManagerScript.TootLvl_3)
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
            topTootTxt.text = "Run!!";
            botTootTxt.text = "You must outrun the bulldozer!";
        }
        else if (tootStep == 1)
        {
            tootStep++;
            topTootTxt.text = "Touch anywhere to begin";
            botTootTxt.text = "";
        }
        else if (tootStep == 2)
        {
            tootStep++;
            GameManager.gameManagerScript.TootLvl_3 = false;
            tootMenu.SetActive(false);
            pauseBtn.enabled = true;
            Time.timeScale = 1;
        }
    }
}
