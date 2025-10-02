using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        float targetX = player.position.x + offset.x;

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void ResetPositionImmediate()
    {
        if (player == null) return;
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
    }

}