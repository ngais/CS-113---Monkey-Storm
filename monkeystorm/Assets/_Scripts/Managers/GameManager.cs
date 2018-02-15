using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManagerScript;

    private bool tootLvl_1 = true;
    private bool tootLvl_2 = true;
    private bool tootLvl_3 = true;
    private int score = 0;
    private int level = 1;
    
    public bool TootLvl_1 { get { return tootLvl_1; } set { tootLvl_1 = value; } }
    public bool TootLvl_2 { get { return tootLvl_2; } set { tootLvl_2 = value; } }
    public bool TootLvl_3 { get { return tootLvl_3; } set { tootLvl_3 = value; } }
    public int Score { get { return score; } set { score = value; } }
    public int Level { get { return level; } set { level = value; } }

    private void Awake ()
    {
	    if(gameManagerScript == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManagerScript = this;
        }
        else if(gameManagerScript != this)
            Destroy(gameObject);
	}	
}
