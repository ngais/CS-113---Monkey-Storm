using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIEvents : MonoBehaviour
{
    //Main menu
    public GameObject mainMenu;
    public GameObject setting;
    public GameObject Record;
    public GameObject Rankings;
    public Texture[] BGTextures;
    public UITexture BG;
    //Game menu
    public GameObject gameMenu;
   
    public UILabel timerLabel;
    public UILabel scoreLabel;
    public UILabel levelLabel;

    public static UIEvents uiEventsScript;
    public UITexture[] hearts;
    public UIButton pauseBtn;
    public UILabel playBtnLbl;

    private bool loseHeart = true;
    private bool restartLvl = false;
    private int heartIndex;

    public bool LoseHeart { get { return loseHeart; } }
    public int NumHearts { get { return hearts.Length - heartIndex; } }

    /// <summary>
    /// Returns the timer value as an integer
    /// </summary>
    public int Timer {
        get
        {
            int timer;
            int.TryParse(timerLabel.text, out timer);
            return timer;
        }
        set
        {
            timerLabel.text = value.ToString();
        }
    }

    /// <summary>
    /// Returns a score integer and takes a integer that is added to the score and displayed
    /// </summary>
    public int Score {
        get
        {
            return GameInstance.currentScore;
        }
        set
        {
            GameInstance.currentScore += value;
            scoreLabel.text = (GameInstance.currentScore).ToString() ;
        }
    }

    private void Awake()
    {
        uiEventsScript = this;
        heartIndex = hearts.Length - 1;
    }

    private void Start()
    {
        Debug.Log(GameInstance.saveInfo[0]);
        if (Application.loadedLevelName != "Main Menu")
        {
            GameInstance.timer = 0;
            scoreLabel.text = GameManager.gameManagerScript.Score.ToString();
            levelLabel.text = GameInstance.level.ToString();
        }
        else
        {
            GameInstance.currentScore = 0;
            for (int k = 0; k < GameInstance.scores.Length; k++)
            {
                GameInstance.scores[k] = PlayerPrefs.GetString("ScoreSlot" + k.ToString());
                Debug.Log(GameInstance.scores[k]);
            }

            for (int k = 0; k < GameInstance.saveInfo.Length; k++)
            {
                GameInstance.saveInfo[k] = PlayerPrefs.GetString("SaveSlot" + k.ToString());
                Debug.Log(GameInstance.saveInfo[k]);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName != "Main Menu" && Time.timeScale != 0)
        {
            GameInstance.timer += Time.deltaTime;
            timerLabel.text = ((int)GameInstance.timer).ToString();

            //scoreLabel.text = GameInstance.currentScore.ToString();

            //for (int i = 0; i < hearts.Length; i++)
            //{
            //    hearts[i].gameObject.SetActive(false);
            //}
            //for (int i = 0; i < GameInstance.currentHP; i++)
            //{
            //    hearts[i].gameObject.SetActive(true);
            //}
        }
    }

    #region MainMenu
    public void OnPlayClick()
    {
        Application.LoadLevel("Level1");
    }

    public void OnSettingClick()
    {
        BG.mainTexture = BGTextures[1];
        mainMenu.SetActive(false);
        setting.transform.DOLocalMoveX(0, 0.3f);
        setting.SetActive(true);

    }

    public void OnBGMClick()
    {
        UIButton.current.transform.FindChild("BGMSliders").gameObject.SetActive(true);
    }

    public void OnBGMSlidersCancel()
    {
        UIEventTrigger.current.gameObject.SetActive(false);
    }

    public void OnMusicSliderValueChange()
    {
        GameInstance.volum = UISlider.current.value;
        Debug.Log(GameInstance.volum);
    }

    public void OnHelpClick()
    {
        UIButton.current.transform.FindChild("HelpMenu").gameObject.SetActive(true);
    }

    public void OnHelpMenuCancel()
    {
        UIEventTrigger.current.gameObject.SetActive(false);
    }

    public void OnLoadClick()
    {
        BG.mainTexture = BGTextures[1];
        mainMenu.SetActive(false);
        Record.transform.DOMoveX(0, 0.3f);
        Record.SetActive(true);

        //for (int i = 0; i < GameInstance.saveInfo.Length; i++)
        //{
        //    Record.transform.FindChild("record " + i.ToString()).GetComponentInChildren<UILabel>().text =
        //        GameInstance.saveInfo[i];
        //}
    }

    public void OnMenuClick()
    {
        BG.mainTexture = BGTextures[0];
        setting.transform.DOMoveX(795 / 360, 0.3f);
        setting.SetActive(false);
        mainMenu.transform.DOMoveX(0, 0.3f);
        mainMenu.SetActive(true);
    }

    public void OnRecordBacklClick()
    {
        BG.mainTexture = BGTextures[0];
        Record.SetActive(false);
        Record.transform.DOMoveX(795 / 360, 0.3f);
        mainMenu.transform.DOMoveX(0, 0.3f);
        mainMenu.SetActive(true);

       
    }

    public void OnRecordClick()
    {
        BG.mainTexture = BGTextures[1];
        setting.SetActive(false);
        Rankings.transform.DOMoveX(0, 0.3f);
        Rankings.SetActive(true);

        for (int i = 0; i < GameInstance.scores.Length; i++)
        {
            Rankings.transform.FindChild("rank " + i.ToString()).GetComponentInChildren<UILabel>().text =
                GameInstance.scores[i];
        }
    }

    public void OnLoadRecord()
    {
        string[] tempStrs = UIButton.current.GetComponentInChildren<UILabel>().text.Split(':');
        Application.LoadLevel(tempStrs[1]);
    }

    public void OnRankingBackClick()
    {
        BG.mainTexture = BGTextures[0];
        Rankings.SetActive(false);
        Rankings.transform.DOMoveX(795 / 360, 0.3f);
        mainMenu.transform.DOMoveX(0, 0.3f);
        mainMenu.SetActive(true);
    }
    #endregion

    #region GameMenu
    public void OnPauseClick()
    {
        Time.timeScale = 0; // game pause
        gameMenu.SetActive(true);
    }

    public void OnGameMenuPlayClick()
    {
        Time.timeScale = 1; //resume
        if (restartLvl)
            Application.LoadLevel(Application.loadedLevelName);
        else
            gameMenu.SetActive(false);
    }

    public void OnSaveClick()
    { }

    public void OnMainMenuSettingClick()
    { }

    public void OnQuitClick()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Main Menu");
    }

    public void LoadLevel4()
    {
        GameInstance.level = 4;
        Application.LoadLevel("Level1");
    }

    public void LoadLevel2()
    {
        GameInstance.level = 2;
        Application.LoadLevel("Level2");
    }

    public void LoadLevel3()
    {
        GameInstance.level = 3;
        Application.LoadLevel("Level3");
    }

    /// <summary>
    /// Removes all of the players hearts
    /// </summary>
    public void InstantKill()
    {
        for(int i = 0; i < hearts.Length - heartIndex; i++)
        {
            RemoveHeart();
            loseHeart = true;
        }
    }

    /// <summary>
    /// Removes a heart from the UI when damage received. Returns true if there are hearts to lose
    /// </summary>
    public bool  RemoveHeart()
    {
        if(loseHeart)
        {
            if (heartIndex >= 0)
            {
                hearts[heartIndex].enabled = false;
                heartIndex--;
            }
            
            if (heartIndex < 0)
            {
                transform.FindChild("GameOver").gameObject.SetActive(true);
                pauseBtn.enabled = false;
                Time.timeScale = 0;
                return false;
            }
            
            loseHeart = false;
            Invoke("StopRemoveHeart", .5f);
        }

        return true;
    }

    /// <summary>
    /// Subtracts the timer from the score
    /// </summary>
    public IEnumerator MinusTimer()
    {
        float mytime = 0;
        GameManager.gameManagerScript.Score = Score - Timer;
        while (Timer > 0)
        {
            if(mytime % 5 == 0)
            {
                Timer = (Timer - 1);
                Score = -1;
            }

            mytime += 1;
            yield return null;
        }
            
    }

    /// <summary>
    /// Allows to set a time between losing hearts, so you don't lose multiple in one hit
    /// </summary>
    private void StopRemoveHeart()
    {
        loseHeart = true;
    }

    public void OnSave()
    {
        UIButton.current.transform.FindChild("Input").gameObject.SetActive(true);
    }

    public void OnSaveConfirm()
    {
        int i = 0;
        for (; i < GameInstance.saveInfo.Length; i++)
        {
            if (GameInstance.saveInfo[i] == string.Empty)
            {
                break;
            }
        }
        string saveName = UIButton.current.transform.parent.GetComponent<UIInput>().value == string.Empty ? "Defalut" : UIButton.current.transform.parent.GetComponent<UIInput>().value;

        if (i == GameInstance.saveInfo.Length)     //full
        {
            for (int j = 0; j < GameInstance.saveInfo.Length-1; j++)
            {
                GameInstance.saveInfo[j] = GameInstance.saveInfo[j + 1];
            }
            GameInstance.saveInfo[GameInstance.saveInfo.Length-1] = saveName + ":" + Application.loadedLevelName;
        }
        else
        {
            GameInstance.saveInfo[i] = saveName + ":" + Application.loadedLevelName;
        }

        for (int k = 0; k < GameInstance.saveInfo.Length; k++)
        {
            PlayerPrefs.SetString("SaveSlot"+k.ToString(), GameInstance.saveInfo[k]);
        }

        UIButton.current.transform.parent.gameObject.SetActive(false);
    }

    public void OnSaveCancel()
    {
         UIButton.current.transform.parent.gameObject.SetActive(false);
    }

    public void OnGameOverConfirm()
    {
        Time.timeScale = 1;
        GameInstance.level = 1;

        if (GameInstance.currentScore == 0)
        {
            Application.LoadLevel("Main Menu");
            return;
        }
        int i = 0;
        for (; i < GameInstance.scores.Length; i++)
        {
            Debug.Log(GameInstance.scores[i]);
            if (GameInstance.scores[i].Length <= 2)
            {
                break;
            }
        }
        string saveName = UIButton.current.transform.parent.GetComponent<UIInput>().value == string.Empty ? "Defalut" : UIButton.current.transform.parent.GetComponent<UIInput>().value;

        if (i == GameInstance.scores.Length)     //full
        {
            for (int j = 0; j < GameInstance.scores.Length; j++)
            {
                string[] tempStrs = GameInstance.scores[j].Split(':');
                int tempScore = Int32.Parse(tempStrs[1]);
                if (GameInstance.currentScore <= tempScore)
                    continue;
                else
                {
                    for (int h = GameInstance.scores.Length - 1; h > j; h--)
                    {
                        GameInstance.scores[h] = GameInstance.scores[h - 1];
                    }
                    GameInstance.scores[j] = saveName + ":" + GameInstance.currentScore.ToString(); ;
                }
            }
        }
        else
        {
            GameInstance.scores[i] = saveName + ":" + GameInstance.currentScore.ToString(); ;
        }

        for (int k = 0; k < GameInstance.scores.Length; k++)
        {
            PlayerPrefs.SetString("ScoreSlot" + k.ToString(), GameInstance.scores[k]);
        }

        GameInstance.level = 1;
        GameInstance.currentScore = 0;
        Application.LoadLevel("Main Menu");
    }
    #endregion
}
