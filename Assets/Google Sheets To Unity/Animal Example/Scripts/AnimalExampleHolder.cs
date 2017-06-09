using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleSheetsToUnity;

/// <summary>
/// example script to demo real time updates for a particular item
/// </summary>
public class AnimalExampleHolder : MonoBehaviour
{
    public Animal animal;

    public Text animalName;
    public Text animalHealth;
    public Text animalAttack;
    public Text animalDefence;

    public bool updateOnPlay;

    void Start()
    {
        //Update on play here is used as an example of real time data retreval, it is not recomended to do a lot of calls on begin, if needed group it to one larger call
        //See the UpdateAllAnimals.cs on how to do a batch update in the editor or AnimalManager.cs for a runtime example
        if(updateOnPlay)
        {
            SpreadSheetManager manager = new SpreadSheetManager();
            GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Animal Stats").LoadWorkSheet("Stats");
            WorksheetData data = worksheet.LoadAllWorksheetInformation();
            RowData rData = null;

            for (int i = 0; i < data.rows.Count; i++)
            {
                if (data.rows[i].rowTitle == animal.name)
                {
                    rData = data.rows[i];
                    break;
                }
            }

            if (rData != null)
            {
                Debug.Log("updating animal stats " + animal.name);
                animal.UpdateStats(rData);
            }
            else
            {
                Debug.Log("no found data");
            }
        }

        //show the stats on the canvas
        animalName.text = animal.name;
        animalHealth.text = string.Format(animalHealth.text, animal.health.ToString());
        animalAttack.text = string.Format(animalAttack.text, animal.attack.ToString());
        animalDefence.text = string.Format(animalDefence.text, animal.defence.ToString());
    }
}
