using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_ButtonView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text price;
    [SerializeField]
    private TMP_Text detail;

    public void OnProductFetched(Product product)
    {
        if (title != null)
        {
            title.text = product.metadata.localizedTitle;
        }

        if (price != null)
        {
            price.text = $"{product.metadata.localizedPriceString} USD";
        }

        if (detail != null)
        {
            detail.text = product.metadata.localizedDescription;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
