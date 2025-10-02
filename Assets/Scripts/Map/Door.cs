using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public bool requiresKey = false;
    public string keyID;

    [Header("Warp Settings (Next Scene)")]
    public bool isSceneDoor = false;
    public string sceneName;

    [Header("Warp Settings (Same Scene)")]
    public bool warpInScene = false;
    public Transform warpTarget;

    [Header("Door Graphics")]
    public SpriteRenderer doorRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;
    public Collider2D doorCollider;

    private bool isOpen = false;

    public void OnInteract(PlayerController player)
    {
        if (isOpen) return;

        if (requiresKey)
        {
            if (!player.HasKey(keyID))
            {
                Debug.Log("Door is locked!");
                return;
            }
            player.UseKey(keyID);
        }

        ToggleDoor(player);
    }

    private void ToggleDoor(PlayerController player)
    {
        if (isSceneDoor && !string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (warpInScene && warpTarget != null)
        {
            player.transform.position = warpTarget.position;

            var cameraFollow = Camera.main.GetComponent<CameraFollowPlayer>();
            if (cameraFollow != null)
            {
                cameraFollow.ResetPositionImmediate();
            }
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