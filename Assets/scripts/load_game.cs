using UnityEngine;
using System.Collections;

public class load_game : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void load_level() {
		Application.LoadLevel("hospital_bed");
		
	}

	public void load_credits() {
		Application.LoadLevel("credits");

	}

	public void load_title() {
		Application.LoadLevel("title");

	}
}
