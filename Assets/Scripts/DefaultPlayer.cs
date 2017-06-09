using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DefaultPlayer : NetworkBehaviour
{
	public float speed = 3.0f, jumpPower = 200f, gravity = 10f, drag = .8f, curDam, airDrag = .2f, rotSpeed = 150f, stunDone;
	public bool grounded, doubleJump, targetting;
	public bool isStunned= false, isArmored = false, isBlocking = false, isDashing = false;
	public GameObject target, dummy;
	private float superArmorDone = 0.0f;
	private Character thisChar;
	private Rigidbody thisRigidbody;
	void Start()
	{
		grounded = true;
		doubleJump = true;
		curDam = 0;
		thisChar = GetComponent<Character>();
		thisRigidbody = GetComponent<Rigidbody>();
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Camera.main.GetComponent<CameraFollow>().transform.SetParent(gameObject.transform);
		Camera.main.GetComponent<CameraFollow>().transform.position = new Vector3(-2, 2, -4);
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isLocalPlayer) { return; }
		if (Time.time < stunDone)
		{
			return;
		}
		//Movement
		DoMovement();

		if (targetting)
		{
			transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
		}

	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		if(Time.time < stunDone)
		{
			return;
		}
		else
		{
			isStunned = false;
			isDashing = false;
		}
		isArmored =  Time.time < superArmorDone;
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (targetting)
			{
				targetting = false;
				speed *= 3;
			}
			else
			{
				targetting = true;
				speed /= 3;
			}
		}
		if ((grounded || doubleJump) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
		{
			DoJump();
		}
		if (Input.GetKeyDown(KeyCode.JoystickButton2))
		{
			DoWeakAttacks();
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton3))
		{
			DoStrongAttacks();
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			DoSpecialAttacks();
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton6))
		{
			GameObject newPlayer = Instantiate(dummy, this.transform.forward + this.transform.position, this.transform.rotation);
			NetworkServer.Spawn(newPlayer);
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log("Forward Special - " + thisChar.fSp.GetInfo());
			thisChar.ForwardSpecial();
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			Debug.Log("Back Special - " + thisChar.bSp.GetInfo());
			thisChar.BackSpecial();
		}
	}
	void DoWeakAttacks()
	{
		if(grounded)
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Weak - " + thisChar.nW.GetInfo());
				thisChar.NeutralWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Weak - " + thisChar.sW.GetInfo());
				thisChar.SideWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Weak - " + thisChar.fW.GetInfo());
				thisChar.ForwardWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Weak - " + thisChar.bW.GetInfo());
				thisChar.BackWeakAttack();
			}
		}
		else
		{

			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral WeakAir - " + thisChar.nWA.GetInfo());
				thisChar.NeutralWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side WeakAir - " + thisChar.sWA.GetInfo());
				thisChar.SideWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward WeakAir - " + thisChar.fWA.GetInfo());
				thisChar.ForwardWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back WeakAir - " + thisChar.bWA.GetInfo());
				thisChar.BackWeakAirAttack();
			}
		}
	}
	void DoStrongAttacks()
	{
		if(grounded)
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Strong - " + thisChar.nS.GetInfo());
				thisChar.NeutralStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Strong - " + thisChar.sS.GetInfo());
				thisChar.SideStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Strong - " + thisChar.fS.GetInfo());
				thisChar.ForwardStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Strong - " + thisChar.bS.GetInfo());
				thisChar.BackStrongAttack();
			}
		}
		else
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Strong Air - " + thisChar.nSA.GetInfo());
				thisChar.NeutralStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Strong Air - " + thisChar.sSA.GetInfo()); ;
				thisChar.SideStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Strong Air - " + thisChar.fSA.GetInfo());
				thisChar.ForwardStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back StrongAir - " + thisChar.bSA.GetInfo());
				thisChar.BackStrongAirAttack();
			}
		}
	}
	void DoSpecialAttacks()
	{
		if (grounded)
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Special - " + thisChar.nSp.GetInfo());
				thisChar.NeutralSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Special - " + thisChar.sSp.GetInfo());
				thisChar.SideSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Special - " + thisChar.fSp.GetInfo());
				thisChar.ForwardSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Special - " + thisChar.bSp.GetInfo());
				thisChar.BackSpecial();
			}
		}
		else
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Air Special - " + thisChar.nASp.GetInfo());
				thisChar.NeutralAirSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Air Special - " + thisChar.sASp.GetInfo()); ;
				thisChar.SideAirSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Air Special - " + thisChar.fASp.GetInfo());
				thisChar.ForwardAirSpecial();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Air Special - " + thisChar.bASp.GetInfo());
				thisChar.BackAirSpecial();
			}
		}
	}
	void OnCollisionEnter(Collision contacted)
	{
		if (contacted.gameObject.tag == "Floor")
		{
			doubleJump = true;
			grounded = true;
		}
	}

	void DoMovement()
	{

		if (((Camera.main.transform.eulerAngles.x < 90 || Camera.main.transform.eulerAngles.x > 330) && Input.GetAxis("Right Joy Y") < 0) || ((Camera.main.transform.eulerAngles.x > 300 || Camera.main.transform.eulerAngles.x < 45) && Input.GetAxis("Right Joy Y") > 0))
		{
			Camera.main.GetComponent<CameraFollow>().transform.RotateAround(gameObject.transform.position, Camera.main.transform.right, Input.GetAxis("Right Joy Y") * rotSpeed);
		}
		if (isStunned)
		{
			transform.Rotate(0, Input.GetAxis("Right Joy X") * rotSpeed *.5f, 0);
		}
		else
		{
			transform.Rotate(0, Input.GetAxis("Right Joy X") * rotSpeed, 0);
		}
		if (!grounded)
		{
			thisRigidbody.AddForce(Vector3.down * gravity);
			if (thisRigidbody.velocity != new Vector3(0, 0, 0) && !isDashing)
			{
				thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x * airDrag, thisRigidbody.velocity.y, thisRigidbody.velocity.z * airDrag);

			}
		}
		else if (thisRigidbody.velocity != new Vector3(0, 0, 0) && !isDashing)
		{
			thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x * drag, thisRigidbody.velocity.y, thisRigidbody.velocity.z * drag);
		}
		if (isStunned)
		{
			return;
		}
		if (grounded)
		{
			thisRigidbody.AddForce(Input.GetAxis("Left Joy X") * transform.right * speed);
			thisRigidbody.AddForce(Input.GetAxis("Left Joy Y") * -transform.forward * speed);
		}
		else
		{
			thisRigidbody.AddForce(Input.GetAxis("Left Joy X") * transform.right * speed * .2f);
			thisRigidbody.AddForce(Input.GetAxis("Left Joy Y") * transform.forward * -speed * .2f);
		}
	}

	void DoJump()
	{
		//jumping
		thisRigidbody.AddForce(Vector3.up * jumpPower);
		if (grounded) { grounded = false; }
		else
		{
			doubleJump = false;
			thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, thisRigidbody.velocity.z);
		}
	}

	public void GetHit(float dmgTaken, float knockback, Vector3 direction, float stun)
	{
		if(!isBlocking)
		{
			curDam += dmgTaken;
			if (!isArmored)
			{
				thisChar.Cancel();
				thisRigidbody.AddForce(direction * knockback * curDam * dmgTaken);
				stunDone = Time.time + stun;
			}
		}
	
	}

	public void SetSuperArmor(float duration)
	{
		superArmorDone = Time.time + duration;
	}
	public void SetStun(float duration)
	{
		stunDone = Time.time + duration;
	}

	public void SetDashing()
	{
		isDashing = true;
	}
}