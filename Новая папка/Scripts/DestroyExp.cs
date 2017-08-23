using UnityEngine;
using System.Collections;

public class DestroyExp : MonoBehaviour {

    public float timeToDestroy;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, timeToDestroy);
	}
	
}
