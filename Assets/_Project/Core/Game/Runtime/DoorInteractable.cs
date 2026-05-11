using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public string sceneName;
    public static bool roomOpened;
    public Door door;
    [SerializeField] private SceneReference sceneReference;

    public string InteractPrompt => "Open";
    public InteractInputType RequiredInput => InteractInputType.JoystickUp;

    public bool CanInteract(GameObject player)
    {
        return sceneReference != null || !string.IsNullOrWhiteSpace(sceneName);
    }

    public void OnInteract(GameObject player)
    {
        OpenDoor(player);
    }

    public void OnInteractEnd(GameObject player)
    {
    }

    public virtual void OpenDoor()
    {
        OpenDoor(null);
    }

    public virtual void OpenDoor(GameObject player)
    {
        if (player != null && Services.Session != null)
        {
            Services.Session.LastPlayerPosition = player.transform.position;
        }

        string lockedDoorId = door != null ? door.doorID : null;
        SceneTransitionData transitionData = SceneTransitionData.ForDoor(lockedDoorId);

        if (sceneReference != null)
        {
            Services.SceneLoader?.LoadScene(sceneReference, transitionData);
        }
        else
        {
            Services.SceneLoader?.LoadSceneByName(sceneName, transitionData);
        }
    }

    public void changScene()
    {
        if (sceneReference != null)
        {
            Services.Session?.RestartSession(sceneReference);
        }
        else
        {
            Services.Session?.RestartSession(sceneName);
        }
    }

    public void Quit()
    {
        Services.Session?.QuitGame();
    }
}
