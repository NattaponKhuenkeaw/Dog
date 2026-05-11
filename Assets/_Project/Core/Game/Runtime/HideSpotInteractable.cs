using UnityEngine;

public class HideSpotInteractable : MonoBehaviour, IInteractable
{
    public string InteractPrompt => "Hide";
    public InteractInputType RequiredInput => InteractInputType.JoystickUp;

    public bool CanInteract(GameObject player)
    {
        return player != null && player.GetComponent<PlayerHiding>() != null;
    }

    public void OnInteract(GameObject player)
    {
        PlayerHiding hiding = player != null ? player.GetComponent<PlayerHiding>() : null;
        if (hiding != null && !hiding.IsHidden)
        {
            hiding.EnterHide();
        }
    }

    public void OnInteractEnd(GameObject player)
    {
    }
}
