using UnityEngine;
using System.Collections;

public class reload_scene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void reload() {
		Debug.Log("-----reloading scene");
		Application.LoadLevel("title");
	}
}
