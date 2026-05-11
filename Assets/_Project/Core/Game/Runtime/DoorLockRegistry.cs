using System.Collections.Generic;
using UnityEngine;

public class DoorLockRegistry : MonoBehaviour
{
    private readonly HashSet<string> lockedDoors = new HashSet<string>();

    private void Awake()
    {
        Services.Doors = this;
    }

    public void LockDoor(string doorId)
    {
        if (string.IsNullOrWhiteSpace(doorId))
        {
            return;
        }

        lockedDoors.Add(doorId);
    }

    public void UnlockDoor(string doorId)
    {
        if (string.IsNullOrWhiteSpace(doorId))
        {
            return;
        }

        lockedDoors.Remove(doorId);
    }

    public bool IsDoorLocked(string doorId)
    {
        return !string.IsNullOrWhiteSpace(doorId) && lockedDoors.Contains(doorId);
    }

    public void Clear()
    {
        lockedDoors.Clear();
    }
}
