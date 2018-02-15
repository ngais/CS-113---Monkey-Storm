using UnityEngine;
using System.Collections;

public class EnemyManager_l1 : MonoBehaviour {

    public GameObject[] enemy;

    private void Start()
    {
        if(GameInstance.level == 4)
        {
            enemy[0].SetActive(true);
        }
    }
}
