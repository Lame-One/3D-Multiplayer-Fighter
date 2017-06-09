using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpreadsheetManager : MonoBehaviour {

	PublicSpreadSheetManager manager;
	// Use this for initialization
	void Start()
	{
		manager = new PublicSpreadSheetManager();
		manager.LoadPublicWorksheet("1s_h56EmX3touNxVeBHjWyOrYfemSW6cNvgu_oeU0hCs", "Big Hit Guy", Callback);
	}

	void Callback()
	{
		//output the health stored for the dog, as this is the only value on the spreadsheet we need to access it from element 0
		Debug.Log(manager.WorkSheetData["WN"].data["Strength"][0]);
	}

	// Update is called once per frame
	void Update () {
		
	}
}