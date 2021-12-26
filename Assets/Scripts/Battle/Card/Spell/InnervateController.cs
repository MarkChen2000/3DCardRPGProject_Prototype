using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnervateController : MonoBehaviour
{
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        this.lifeTime = 1f;
        this.gameObject.transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
        Invoke("KillSelf", this.lifeTime);
    }

    void Update()
    {
        this.gameObject.transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
    }

    private void KillSelf()
    {
        Destroy(this.gameObject);
    }
}
