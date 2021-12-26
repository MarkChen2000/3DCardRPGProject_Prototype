using UnityEngine;

public class PlayerEquipmentDisplayController : MonoBehaviour
{
    [SerializeField] Transform WeaponHoldingSpot;

    private void Awake()
    {
    }

    public void DisplayEquipmentPrefab(EquipmentType type,GameObject prefab)
    {
        if (WeaponHoldingSpot.childCount==1) // if there is already have a weapon.
        {
            //Debug.Log("Destroy" + WeaponHoldingSpot.GetChild(0).gameObject);
            Destroy(WeaponHoldingSpot.GetChild(0).gameObject);
        }
        if (prefab != null)
        {
            Instantiate(prefab, WeaponHoldingSpot);
        }
    }

}
