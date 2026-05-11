using UnityEngine;

[CreateAssetMenu(menuName = "Dog/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float walkThreshold = 0.1f;
    public float runThreshold = 0.6f;
    public float interactThreshold = 0.8f;
    public float runEnergyCost = 3f;
    public float minRunEnergy = 25f;
    public float walkInterval = 0.45f;
    public float runInterval = 0.25f;
}
