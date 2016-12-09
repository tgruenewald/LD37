using UnityEngine;
using System.Collections;

public class itemContact : MonoBehaviour {
	public bool contact = false;
	public Interactions Interactions;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown("Jump") && contact && !Interactions.inDialogue && !Interactions.dialogueComplete)
		{
			Debug.Log ("itemContact test " + Interactions.inDialogue + Interactions.dialogueComplete);

			Interactions.showText (this.tag);
		}




	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		contact = true;

	}

	void OnTriggerExit2D(Collider2D coll)
	{
		contact = false;
	}
}
