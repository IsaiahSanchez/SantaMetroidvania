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
    [SerializeField] private Animator popupAnim;
    [SerializeField] private Animator PresentPickupAnim;
    [SerializeField] private Coroutine currentWait;

    private string nextInfo;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

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

    public void shakePresentPanel()
    {
        PresentPickupAnim.ResetTrigger("Shake");
        PresentPickupAnim.SetTrigger("Shake");
    }

    public void showPowerup(string nameAndHowToUse)
    {
        if (currentWait != null)
        {
            nextInfo = nameAndHowToUse;
        }
        else
        {
            nextInfo = null;
            popupAnim.ResetTrigger("In");
            popupAnim.SetTrigger("In");
            PowerUpPopup.text = nameAndHowToUse;
            currentWait = StartCoroutine(waitToDisablePowerUpPopup());
        }
    }

    private IEnumerator waitToDisablePowerUpPopup()
    {
        yield return new WaitForSeconds(4f);
        popupAnim.ResetTrigger("Out");
        popupAnim.SetTrigger("Out");
        if (nextInfo != null)
        {
            StartCoroutine(waitTilFinished());
        }
        currentWait = null;
    }

    private IEnumerator waitTilFinished()
    {
        yield return new WaitForSeconds(1f);
        showPowerup(nextInfo);
    }
}
