using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
public class Player : MonoBehaviour {
	public static int max_number_of_walls = 15;
	public const int MAX_INVENTORY = 7;  // real max of 14
	public float speed = 1f;
	public int energy = 20;
	public int energy_step = 5;
	public Interactions Interactions;
	GameObject healthBar;
	GameObject scoreText;
	GameObject healthText;
	GameObject prevBrick;
	public GameObject prey;
	GameObject energyBar;
	bool ogreIsAboutToDie = false;
	int score = 0;
	private bool facingRight = true;
	public static Button[] InventoryArray = new Button[MAX_INVENTORY];
	static bool created = false;
	private static Player playerInstance;
	private string[] weapons = {
		"phaser", 
		"sword",
		"club",
		"rock",
		"rifle",
		"crossbow",
		"grenade",
		"sling",
		"spear",
		"hotdog",
		"crystal"
	};

	IEnumerator yieldConnect()
	{
		while(true)
		{

			if (!GameState.gameOver) {
				Text t = scoreText.GetComponent<Text> ();
				t.text = "";
				t.text = "Score:  " + GameState.score++;
			}

			Text j = healthText.GetComponent<Text>();
			j.text = "";
			j.text = "Health:  " + GameState.playerHP;
			//Debug.Log("fill amount " + img.fillAmount);
			yield return new WaitForSeconds(1);
		}
	}

	void Awake() {
		DontDestroyOnLoad (gameObject);
		if (playerInstance == null) {
			playerInstance = this;
		} else {
			DestroyObject(gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		scoreText = GameObject.Find("score");
		healthText = GameObject.Find("judyhealth");
//		var brickText = GameObject.Find("BrickText");
		GameState.player = gameObject;

		StartCoroutine(yieldConnect());

	}
	void OnCollisionEnter2D(Collision2D col)
	{
//		if (col.gameObject.tag == "prey")
//		{
//			//AudioSource.PlayClipAtPoint(prey.audio.clip, transform.position);
//			prey.GetComponent<AudioSource>().Play();
//			Destroy(col.gameObject);  
////			Image img = healthBar.GetComponent<Image>();
////			img.fillAmount =  img.fillAmount + 0.2f;
//			energy = energy + energy_step;			
//			Text energyLevelText = healthBar.GetComponent<Text>();
//			energyLevelText.text = "" + energy;
//			Image img = energyBar.GetComponent<Image>();
//			img.fillAmount = img.fillAmount + .01f * energy_step;
//
//
//
//
//		}
	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		var tag = coll.gameObject.tag;
		if (tag == "portkey") {
			// don't inventory the portal itself
			return;
		}

         
		if (weapons.Contains (tag)) {


			for (int i = 1; i <= MAX_INVENTORY; i++) {
				Button inventorySlot = GameObject.Find ("InventoryButton" + i).GetComponent<UnityEngine.UI.Button> ();
			 
				// now check if available
				if (inventorySlot.tag == "available") {
					inventorySlot.tag = tag;
					Debug.Log ("tag getting assigned " + tag);
					inventorySlot.image.sprite = coll.gameObject.GetComponent<SpriteRenderer> ().sprite;//Resources.Load<Sprite>("Sprites/sword");
					InventoryArray [i-1] = inventorySlot;

					DestroyObject (coll.gameObject);				
					break;
				}
			}



		}



	}
	void Flip()
	{
		//Debug.Log("switching...");
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
//		transform.rotation = rot;
//		transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z);
//		GetComponent<Rigidbody2D>().angularVelocity = 0;

//		float input = Input.GetAxis("Vertical");
//		GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed * input);

		if (!Interactions.inDialogue) {
			
			float movex = Input.GetAxis ("Horizontal");
			float movey = Input.GetAxis ("Vertical");
			// anim.SetFloat("Speed", Mathf.Abs(move));
			if (movex > 0 && !facingRight) {
				Flip ();
			} else if (movex < 0 && facingRight) {
				Flip ();
			}
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (movex * speed, movey * speed);
			//		if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) // left or right
			//		{
			//			if (Wall.NumberOfWallsLeft() <= max_number_of_walls)
			//			{
			//				var brickText = GameObject.Find("BrickText");
			//				Text t = brickText.GetComponent<Text>();
			//				
			//				t.text = "" + (max_number_of_walls - Wall.NumberOfWallsLeft());
			//
			//				Vector3 offset = new Vector3(-.1f,.1f,0f);
			//				GameObject newBrick = (GameObject) Instantiate(brick,transform.position - offset	,transform.rotation);
			//				Physics2D.IgnoreCollision(newBrick.GetComponent<Collider2D>(), GetComponent<Collider2D>(),true);
			//				if (prevBrick != null)
			//				{
			//					Physics2D.IgnoreCollision(newBrick.GetComponent<Collider2D>(), prevBrick.GetComponent<Collider2D>(),true);
			//				}
			//				prevBrick = newBrick;
			//
			//			}
			//			//				tree.rigidbody2D.AddRelativeForce(new Vector2(10,6));
			//			//newBrick.rigidbody2D.AddForce(new Vector2(20,20));
			//		}


		}
	}
}
