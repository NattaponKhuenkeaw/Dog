using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    private float currentSpeed;

    [Header("Energy Settings")]
    public bool useEnergySystem = true;
    public float runEnergyCost = 3f; // ใช้พลังต่อวินาทีตอนวิ่ง

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float x = input.x;
        float inputMagnitude = Mathf.Abs(x);

        float currentEnergy = GameManager.instance.energy;

        // 🔹 กำหนดความเร็ว
        if (inputMagnitude > 0.1f && inputMagnitude <= 0.6f)
        {
            currentSpeed = walkSpeed;
            GameManager.instance.isRunning = false;
        }
        else if (inputMagnitude > 0.6f)
        {
            currentSpeed = currentEnergy > 0 ? runSpeed : walkSpeed;
            GameManager.instance.isRunning = currentSpeed == runSpeed;
        }
        else
        {
            currentSpeed = 0f;
            GameManager.instance.isRunning = false;
        }

        // เคลื่อนที่
        Vector3 move = new Vector3(x, 0, 0);
        transform.position += move * currentSpeed * Time.deltaTime;

        // ส่งสถานะการเคลื่อนไหว
        GameManager.instance.isMoving = currentSpeed > 0;

        // ใช้พลังงานตอนวิ่ง
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
        {
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);
        }
    }
}
