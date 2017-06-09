using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GoogleSheetsToUnity;

#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class Character : MonoBehaviour
{

	public string charName;
	protected DefaultPlayer thisPlayer;
	public Attack nW, nS, fW, fS, sW, sS, bW, bS, nWA, nSA, fWA, fSA, sWA, sSA, bWA, bSA, nSp, fSp, sSp, bSp, nASp, fASp, sASp, bASp;
	public List<HitBox> attackQueue;
	public virtual void Start()
	{
		thisPlayer = GetComponent<DefaultPlayer>();
	}

	public virtual void Update()
	{
		//Giant block just checking all the inputs. Currently: Face buttons, left joystick, and the right trigger (no left, because maybe targeting?)
		if (Input.GetAxis("Left Joy X") !=0 || Input.GetAxis("Left Joy Y") != 0 || 
			Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1) || 
			Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.JoystickButton3) || 
			Input.GetAxis("Right Trigger") != 0)
		{
			DidSomething();
		}
	}

	public virtual void DidSomething()
	{

	}
	
	public virtual void Cancel()
	{
		foreach (HitBox hb in attackQueue)
		{
			hb.GetComponent<HitBox>().Cancel();
		}
		attackQueue.Clear();
	}
	public virtual HitBox StandardAttack(Attack atk)
	{
		thisPlayer.SetStun(atk.lag);
		GameObject newHitBox = Instantiate(atk.HB, this.transform.forward + this.transform.position, this.transform.rotation);
		NetworkServer.Spawn(newHitBox);
		Destroy(newHitBox, atk.hbDuration + atk.delay);
		HitBox thisHB = newHitBox.GetComponent<HitBox>();
		thisHB.creator = this.gameObject;
		thisHB.Setup(atk);
		attackQueue.Add(thisHB);
		return (thisHB);
	}
	public virtual void NeutralWeakAttack()
	{
		StandardAttack(nW);
	}
	public virtual void NeutralStrongAttack()
	{
		StandardAttack(nS);
	}
	public virtual void ForwardWeakAttack()
	{
		StandardAttack(fW);
	}
	public virtual void ForwardStrongAttack()
	{
		StandardAttack(fS);
	}
	public virtual void SideWeakAttack()
	{
		StandardAttack(sW);
	}
	public virtual void SideStrongAttack()
	{
		StandardAttack(sS);
	}
	public virtual void BackWeakAttack()
	{
		StandardAttack(bW);
	}
	public virtual void BackStrongAttack()
	{
		StandardAttack(bS);
	}
	public virtual void NeutralWeakAirAttack()
	{
		StandardAttack(nWA);
	}
	public virtual void NeutralStrongAirAttack()
	{
		StandardAttack(nSA);
	}
	public virtual void ForwardWeakAirAttack()
	{
		StandardAttack(fWA);
	}
	public virtual void ForwardStrongAirAttack()
	{
		StandardAttack(fSA);
	}
	public virtual void SideWeakAirAttack()
	{
		StandardAttack(sWA);
	}
	public virtual void SideStrongAirAttack()
	{
		StandardAttack(sSA);
	}
	public virtual void BackWeakAirAttack()
	{
		StandardAttack(bWA);
	}
	public virtual void BackStrongAirAttack()
	{
		StandardAttack(bSA);
	}
	public virtual void NeutralSpecial()
	{
		StandardAttack(nSp);
	}
	public virtual void ForwardSpecial()
	{
		StandardAttack(fSp);
	}
	public virtual void SideSpecial()
	{
		StandardAttack(sSp);
	}
	public virtual void BackSpecial()
	{
		StandardAttack(bSp);
	}
	public virtual void NeutralAirSpecial()
	{
		NeutralSpecial();
	}
	public virtual void ForwardAirSpecial()
	{
		ForwardSpecial();
	}
	public virtual void SideAirSpecial()
	{
		SideSpecial();
	}
	public virtual void BackAirSpecial()
	{
		BackSpecial();
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
							c.nW = SetAttack(rData, c.nW);
							break;
						}
					case "Neutral Strong":
						{
							c.nS = SetAttack(rData, c.nS);
							break;
						}
					case "Forward Weak":
						{
							c.fW = SetAttack(rData, c.fW);
							break;
						}
					case "Forward Strong":
						{
							c.fS = SetAttack(rData, c.fS);
							break;
						}
					case "Side Weak":
						{
							c.sW = SetAttack(rData, c.sW);
							break;
						}
					case "Side Strong":
						{
							c.sS = SetAttack(rData, c.sS);
							break;
						}
					case "Back Weak":
						{
							c.bW = SetAttack(rData, c.bW);
							break;
						}
					case "Back Strong":
						{
							c.bS = SetAttack(rData, c.bS);
							break;
						}
					case "Neutral Weak Air":
						{
							c.nWA = SetAttack(rData, c.nWA);
							break;
						}
					case "Neutral Strong Air":
						{
							c.nSA = SetAttack(rData, c.nSA);
							break;
						}
					case "Forward Weak Air":
						{
							c.fWA = SetAttack(rData, c.fWA);
							break;
						}
					case "Forward Strong Air":
						{
							c.fSA = SetAttack(rData, c.fSA);
							break;
						}
					case "Side Weak Air":
						{
							c.sWA = SetAttack(rData, c.sWA);
							break;
						}
					case "Side Strong Air":
						{
							c.sSA = SetAttack(rData, c.sSA);
							break;
						}
					case "Back Weak Air":
						{
							c.bWA = SetAttack(rData, c.bWA);
							break;
						}
					case "Back Strong Air":
						{
							c.bSA = SetAttack(rData, c.bSA);
							break;
						}
					case "Neutral Special":
						{
							c.nSp = SetAttack(rData, c.nSp);
							break;
						}
					case "Forward Special":
						{
							c.fSp = SetAttack(rData, c.fSp);
							break;
						}
					case "Side Special":
						{
							c.sSp = SetAttack(rData, c.sSp);
							break;
						}
					case "Back Special":
						{
							c.bSp = SetAttack(rData, c.bSp);
							break;
						}
					case "Neutral Air Special":
						{
							c.nASp = SetAttack(rData, c.nASp);
							break;
						}
					case "Forward Air Special":
						{
							c.fASp = SetAttack(rData, c.fASp);
							break;
						}
					case "Side Air Special":
						{
							c.sASp = SetAttack(rData, c.sASp);
							break;
						}
					case "Back Air Special":
						{
							c.bASp = SetAttack(rData, c.bASp);
							break;
						}
				}
			}
			EditorUtility.SetDirty(target);
		}

		Attack SetAttack(RowData rData, Attack atk)
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
								atk.hbDuration = float.Parse(rData.cells[i].value);
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
			if (atk.HB == null)
			{
				atk.HB = c.nW.HB;
			}
			return (atk);
		}
	}
#endif
}
