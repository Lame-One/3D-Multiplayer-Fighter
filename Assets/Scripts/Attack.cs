using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attack
{
	public float damage, knockback, vertical, horizontal, duration, lag, stun, delay;
	
	public GameObject HB;

	public string getInfo()
	{
		return ("Damage: " + damage + ".  Knockback: " + knockback + ".  Vertical: " + vertical + ".  Horizontal: " + horizontal + ".  Duration: " + duration + ".  Lag: " + lag + ".  Stun: " + stun + ".  Delay: " + delay);
	}
}
