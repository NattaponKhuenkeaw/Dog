using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 CurrentInput { get; private set; }
    public float CurrentSpeed { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }

    private Player owner;
    private PlayerHiding hiding;

    public void Initialize(Player playerOwner, PlayerHiding playerHiding)
    {
        owner = playerOwner;
        hiding = playerHiding;
    }

    private void Update()
    {
        if (owner == null || owner.PlayerInput == null)
        {
            return;
        }

        CurrentInput = owner.PlayerInput.actions["Move"].ReadValue<Vector2>();
        float x = CurrentInput.x;
        float inputMagnitude = Mathf.Abs(x);
        float currentEnergy = Services.Energy != null ? Services.Energy.CurrentEnergy : 0f;

        if (inputMagnitude > owner.WalkThreshold && inputMagnitude <= owner.RunThreshold)
        {
            CurrentSpeed = owner.WalkSpeed;
            IsRunning = false;
        }
        else if (inputMagnitude > owner.RunThreshold)
        {
            bool canRun = currentEnergy >= owner.MinRunEnergy;
            CurrentSpeed = canRun ? owner.RunSpeed : owner.WalkSpeed;
            IsRunning = canRun;
        }
        else
        {
            CurrentSpeed = 0f;
            IsRunning = false;
        }

        if (owner.Animator != null)
        {
            owner.Animator.SetFloat("Speed", CurrentSpeed);
            owner.Animator.SetBool("IsMoving", inputMagnitude > 0.01f);
        }

        if (owner.SpriteRenderer != null)
        {
            if (x > 0f)
            {
                owner.SpriteRenderer.flipX = false;
            }
            else if (x < 0f)
            {
                owner.SpriteRenderer.flipX = true;
            }
        }

        if ((hiding != null && hiding.IsHidden) || owner.stopX)
        {
            x = 0f;
        }

        Vector3 move = new Vector3(x, 0f, 0f);
        Vector3 newPosition = transform.position + move * CurrentSpeed * Time.deltaTime;
        if (owner.useBoundaries)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, owner.minX, owner.maxX);
        }

        transform.position = newPosition;

        IsMoving = CurrentSpeed > 0.01f;
        owner.isMoving = IsMoving;
        owner.isRunning = IsRunning;

        if (Services.Energy != null)
        {
            Services.Energy.SetMovementState(IsMoving, IsRunning);
            if (owner.useEnergySystem && IsRunning && currentEnergy > 0f)
            {
                Services.Energy.UseEnergy(owner.RunEnergyCost * Time.deltaTime);
            }
        }
    }
}
