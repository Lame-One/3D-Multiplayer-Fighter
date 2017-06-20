using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

	public Attack atk;
	public GameObject creator;
	public Vector3 knockDir;
	private float timeOn;
	public bool isBlockable = true;

	// Use this for initialization
	void Start () {
	}
	
	public void Setup(Attack incAtk)
	{
		atk = incAtk;
		knockDir = this.transform.forward;
		knockDir = Quaternion.AngleAxis(-atk.vertical, creator.transform.right) * knockDir;
		knockDir = Quaternion.AngleAxis(atk.horizontal, creator.transform.up) * knockDir;
		timeOn = Time.time + atk.delay;
	}
	// Update is called once per frame
	void Update () {
		if(Time.time > timeOn)
		{
			GetComponent<Collider>().enabled = true;
			GetComponent<Renderer>().enabled = true;
		}
	}

	private void OnDestroy()
	{
	}
	public void Cancel()
	{
		Destroy(this);
		creator.GetComponent<Character>().attackQueue.Remove(this);
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != creator && other.CompareTag("Player"))
		{
			other.gameObject.GetComponent<DefaultPlayer>().GetHit(atk.damage, atk.knockback, knockDir, atk.stun, isBlockable);
		}
	}

}
