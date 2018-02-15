using UnityEditor;
using UnityEngine;


public class AddEditorOptions : MonoBehaviour {

    [MenuItem("Edit/Reset Playerprefs")]

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
