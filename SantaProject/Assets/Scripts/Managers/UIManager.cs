using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI PowerUpPopup;
    [SerializeField] private TextMeshProUGUI PresentText;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;

        updatePresentText(0);
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(this);
        //}
    }

    public void updatePresentText(int presents)
    {
        PresentText.text = presents + " / 35";
    }

    public void updatePlayerHealthText(float currentHealth)
    {
        playerHealthText.text = currentHealth.ToString();
    }

    public void showPowerup(string nameAndHowToUse)
    {
        PowerUpPopup.gameObject.SetActive(true);
        PowerUpPopup.text = nameAndHowToUse;
        StartCoroutine(waitToDisablePowerUpPopup());
    }

    private IEnumerator waitToDisablePowerUpPopup()
    {
        yield return new WaitForSeconds(5f);
        PowerUpPopup.gameObject.SetActive(false);
    }
}
