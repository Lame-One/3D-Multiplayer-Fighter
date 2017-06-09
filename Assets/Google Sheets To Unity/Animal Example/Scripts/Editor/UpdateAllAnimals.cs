using UnityEngine;
using System.Collections;
using UnityEditor;
using GoogleSheetsToUnity;

public class UpdateAllAnimals : MonoBehaviour
{
    [MenuItem("Update/Update All Animals")]
    static void DoSomething()
    {
        UpdateAnimals();
    }

    static void UpdateAnimals()
    {
        //load the animal container from resources
        AnimalContainer container = Resources.Load<AnimalContainer>("AnimalContainer");

        //load the revelant spreadsheet
        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Animal Stats").LoadWorkSheet("Stats");
        WorksheetData data = worksheet.LoadAllWorksheetInformation();

        //loop through all the animals on the spreadsheet, if an animal is found then update their stats
        for(int i = 0; i < data.rows.Count; i++)
        {
            Animal animal = container.allAnimals.Find(x => x.name == data.rows[i].rowTitle);

            //if an animal can not be found then create an asset for that animal
            if(animal == null)
            {
                animal = new Animal();
                animal.name = data.rows[i].rowTitle;
                AssetDatabase.CreateAsset(animal, "Assets/Animal Example/Animal Stats/" + animal.name + ".asset");
                container.allAnimals.Add(animal);

                Debug.Log("Creating animal asset for " + animal.name);
            }

            //update the animals stats
            animal.UpdateStats(data.rows[i]);
            Debug.Log("Updating Stats for " + animal.name);
        }
    }
}
