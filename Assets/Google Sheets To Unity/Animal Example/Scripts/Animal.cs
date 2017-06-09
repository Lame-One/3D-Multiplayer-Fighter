using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Animal : ScriptableObject
{
    public int health;
    public int attack;
    public int defence;

    /// <summary>
    /// update the stats based on the data sent in
    /// all cellColumTitle names must be in lowercase and with no spaces
    /// </summary>
    /// <param name="rData">data for the selected object</param>
    public void UpdateStats(RowData rData)
    {
        for (int i = 0; i < rData.cells.Count; i++)
        {
            switch (rData.cells[i].cellColumTitle)
            {
                case "health":
                    {
                        health = int.Parse(rData.cells[i].value);
                        break;
                    }
                case "attack":
                    {
                        attack = int.Parse(rData.cells[i].value);
                        break;
                    }
                case "defence":
                    {
                        defence = int.Parse(rData.cells[i].value);
                        break;
                    }
            }
        }
    }
}


//Custom editior to provide additional features
//#if UNITY_EDITOR
[CustomEditor(typeof(Animal))]
public class AnimalEditor : Editor
{
    Animal animal;

    void OnEnable()
    {
        animal = (Animal)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update"))
        {
            UpdateStats();
        }

        if (GUILayout.Button("Add"))
        {
            SendAnimalDataToSheet();
        }
    }

    /// <summary>
    /// Loads the revelant sheet and searches for the information for this asset
    /// if you want to access a different sheet replace "Animal Stats" and "Stats" with the revelant spreadsheet title and sheet name
    /// </summary>
    void UpdateStats()
    {
        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Animal Stats").LoadWorkSheet("Stats");
        WorksheetData data = worksheet.LoadAllWorksheetInformation();

        RowData rData = null;

        for (int i = 0; i < data.rows.Count; i++)
        {
            if (data.rows[i].rowTitle == animal.name
                )
            {
                rData = data.rows[i];
                break;
            }
        }

        if (rData != null)
        {
            animal.UpdateStats(rData);
        }
        else
        {
            Debug.Log("no found data");
        }

        EditorUtility.SetDirty(target);
    }

    /// <summary>
    /// Adds the new animal to the spreadsheet online
    /// if you want to access a different sheet replace "Animal Stats" and "Stats" with the revelant spreadsheet title and sheet name
    /// all title names must be in lowercase and contain no spaces
    /// </summary>
    void SendAnimalDataToSheet()
    {
        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Animal Stats").LoadWorkSheet("Stats");
        worksheet.AddRowData(new Dictionary<string, string>
        {
            //NOTE: all data names are in lower case
            {"name", animal.name},
            {"health", animal.health.ToString() },
            {"attack", animal.health.ToString() },
            {"defence", animal.health.ToString()},
        });
    }
}
//#endif
