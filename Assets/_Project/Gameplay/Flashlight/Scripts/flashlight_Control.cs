using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{

    public AudioSource audioSource;    
    public AudioClip Opendoor;
    private PlayerInput playerInput;
    public float speed = 5f;

    void Start()
    {
        audioSource.PlayOneShot(Opendoor);
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // อ่านค่าจาก Move (WASD หรือ Joystick)
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, input.y, 0);

        transform.position += move * speed * Time.deltaTime;
    }
}
