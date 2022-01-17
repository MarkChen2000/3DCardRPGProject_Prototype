using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning_Controller : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject turtlePrefab;
    public GameObject stonePrefab;
    private GameObject player;
    private bool spawn_Is_Activate;
    public int zone_difficulty;

    private GameState currentGameState;
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.spawn_Is_Activate = false;
        this.currentGameState = GameObject.Find("GameManager").GetComponent<GameStateController>().CurrentGameState;
    }

    // Update is called once per frame
    void Update()
    {
        if (Detect() && !this.spawn_Is_Activate)
        {
            this.spawn_Is_Activate = true;
            InvokeRepeating("Spawn", 1f, 20f);
        }
    }

    private bool Detect()
    {
        if (Vector3.Distance(this.transform.position, this.player.transform.position) <= 20f)
        {
            return true;
        }
        else
        {
            CancelInvoke("Spawn");
            this.spawn_Is_Activate = false;
            return false;
        }
    }

    private void Spawn()
    {
        System.Random rnd = new System.Random();
        if (this.zone_difficulty == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject monster = Instantiate(slimePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
            }
        }
        else if (this.zone_difficulty == 2)
        {
            float temp = rnd.Next(1, 100);
            if (temp <= 25)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject monster = Instantiate(slimePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
                for (int i = 0; i < 2; i++)
                {
                    GameObject monster = Instantiate(turtlePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject monster = Instantiate(slimePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
                for (int i = 0; i < 1; i++)
                {
                    GameObject monster = Instantiate(turtlePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
            }
        }
        else if (this.zone_difficulty == 3)
        {
            float temp = rnd.Next(1, 100);
            if (temp <= 25)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject monster = Instantiate(turtlePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject monster = Instantiate(slimePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
                for (int i = 0; i < 3; i++)
                {
                    GameObject monster = Instantiate(turtlePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
                }
            }
        }
        else if (this.zone_difficulty == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject monster = Instantiate(stonePrefab, new Vector3(UnityEngine.Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), this.transform.position.y + 1, UnityEngine.Random.Range(this.transform.position.z - 10, this.transform.position.z + 10)), this.transform.rotation);
            }
        }
    }
}
