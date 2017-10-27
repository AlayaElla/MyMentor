using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCenter : MonoBehaviour {

    private Vector3 Pos;
    private float speed = 2f;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        float _sp = speed * 0.05f;
        transform.position = new Vector2(Mathf.Lerp(transform.position.x, Pos.x, _sp), Mathf.Lerp(transform.position.y, Pos.y, _sp));

        if (Vector2.Distance(transform.position, Pos) <= 0.1)
        {
            Destroy(this);
        }
	}

    public void SetPos(Vector2 setpos)
    {
        Pos = setpos;
    }

    public void SetSpeed(float setspeed)
    {
        speed = setspeed;
    }
}
