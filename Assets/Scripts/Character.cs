using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using GoogleSheetsToUnity;


#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class Character : MonoBehaviour
{
	
	public string charName;
	public Attack nW, nS, fW, fS, sW, sS, bW, bS, nWA, nSA, fWA, fSA, sWA, sSA, bWA, bSA, nWS, nSS, fWS, fSS, sWS, sSS, bWS, bSS, nWAS, nSAS, fWAS, fSAS, sWAS, sSAS, bWAS, bSAS;
	public virtual void standardAttack(Attack atk)
	{
		
		GetComponent<DefaultPlayer>().setStun(atk.lag);
		GameObject newHitBox = Instantiate(atk.HB, this.transform.forward + this.transform.position, this.transform.rotation);
		NetworkServer.Spawn(newHitBox);
		Destroy(newHitBox, atk.duration + atk.delay);
		HitBox thisHB = newHitBox.GetComponent<HitBox>();
		thisHB.creator = this.gameObject;
		thisHB.setup(atk);

	}
	public virtual void neutralWeakAttack()
	{
		standardAttack(nW);
	}
	public virtual void neutralStrongAttack()
	{
		standardAttack(nS);
	}
	public virtual void forwardWeakAttack()
	{
		standardAttack(fW);
	}
	public virtual void forwardStrongAttack()
	{
		standardAttack(fS);
	}
	public virtual void sideWeakAttack()
	{
		standardAttack(sW);
	}
	public virtual void sideStrongAttack()
	{
		standardAttack(sS);
	}
	public virtual void backWeakAttack()
	{
		standardAttack(bW);
	}
	public virtual void backStrongAttack()
	{
		standardAttack(bS);
	}
	public virtual void neutralWeakAirAttack()
	{
		standardAttack(nWA);
	}
	public virtual void neutralStrongAirAttack()
	{
		standardAttack(nSA);
	}
	public virtual void forwardWeakAirAttack()
	{
		standardAttack(fWA);
	}
	public virtual void forwardStrongAirAttack()
	{
		standardAttack(fSA);
	}
	public virtual void sideWeakAirAttack()
	{
		standardAttack(sWA);
	}
	public virtual void sideStrongAirAttack()
	{
		standardAttack(sSA);
	}
	public virtual void backWeakAirAttack()
	{
		standardAttack(bWA);
	}
	public virtual void backStrongAirAttack()
	{
		standardAttack(bSA);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(Character), true)]
	public class CharacterEditor : Editor
	{
		SpreadSheetManager m;
		Character c;
		private GameObject defaultHitBox;

		void OnEnable()
		{
			c = (Character)target;
		}
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Update"))
			{
				UpdateStats();
			}
		}
		void UpdateStats()
		{
			SpreadSheetManager manager = new SpreadSheetManager();
			GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Character Data").LoadWorkSheet(c.charName);
			WorksheetData data = worksheet.LoadAllWorksheetInformation();
			Debug.Log("Starting nW.Dam = " + c.nW.damage);
			RowData rData = null;
			for (int i = 0; i < data.rows.Count; i++)
			{
				rData = data.rows[i];
				switch (data.rows[i].rowTitle)
				{
					case "Neutral Weak":
						{
							c.nW = setAttack(rData, c.nW);
							break;
						}
					case "Neutral Strong":
						{
							c.nS = setAttack(rData, c.nS);
							break;
						}
					case "Forward Weak":
						{
							c.fW = setAttack(rData, c.fW);
							break;
						}
					case "Forward Strong":
						{
							c.fS = setAttack(rData, c.fS);
							break;
						}
					case "Side Weak":
						{
							c.sW = setAttack(rData, c.sW);
							break;
						}
					case "Side Strong":
						{
							c.sS = setAttack(rData, c.sS);
							break;
						}
					case "Back Weak":
						{
							c.bW = setAttack(rData, c.bW);
							break;
						}
					case "Back Strong":
						{
							c.bS = setAttack(rData, c.bS);
							break;
						}
					case "Neutral Weak Air":
						{
							c.nWA = setAttack(rData, c.nWA);
							break;
						}
					case "Neutral Strong Air":
						{
							c.nSA = setAttack(rData, c.nSA);
							break;
						}
					case "Forward Weak Air":
						{
							c.fWA = setAttack(rData, c.fWA);
							break;
						}
					case "Forward Strong Air":
						{
							c.fSA = setAttack(rData, c.fSA);
							break;
						}
					case "Side Weak Air":
						{
							c.sWA = setAttack(rData, c.sWA);
							break;
						}
					case "Side Strong Air":
						{
							c.sSA = setAttack(rData, c.sSA);
							break;
						}
					case "Back Weak Air":
						{
							c.bWA = setAttack(rData, c.bWA);
							break;
						}
					case "Back Strong Air":
						{
							c.bSA = setAttack(rData, c.bSA);
							break;
						}
					case "Neutral Weak Special":
						{
							c.nW = setAttack(rData, c.nWS);
							break;
						}
					case "Neutral Strong Special":
						{
							c.nS = setAttack(rData, c.nSS);
							break;
						}
					case "Forward Weak Special":
						{
							c.fW = setAttack(rData, c.fWS);
							break;
						}
					case "Forward Strong Special":
						{
							c.fS = setAttack(rData, c.fSS);
							break;
						}
					case "Side Weak Special":
						{
							c.sW = setAttack(rData, c.sWS);
							break;
						}
					case "Side Strong Special":
						{
							c.sS = setAttack(rData, c.sSS);
							break;
						}
					case "Back Weak Special":
						{
							c.bW = setAttack(rData, c.bWS);
							break;
						}
					case "Back Strong Special":
						{
							c.bS = setAttack(rData, c.bSS);
							break;
						}
					case "Neutral Weak Air Special":
						{
							c.nWA = setAttack(rData, c.nWAS);
							break;
						}
					case "Neutral Strong Air Special":
						{
							c.nSA = setAttack(rData, c.nSAS);
							break;
						}
					case "Forward Weak Air Special":
						{
							c.fWA = setAttack(rData, c.fWAS);
							break;
						}
					case "Forward Strong Air Special":
						{
							c.fSA = setAttack(rData, c.fSAS);
							break;
						}
					case "Side Weak Air Special":
						{
							c.sWA = setAttack(rData, c.sWAS);
							break;
						}
					case "Side Strong Air Special":
						{
							c.sSA = setAttack(rData, c.sSAS);
							break;
						}
					case "Back Weak Air Special":
						{
							c.bWA = setAttack(rData, c.bWAS);
							break;
						}
					case "Back Strong Air Special":
						{
							c.bSA = setAttack(rData, c.bSAS);
							break;
						}
				}	
			}

			EditorUtility.SetDirty(target);
		}

		Attack setAttack(RowData rData, Attack atk)
		{
			if (rData != null)
			{
				for (int i = 0; i < rData.cells.Count; i++)
				{
					switch (rData.cells[i].cellColumTitle)
					{
						case "damage":
							{
								Debug.Log("Recieved: " + rData.cells[i].value);
								atk.damage = float.Parse(rData.cells[i].value);
								Debug.Log("Set to: " + atk.damage);
								break;
							}
						case "knockback":
							{
								atk.knockback = float.Parse(rData.cells[i].value);
								break;
							}
						case "vertical":
							{
								atk.vertical = float.Parse(rData.cells[i].value);
								break;
							}
						case "horizontal":
							{
								atk.horizontal = float.Parse(rData.cells[i].value);
								break;
							}
						case "duration":
							{
								atk.duration = float.Parse(rData.cells[i].value);
								break;
							}
						case "lag":
							{
								atk.lag = float.Parse(rData.cells[i].value);
								break;
							}
						case "stun":
							{
								atk.stun = float.Parse(rData.cells[i].value);
								break;
							}
						case "delay":
							{
								atk.delay = float.Parse(rData.cells[i].value);
								break;
							}
						default:
							{
								break;
							}
					}
				}
			}
			else
			{
				Debug.Log("No Data Found");
			}
			if(atk.HB == null)
			{
				atk.HB = c.nW.HB;
			}
			return (atk);
		}
	}
}
#endif