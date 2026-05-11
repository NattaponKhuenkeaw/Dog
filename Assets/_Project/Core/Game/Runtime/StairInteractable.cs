using UnityEngine;

public class StairInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform targetPosition;

    public string InteractPrompt => "Climb";
    public InteractInputType RequiredInput => InteractInputType.JoystickUp;

    public bool CanInteract(GameObject player)
    {
        return player != null && targetPosition != null;
    }

    public void OnInteract(GameObject player)
    {
        if (player != null && targetPosition != null)
        {
            player.transform.position = targetPosition.position;
        }
    }

    public void OnInteractEnd(GameObject player)
    {
    }
}
