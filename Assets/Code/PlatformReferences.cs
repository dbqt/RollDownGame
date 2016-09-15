using UnityEngine;
using System.Collections;

public class PlatformReferences : MonoBehaviour {

	public Transform[] Pieces;

	void Update () {

		if(this.gameObject.transform.position.y - GameManager.instance.gameObject.transform.position.y >= 6.5f) {
			GameManager.instance.NextPlatform();
			Destroy(this.gameObject);
		}
	}

	void FixedUpdate() {
		

		//this.gameObject.transform.Translate(Vector3.up*GameManager.instance.UpSpeed);
	}
}
