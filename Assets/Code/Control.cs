using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	public float Movespeed;
	public GameObject heart;
	public Material playerMat;

	private bool rainbowColor;
	private Color startColor;
	private Color endColor;
	private float counter;
	private Color[] allColors;
	// Use this for initialization
	void Start () {
		allColors = new Color[]{Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow};
		heart.SetActive(false);
		rainbowColor = false;
		if(PlayerPrefs.GetInt("ActiveMaterial") == 6)
		{
			//rainbow
			rainbowColor = true;
			counter = 0f;
			startColor = allColors[Mathf.FloorToInt(Random.Range(0f,allColors.Length))];
			ChangeColor();

		}
		if(PlayerPrefs.GetInt("ActiveMaterial") == 7)
		{
			//hearts
			heart.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int controlType = PlayerPrefs.GetInt("Control");

		Vector3 v = Vector3.zero;

		// axis
		if(controlType == -1 && Input.GetAxis("Horizontal") != 0f) {
			v.x = Mathf.Sign(Input.GetAxis("Horizontal"))*Movespeed;		
		}

		//touch
		if(controlType == 0 && Input.GetMouseButton(0)) {
			Vector3 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(this.transform.position.x > touchPoint.x)
			{
				v.x = -Movespeed;
			}
			if(this.transform.position.x < touchPoint.x)
			{
				v.x = Movespeed;
			}	
		}

		// tilt
		if(controlType == 1) {
			float rawValue = Input.acceleration.x;
			if(rawValue >= 0.2f) rawValue = 1;
			else if(rawValue <= -0.2f) rawValue = -1;
			else rawValue = 0f;
			v.x = rawValue*Movespeed;
		}
		if(rainbowColor){
			if(counter >= 1f)
			{
				ChangeColor();
			}
			playerMat.color = Color.Lerp(startColor, endColor, counter);
			counter += Time.deltaTime;
		}
		this.gameObject.GetComponent<Rigidbody>().AddForce(v);
	}

	void OnTriggerEnter(Collider o)
	{
		if(o.tag == "Finish")
		{
			//Debug.Log("lose");
			GameManager.instance.EndGame();
		}
	}

	void OnTriggerExit(Collider o)
	{
		if(o.tag == "PlatformScore")
		{
			int n = PlayerPrefs.GetInt("CurrentScore");
			PlayerPrefs.SetInt("CurrentScore", n+1);
		}
	}

	private void ChangeColor ()
	{
		startColor = playerMat.color;
		endColor = allColors[Mathf.FloorToInt(Random.Range(0f,allColors.Length))];
		counter = 0f;
	}
}
