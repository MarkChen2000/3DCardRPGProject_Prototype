using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - GameObject.Find("MainCamera").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - GameObject.Find("MainCamera").transform.position);
    }
}
