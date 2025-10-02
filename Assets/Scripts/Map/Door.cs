using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public string keyID;
    public bool isSceneDoor = false;
    public string sceneName;

    [Header("Door Graphics")]
    public SpriteRenderer doorRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;
    public Collider2D doorCollider;

    private bool isOpen = false;

    public void OnInteract(PlayerController player)
    {
        if (isOpen) return;

        var inventory = player.GetComponent<PlayerInventory>();
        if (!string.IsNullOrEmpty(keyID) && inventory != null)
        {
            if (!inventory.HasKey(keyID))
            {
                Debug.Log("Door is locked!");
                return;
            }
            inventory.UseKey(keyID);
        }

        ToggleDoor(player);
    }

    private void ToggleDoor(PlayerController player)
    {
        if (isSceneDoor && !string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                if (openSprite != null) doorRenderer.sprite = openSprite;
                if (doorCollider != null) doorCollider.enabled = false;
            }
            else
            {
                if (closedSprite != null) doorRenderer.sprite = closedSprite;
                if (doorCollider != null) doorCollider.enabled = true;
            }
        }
    }
}