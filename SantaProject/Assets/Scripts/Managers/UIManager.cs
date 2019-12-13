using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TextMeshProUGUI PowerUpPopup;
    [SerializeField] private TextMeshProUGUI PresentText;
    [SerializeField] private Image dash;
    [SerializeField] private Image doubleJump;
    [SerializeField] private Image Snowball;

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

    public void updatePlayerHealthText(float currentHealth, float maxHealth)
    {
        //playerHealthText.text = currentHealth.ToString();
        playerHealthSlider.value = currentHealth / maxHealth;
    }

    public void hasDash()
    {
        dash.gameObject.SetActive(true);
    }
    public void setDashActive()
    {
        dash.color = new Vector4(1, 1, 1, 1);
    }
    public void setDashInactive()
    {
        dash.color = new Vector4(.75f, .75f, .75f, .5f);
    }


    public void hasDoubleJump()
    {
        doubleJump.gameObject.SetActive(true);
    }
    public void setDoubleJumpActive()
    {
        doubleJump.color = new Vector4(1, 1, 1, 1);
    }
    public void setDoubleJumpInactive()
    {
        doubleJump.color = new Vector4(.75f, .75f, .75f, .5f);
    }


    public void hasSnowball()
    {
        Snowball.gameObject.SetActive(true);
    }
    public void setSnowballActive()
    {
        Snowball.color = new Vector4(1, 1, 1, 1);
    }
    public void setSnowballInactive()
    {
        Snowball.color = new Vector4(.75f, .75f, .75f, .5f);
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
