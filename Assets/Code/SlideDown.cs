using UnityEngine;
using System.Collections;

public class SlideDown : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.gameObject.transform.Translate(Vector3.down*GameManager.instance.GetUpSpeed());
		
	}
}
