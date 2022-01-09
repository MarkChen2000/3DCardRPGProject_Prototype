using UnityEngine;

public class PrefabSelfDestroyController : MonoBehaviour
{
    [HideInInspector]public float lifeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
