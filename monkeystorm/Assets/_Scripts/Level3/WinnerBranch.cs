using UnityEngine;
using System.Collections;

public class WinnerBranch : MonoBehaviour {

    public GameObject gameOva;
    public UILabel topText;
    public UILabel botText;

    private bool checkInput = false;

    private void Update()
    {
        if (checkInput)
            if (Input.GetMouseButtonDown(0))
                Application.LoadLevel("level1");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            SoundManager.soundManagerScript.Celebrate();
            topText.text = "You have outrun the bulldozer!!";
            botText.text = "Now you must find a new place to rebuild your home.";
            gameOva.SetActive(true);
            checkInput = true;
            GameManager.gameManagerScript.Level = 1;
            UIEvents.uiEventsScript.Score = 250;
            StartCoroutine(UIEvents.uiEventsScript.MinusTimer());
            GameInstance.level++;
            Time.timeScale = 0;
        }
    }
}
