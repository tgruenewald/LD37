using UnityEngine;
using System.Collections;

public class eventSystem : MonoBehaviour {
	public bool enemyContact = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("E"))
			{
				Debug.Log("E pressed");
			}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		enemyContact = true;

	}

	void OnTriggerExit2D(Collider2D coll)
	{
		enemyContact = false;
	}

}
