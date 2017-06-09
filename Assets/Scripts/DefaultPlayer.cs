using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DefaultPlayer : NetworkBehaviour
{

	public float speed = 3.0f, jumpPower = 200f, gravity = 10f, drag = .8f, curDam, airDrag = .2f, rotSpeed = 150f, stunDone;
	public bool grounded, doubleJump, targetting;
	private bool curStunned = false;
	public GameObject target, dummy;

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
		curStunned = (Time.time < stunDone);
		if(curStunned)
		{
			return;
		}

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

		if (Input.GetKeyDown(KeyCode.JoystickButton6))
		{
			GameObject newPlayer = Instantiate(dummy, this.transform.forward + this.transform.position, this.transform.rotation);
			NetworkServer.Spawn(newPlayer);
		}
	}
	void DoWeakAttacks()
	{
		if(grounded)
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Weak - " + thisChar.nW.getInfo());
				thisChar.neutralWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Weak - " + thisChar.sW.getInfo());
				thisChar.sideWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Weak - " + thisChar.fW.getInfo());
				thisChar.forwardWeakAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Weak - " + thisChar.bW.getInfo());
				thisChar.backWeakAttack();
			}
		}
		else
		{

			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral WeakAir - " + thisChar.nWA.getInfo());
				thisChar.neutralWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side WeakAir - " + thisChar.sWA.getInfo());
				thisChar.sideWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward WeakAir - " + thisChar.fWA.getInfo());
				thisChar.forwardWeakAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back WeakAir - " + thisChar.bWA.getInfo());
				thisChar.backWeakAirAttack();
			}
		}
	}
	void DoStrongAttacks()
	{
		if(grounded)
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral Strong - " + thisChar.nS.getInfo());
				thisChar.neutralStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side Strong - " + thisChar.sS.getInfo());
				thisChar.sideStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward Strong - " + thisChar.fS.getInfo());
				thisChar.forwardStrongAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back Strong - " + thisChar.bS.getInfo());
				thisChar.backStrongAttack();
			}
		}
		else
		{
			if (Input.GetAxis("Left Joy X") == 0 && Input.GetAxis("Left Joy Y") == 0)
			{
				Debug.Log("Neutral StrongAir - " + thisChar.nSA.getInfo());
				thisChar.neutralStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") > Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y"))
			{
				Debug.Log("Side StrongAir - " + thisChar.sSA.getInfo()); ;
				thisChar.sideStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") < 0)
			{
				Debug.Log("Forward StrongAir - " + thisChar.fSA.getInfo());
				thisChar.forwardStrongAirAttack();
			}
			else if (Input.GetAxis("Left Joy X") * Input.GetAxis("Left Joy X") < Input.GetAxis("Left Joy Y") * Input.GetAxis("Left Joy Y") && Input.GetAxis("Left Joy Y") > 0)
			{
				Debug.Log("Back StrongAir - " + thisChar.bSA.getInfo());
				thisChar.backStrongAirAttack();
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
		if (curStunned)
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
			if (thisRigidbody.velocity != new Vector3(0, 0, 0))
			{
				thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x * airDrag, thisRigidbody.velocity.y, thisRigidbody.velocity.z * airDrag);

			}
		}
		else if (thisRigidbody.velocity != new Vector3(0, 0, 0))
		{
			thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x * drag, thisRigidbody.velocity.y, thisRigidbody.velocity.z * drag);
		}
		if (curStunned)
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

	public void getHit(float dmgTaken, float knockback, Vector3 direction, float stun)
	{
		curDam += dmgTaken;
		thisRigidbody.AddForce(direction * knockback * curDam);
		stunDone = Time.time + stun;
	}
	public void setStun(float tilDone)
	{
		stunDone = Time.time + tilDone;
	}
}