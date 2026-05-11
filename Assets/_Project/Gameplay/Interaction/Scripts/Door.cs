using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorID;
    public Collider2D col;
    public SpriteRenderer sr;
    public Sprite lockedSprite;

    private void Start()
    {
        if (Services.Doors != null && Services.Doors.IsDoorLocked(doorID))
        {
            Lock();
        }
    }

    public void Lock()
    {
        if (col != null)
        {
            col.enabled = false;
        }

        if (sr != null && lockedSprite != null)
        {
            sr.sprite = lockedSprite;
        }

        Debug.Log($"Door {doorID} was locked on load.");
    }
}
