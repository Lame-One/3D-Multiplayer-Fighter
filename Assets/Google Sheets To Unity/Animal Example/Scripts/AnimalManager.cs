using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using UnityEngine.UI;

/// <summary>
/// example script to show realtime updates of multiple items
/// </summary>
public class AnimalManager : MonoBehaviour
{
    public AnimalContainer container;

    public bool updateOnPlay;

    void Awake()
    {
        //if true will update all animals in the animal container, this can be expensive to do at runtime but will always ensure the most updated values.
        //recomend to use behind a loading screen.
        if (updateOnPlay)
        {
            SpreadSheetManager manager = new SpreadSheetManager();
            GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Animal Stats").LoadWorkSheet("Stats");
            WorksheetData data = worksheet.LoadAllWorksheetInformation();

                for (int i = 0; i < data.rows.Count; i++)
                {
                    Animal animal = container.allAnimals.Find(x => x.name == data.rows[i].rowTitle);

                    if (animal != null)
                    {
                        animal.UpdateStats(data.rows[i]);
                    }
                }
        }
    }
}
