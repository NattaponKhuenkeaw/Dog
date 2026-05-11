using UnityEngine;

public class SceneTransitionData
{
    public bool ClearPlayerPosition;
    public bool ResetSession;
    public string LockedDoorId;

    public static SceneTransitionData ForDoor(string lockedDoorId)
    {
        return new SceneTransitionData
        {
            LockedDoorId = lockedDoorId,
        };
    }

    public static SceneTransitionData ForRestart()
    {
        return new SceneTransitionData
        {
            ClearPlayerPosition = true,
            ResetSession = true,
        };
    }
}
