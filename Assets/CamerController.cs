using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    void Awake()
    {
        //this.gameObject.transform.LookAt(GameObject.Find("rpgpp_lt_building_05").transform);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
