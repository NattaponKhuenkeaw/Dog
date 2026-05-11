using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player owner;
    private PlayerMovement movement;
    private PlayerHiding hiding;
    private IInteractable currentTarget;
    private DoorClick legacyDoorClick;
    private float previousVerticalInput;

    public void Initialize(Player playerOwner, PlayerMovement playerMovement, PlayerHiding playerHiding)
    {
        owner = playerOwner;
        movement = playerMovement;
        hiding = playerHiding;
    }

    private void Update()
    {
        if (owner == null || movement == null)
        {
            return;
        }

        float vertical = movement.CurrentInput.y;
        bool upPressed = vertical > owner.InteractThreshold && previousVerticalInput <= owner.InteractThreshold;
        bool downPressed = vertical < -owner.InteractThreshold && previousVerticalInput >= -owner.InteractThreshold;

        if (currentTarget != null)
        {
            if (upPressed && currentTarget.RequiredInput == InteractInputType.JoystickUp && currentTarget.CanInteract(gameObject))
            {
                currentTarget.OnInteract(gameObject);
            }
            else if (downPressed && currentTarget.RequiredInput == InteractInputType.JoystickDown && currentTarget.CanInteract(gameObject))
            {
                currentTarget.OnInteract(gameObject);
            }
        }

        if (upPressed)
        {
            if (legacyDoorClick != null)
            {
                legacyDoorClick.OpenDoor(gameObject);
            }
            else if (owner.playerIsNearHide && hiding != null && !hiding.IsHidden)
            {
                hiding.EnterHide();
            }
            else if (owner.playerIsNearStairs && owner.targetPosition != null)
            {
                transform.position = owner.targetPosition.position;
                owner.playerIsNearStairs = false;
            }
        }
        else if (downPressed && hiding != null && hiding.IsHidden)
        {
            hiding.ExitHide();
            currentTarget?.OnInteractEnd(gameObject);
        }

        previousVerticalInput = vertical;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TryGetInteractable(other, out IInteractable interactable))
        {
            currentTarget = interactable;
        }

        if (other.CompareTag("Door"))
        {
            legacyDoorClick = other.GetComponent<DoorClick>();
            owner.playerIsNearDoor = legacyDoorClick != null;
        }
        else if (other.CompareTag("HideSpot"))
        {
            owner.playerIsNearHide = true;
        }
        else if (other.CompareTag("Stalker"))
        {
            owner.TriggerLegacyJumpscare();
            Services.Health?.TakeDamage(100);
        }
        else if (other.CompareTag("Win"))
        {
            owner.ShowWinPanel();
        }
        else if (other.CompareTag("stairs"))
        {
            owner.playerIsNearStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (TryGetInteractable(other, out IInteractable interactable) && interactable == currentTarget)
        {
            currentTarget = null;
        }

        if (other.CompareTag("Door"))
        {
            legacyDoorClick = null;
            owner.playerIsNearDoor = false;
        }
        else if (other.CompareTag("HideSpot"))
        {
            owner.playerIsNearHide = false;
        }
        else if (other.CompareTag("stairs"))
        {
            owner.playerIsNearStairs = false;
        }
    }

    private static bool TryGetInteractable(Collider2D other, out IInteractable interactable)
    {
        if (TryGetInteractableFromBehaviours(other.GetComponents<MonoBehaviour>(), out interactable))
        {
            return true;
        }

        return TryGetInteractableFromBehaviours(other.GetComponentsInParent<MonoBehaviour>(), out interactable);
    }

    private static bool TryGetInteractableFromBehaviours(MonoBehaviour[] behaviours, out IInteractable interactable)
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i] is IInteractable typedInteractable)
            {
                interactable = typedInteractable;
                return true;
            }
        }

        interactable = null;
        return false;
    }
}
