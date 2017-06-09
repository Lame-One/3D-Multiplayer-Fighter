using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PublicSheetExample: MonoBehaviour
{
    PublicSpreadSheetManager manager;
    void Start()
    {
        manager = new PublicSpreadSheetManager();

        manager.LoadPublicWorksheet("1GVXeyWCz0tCjyqE1GWJoayj92rx4a_hu4nQbYmW_PkE", "Sheet2", Callback);
    }

    void Callback()
    {
        //output the health stored for the dog, as this is the only value on the spreadsheet we need to access it from element 0
        Debug.Log(manager.WorkSheetData["Dog"].data["Health"][0]);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PublicSheetExample))]
public class LevelScriptEditor : Editor
{
    PublicSpreadSheetManager m;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("See Badgers Items"))
        {
            m = new PublicSpreadSheetManager();

            m.LoadPublicWorksheet("1GVXeyWCz0tCjyqE1GWJoayj92rx4a_hu4nQbYmW_PkE", "Sheet2", OnDataRetreved);
        }
    }

    void OnDataRetreved()
    {
        //loop through all the items that the badger has stored on the spreadsheet
        foreach (string s in m.WorkSheetData["Badger"].data["Items"])
        {
            Debug.Log(s);
        }
    }
}
#endif