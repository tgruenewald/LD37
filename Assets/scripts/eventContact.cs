using UnityEngine;
using System.Collections;

public class eventContact : MonoBehaviour {
	Interactions interactions;
	public eventContact eventcontact = null;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("collided");
		interactions = GameObject.Find ("judy").GetComponent<Player> ().Interactions;
		interactions.showText (this.tag);

	}

}
