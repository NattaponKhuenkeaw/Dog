using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    private PlayerInput playerInput;
    public float speed = 5f;

   

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
    }

    void Update()
    {
        
        Vector2 input = playerInput.actions["Look"].ReadValue<Vector2>();

        // แปลงค่าเป็น Vector3 สำหรับแกน X,Y
        Vector3 move = new Vector3(input.x, input.y, 0);

        // เคลื่อนที่ไฟฉาย
        transform.position += move * speed * Time.deltaTime;

        
    }
}
