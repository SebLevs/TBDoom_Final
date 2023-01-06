using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BossHealthCanvas : MonoBehaviour
{
    // SECTION - Field ===================================================================
    // [SerializedField] private GameObject myHealthBarPrefab;
    [SerializeField] private TransformSO canvasRef;
    [SerializeField] private string entityName = "Unknown";

    private TextMeshProUGUI myText;
    private Image healthFiller;
    private Image armorFiller;
    private LivingEntityContext myLEC;

    // SECTION - Method - Unity Specific ===================================================================
    private void Awake()
    {
        // Get
        myText = canvasRef.Transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        healthFiller = canvasRef.Transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        armorFiller = canvasRef.Transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        // Set
        //if (!myText.gameObject.activeSelf)
           // myText.gameObject.SetActive(true);
        myText.text = entityName;
    }


    // SECTION - Method - Utility Specific ===================================================================
    public void SetAll(LivingEntityContext myLivingEntity)
    {
        // TODO (also on trigger exit of room enemy manager)
        //      - Should instantiate prefab of health bar inside a horizontal layout so that...
        //        multiple boss can easily stack
        if (!canvasRef.Transform.GetChild(0).gameObject.activeSelf)
            canvasRef.Transform.GetChild(0).gameObject.SetActive(true);

        if (!myText.gameObject.activeSelf)
            myText.gameObject.SetActive(true);

        healthFiller.fillAmount = myLivingEntity.CurrentHP / myLivingEntity.MaxHP;
        armorFiller.fillAmount = myLivingEntity.CurrentArmor / myLivingEntity.MaxArmor;
    }

    public void SetFill(LivingEntityContext myLivingEntity)
    {
        healthFiller.fillAmount = myLivingEntity.CurrentHP / myLivingEntity.MaxHP;
        armorFiller.fillAmount = myLivingEntity.CurrentArmor / myLivingEntity.MaxArmor;
    }

    public void DeactivateAll()
    {
        // TODO (also on trigger exit of room enemy manager)
        //      - Should destroy instantiated prefab of health bar inside a horizontal layout so that...
        //        multiple boss can easily destack (with a check for boss qty to not delete in the middle of a fight)
        canvasRef.Transform.GetChild(0).gameObject.SetActive(false);
        myText.enabled = false;
    }
}
