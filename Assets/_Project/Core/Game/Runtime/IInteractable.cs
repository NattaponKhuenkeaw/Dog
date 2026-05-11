using UnityEngine;

public interface IInteractable
{
    string InteractPrompt { get; }
    InteractInputType RequiredInput { get; }
    bool CanInteract(GameObject player);
    void OnInteract(GameObject player);
    void OnInteractEnd(GameObject player);
}
