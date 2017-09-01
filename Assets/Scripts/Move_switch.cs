using UnityEngine;
using System.Collections;

public class Move_switch : MonoBehaviour {

    public GameObject _switch;
    public bool _curPosition;
    Vector2 v1, v2;
	// Use this for initialization
	void Start () {
        v1 = _switch.transform.position; 
       
    }
    public void change()
    {
        _curPosition = true;
    }

    public void Move()
    {
     //   _switch.transform.position = Vector2.Distance(v1, _switch.transform.position.x + 60f, Time.deltaTime * 1);
    }
    void Update()
    {
        if (_curPosition)
        {
            Move();
        }
    }
}
