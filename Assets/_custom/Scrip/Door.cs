using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorID;     // ตั้งค่าเป็น “DoorA1” หรือ “Room2_Door3”
    public Collider2D col;
    public SpriteRenderer sr;
    public Sprite lockedSprite;

    void Start()
    {
        if (GameManager.instance.IsDoorLocked(doorID))
        {
            Lock();
        }
    }

    public void Lock()
    {
        if (col != null) col.enabled = false;
        if (sr != null && lockedSprite != null)
            sr.sprite = lockedSprite;

        Debug.Log($"Door {doorID} ถูกล็อกเมื่อโหลดฉาก");
    }
}
