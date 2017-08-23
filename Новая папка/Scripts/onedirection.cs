using UnityEngine;
using System.Collections;

public class onedirection : MonoBehaviour {

    public float speed;
    public Vector3 direction = Vector3.up;

	// Use this for initialization
	void Start () {
        direction = new Vector3(Random.Range(-0.4f, 0.4f), 1f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * speed;
	}
}
