using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mapTracker : MonoBehaviour {

	public string travelMode = "firstTime";
	public int location = 0;
	public int newlocation = 0;

	// Use this for initialization
	void Start () {

		//showPlanets ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[System.Serializable]
	public class Planet
	{
		public bool locked;
		public bool visited;
		public Image planetImage; 
		public int[] accessiblePlanets;
	}

	public Planet[] planet;


	void glowLocation(){
		GameObject.FindWithTag("planet" + location).GetComponent<Image> ().color = Color.green;
	}

	public void travelNewPlanet(int destination){
		GameObject.FindWithTag("planet" + location).GetComponent<Image> ().color = Color.magenta; //cause previous location to change color
		hidePlanets ();
		location = destination;
		//planet [location].visited = true;
		glowLocation ();
		for (int i = 0; i < planet.Length; i++) {
			if (System.Array.IndexOf (planet [location].accessiblePlanets, i) != -1 || i == location) {
				unlock (i);
			} 
				

		}
	}


	public void unvisitPlanets(){
		for (int i = 0; i < planet.Length; i++){
			planet [i].visited = false;
		}
	}

	public void showPlanets(){
		for (int i = 0; i < planet.Length; i++){
			if (planet[i].locked == false)
				reveal (i);
		}
	}

	public void hidePlanets(){
		for (int i = 0; i < planet.Length; i++){
			planet [i].planetImage.enabled = false;
		}
	}

	void lockPlanet(int thisPlanet){
		planet [thisPlanet].locked = true;
	}
	void unlock(int thisPlanet){
		planet [thisPlanet].locked = false;
	}

	void unlockAll(){
		for (int i = 0; i<planet.Length; i++){
			unlock (i);
		}

	}

	void reveal(int thisPlanet){
		planet [thisPlanet].planetImage.enabled = true;
	}

	void revealAllPlanets(){
		for (int i =0; i<planet.Length; i++){
			reveal (i);
		}
	}//revealAllPlanets


}//mapTracker
