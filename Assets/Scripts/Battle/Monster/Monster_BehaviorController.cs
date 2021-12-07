using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_BehaviorController : MonoBehaviour
{
    private GameObject player;
    private Vector3 initPostition;
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.initPostition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        this.speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Detect())
        {
            this.gameObject.transform.GetChild(0).LookAt(this.player.transform);
            Debug.Log("Player detected");
        }
    }

    private bool Detect()
    {
        if (Vector3.Distance(this.initPostition, this.transform.position) < 20f)
        {
            if (Vector3.Distance(this.transform.position, this.player.transform.position) <= 10f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
}
