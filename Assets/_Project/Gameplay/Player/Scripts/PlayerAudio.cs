using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private Player owner;
    private PlayerMovement movement;
    private float footstepTimer;

    public void Initialize(Player playerOwner, PlayerMovement playerMovement)
    {
        owner = playerOwner;
        movement = playerMovement;
    }

    private void Update()
    {
        if (owner == null || movement == null || owner.footstepSource == null)
        {
            return;
        }

        bool movingHorizontally = Mathf.Abs(movement.CurrentInput.x) > owner.WalkThreshold;
        if (!movingHorizontally)
        {
            if (owner.footstepSource.isPlaying)
            {
                owner.footstepSource.Stop();
            }

            footstepTimer = 0f;
            return;
        }

        bool running = movement.IsRunning;
        float interval = running ? owner.RunFootstepInterval : owner.WalkFootstepInterval;
        AudioClip targetClip = running ? owner.runClip : owner.walkClip;

        if (owner.footstepSource.clip != targetClip)
        {
            owner.footstepSource.clip = targetClip;
            owner.footstepSource.Stop();
            footstepTimer = 0f;
        }

        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0f)
        {
            return;
        }

        owner.footstepSource.pitch = Random.Range(0.95f, 1.05f);
        owner.footstepSource.Play();
        footstepTimer = interval;
    }
}
