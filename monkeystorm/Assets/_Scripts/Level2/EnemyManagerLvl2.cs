using UnityEngine;
using System.Collections;

public class EnemyManagerLvl2 : MonoBehaviour {

    public static EnemyManagerLvl2 enemyManagerScript;
    public GameObject[] enemy;
    public GameObject gameOva;
    public UILabel topText;
    public UILabel botText;

    private bool checkInput = false;
    private int numSecTilRelease = 10;
    private int enemyIndex = 0;
    private int maxEnemy = 2;
    private int enemyDead = 0;

    private void Awake()
    {
        enemyManagerScript = this;
    }

    private void Update()
    {
        if (checkInput)
            if (Input.GetMouseButtonDown(0))
                Application.LoadLevel("level3");

        if (GameInstance.timer < numSecTilRelease * maxEnemy && (int)GameInstance.timer % numSecTilRelease == 0 && 
            enemyIndex * numSecTilRelease < GameInstance.timer + 1)
        {
            enemy[enemyIndex].SetActive(true);
            enemyIndex++;
            Debug.Log("release");
        }
    }

    /// <summary>
    /// Checks if there are more enemy to release and then releases them into the world
    /// </summary>
    public void ReleaseEnemy()
    {
        if(enemyIndex < maxEnemy -1)
        {
            enemy[enemyIndex].SetActive(true);
            enemyIndex++;
        }
    }

    /// <summary>
    /// Handles the death of an enemy
    /// </summary>
    public void EnemyDead()
    {
        enemyDead++;
        UIEvents.uiEventsScript.Score = 100;

        if (enemyDead == maxEnemy)
        {
            SoundManager.soundManagerScript.Celebrate();
            topText.text = "You have defeated the EMFA!!";
            botText.text = "Looks like you are safe... for now.";
            gameOva.SetActive(true);
            checkInput = true;
            GameManager.gameManagerScript.Level = 3;
            StartCoroutine(UIEvents.uiEventsScript.MinusTimer());
            GameInstance.level++;
            Time.timeScale = 0;
        }
    }
}
