using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private PlayerInput playerInput;
    public float walkSpeed = 2f;   // ความเร็วเดิน
    public float runSpeed = 5f;    // ความเร็ววิ่ง
    private float currentSpeed;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // อ่านค่าจาก Virtual Joystick (On-Screen Stick → Left Stick)
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        // เอาเฉพาะแกน X (เกม 2D side-scroll)
        float x = input.x;

        // วัดระดับการเอียงของจอย
        float inputMagnitude = Mathf.Abs(x);

        // ถ้าเอียงน้อย = เดิน, ถ้ามาก = วิ่ง
        if (inputMagnitude > 0.1f && inputMagnitude <= 0.5f)
            currentSpeed = walkSpeed;
        else if (inputMagnitude > 0.5f)
            currentSpeed = runSpeed;
        else
            currentSpeed = 0f; // ไม่ขยับ

        // เคลื่อนที่
        Vector3 move = new Vector3(x, 0, 0);
        transform.position += move * currentSpeed * Time.deltaTime;

        // (ถ้ามีอนิเมชัน ก็สามารถใส่ Animator.SetFloat("Speed", Mathf.Abs(x)) ได้)
    }
}
