using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GoogleSheetsToUnity;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BigHitGuy : Character
{
	public float swordStrength = 100f;
	private bool isCharging;
	private float dashStartTime = 0f, dashEndTime = 0f, chargeSpeed = 1f, maxCharge = 300f;

	// Use this for initialization
	public override void Start()
	{
		base.Start();
	}
	public override void Update()
	{
		base.Update();
		if (isCharging && swordStrength < maxCharge)
		{
			swordStrength += 1f;
		}
		else if (isCharging && swordStrength > maxCharge)
		{
			swordStrength = maxCharge;
		}
		else if (swordStrength > 100f && isCharging == false)
		{
			swordStrength -= .1f;
		}
		if(dashStartTime < Time.time && dashStartTime != 0)
		{
			dashStartTime = 0;
			Dash();
		}
		if(dashEndTime < Time.time && dashEndTime != 0)
		{
			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			dashEndTime = 0;
		}
	}

	public override void DidSomething()
	{
		if (isCharging)
		{
			StopCharging();
		}
	}
	public override HitBox StandardAttack(Attack atk)
	{
		Attack buffedAtk = atk;
		buffedAtk.damage *= swordStrength / 100f;
		thisPlayer.SetStun(atk.lag);
		GameObject newHitBox = Instantiate(atk.HB, this.transform.forward + this.transform.position, this.transform.rotation);
		NetworkServer.Spawn(newHitBox);
		Destroy(newHitBox, atk.hbDuration + atk.delay);
		HitBox thisHB = newHitBox.GetComponent<HitBox>();
		thisHB.creator = this.gameObject;
		thisHB.Setup(buffedAtk);
		return (thisHB);
	}
	public override void BackSpecial()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		IEnumerator chargeCoroutine;
		chargeCoroutine = StartCharging();
		StartCoroutine(chargeCoroutine);
		thisPlayer.SetStun(300f);
	}
	public override void ForwardSpecial()
	{
		dashStartTime = Time.time + fSp.delay;
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		HitBox thisHB = StandardAttack(fSp);
		thisHB.transform.SetParent(transform);
		thisPlayer.SetStun(fSp.delay);
	}
	public override void NeutralSpecial()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}

	private void Dash()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * chargeSpeed * swordStrength;
		thisPlayer.SetDashing();
		dashEndTime = Time.time + fSp.hbDuration;
		thisPlayer.SetSuperArmor(fSp.hbDuration);
		swordStrength *= .75f;
		if (swordStrength < 100f)
		{
			swordStrength = 100f;
		}
	}

	private void StopCharging()
	{
		isCharging = false;
		thisPlayer.SetStun(bSp.lag);
	}
	
	private IEnumerator StartCharging()
	{
		yield return new WaitForSeconds(bSp.delay);
		isCharging = true;
	}
}
