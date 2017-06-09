using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attack
{
	public float damage, knockback, vertical, horizontal, hbDuration, lag, stun, delay;
	
	public GameObject HB;

	public string GetInfo()
	{
		return ("Damage: " + damage + ".  Knockback: " + knockback + ".  Vertical: " + vertical + ".  Horizontal: " + horizontal + ".  Duration: " + hbDuration + ".  Lag: " + lag + ".  Stun: " + stun + ".  Delay: " + delay);
	}
}
