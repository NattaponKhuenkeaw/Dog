using UnityEngine;
using System.Collections.Generic;

public class Searchable : MonoBehaviour, IInteractable
{
    [Header("Item Pool (ScriptableObjects or Prefabs)")]
    public List<GameObject> possibleItems;
    public Transform spawnPoint;

    private GameObject spawnedItem = null;
    private bool hasSearched = false;

    public void OnInteract(PlayerController player)
    {
        if (!hasSearched)
        {
            if (possibleItems.Count > 0)
            {
                int index = Random.Range(0, possibleItems.Count);
                spawnedItem = Instantiate(possibleItems[index], spawnPoint.position, Quaternion.identity);
            }

            hasSearched = true;
            Debug.Log("Searched! Item spawned");
        }
        else if (spawnedItem != null)
        {
            var pickup = spawnedItem.GetComponent<IInteractable>();
            if (pickup != null)
            {
                pickup.OnInteract(player);
                spawnedItem = null;
            }
        }
        else
        {
            Debug.Log("Nothing left to collect");
        }
    }
}