using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private Slider serializedEnergySlider;

    private Slider energySlider;
    private bool subscribed;
    private bool initialized;

    public void Initialize(Slider slider)
    {
        energySlider = slider;
        initialized = true;
        Subscribe();
        Refresh(Services.Energy != null ? Services.Energy.CurrentEnergy : 0f, Services.Energy != null ? Services.Energy.MaxEnergy : 1f);
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize(serializedEnergySlider);
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (!subscribed || Services.Energy == null)
        {
            return;
        }

        Services.Energy.OnEnergyChanged -= Refresh;
        subscribed = false;
    }

    private void Subscribe()
    {
        if (subscribed || Services.Energy == null)
        {
            return;
        }

        Services.Energy.OnEnergyChanged += Refresh;
        subscribed = true;
    }

    private void Refresh(float current, float max)
    {
        if (energySlider == null)
        {
            return;
        }

        energySlider.maxValue = max;
        energySlider.value = current;
    }
}
