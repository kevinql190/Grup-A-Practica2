using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILerper : MonoBehaviour
{
    [Header("Page Lerp")]
    [SerializeField] private GameObject firstSelectedLevels;
    [SerializeField] private GameObject firstSelectedMenu;
    public GameObject firstSelectedReceptari;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelLevels;
    [SerializeField] private GameObject panelReceptari;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private float menuFadeTime;
    [SerializeField] private float receptariFadeTime;
    [SerializeField] private float levelsFadeTime;
    [SerializeField] private float settingsFadeTime;
    [SerializeField] private float lerpReceptariTime;
    [SerializeField] private float lerpSettingsTime;
    [SerializeField] private float lerpLevelsTime;

    #region Page Lerp
    public void LerpReceptari(bool toReceptari)
    {
        if (toReceptari)
        {
            StartCoroutine(LerpPanel(panelMainMenu, panelReceptari, menuFadeTime, receptariFadeTime, lerpReceptariTime));
        }
        else
        {
            StartCoroutine(LerpPanel(panelReceptari, panelMainMenu, receptariFadeTime, menuFadeTime, lerpReceptariTime));
        }
    }
    public void LerpLevels(bool toLevel)
    {
        if (toLevel)
        {
            StartCoroutine(LerpPanel(panelMainMenu, panelLevels, menuFadeTime, levelsFadeTime, lerpLevelsTime));
        }
        else
        {
            StartCoroutine(LerpPanel(panelLevels, panelMainMenu, levelsFadeTime, menuFadeTime, lerpLevelsTime));
        }
    }
    public void LerpSettings(bool toSettings)
    {
        if (toSettings)
        {
            StartCoroutine(LerpPanel(panelMainMenu, panelSettings, menuFadeTime, settingsFadeTime, lerpSettingsTime));
        }
        else
        {
            StartCoroutine(LerpPanel(panelSettings, panelMainMenu, settingsFadeTime, menuFadeTime, lerpSettingsTime));
        }
    }
    public IEnumerator LerpPanel(GameObject fadeout, GameObject fadein, float fadeoutTime, float fadeinTime, float inbetweenTime)
    {
        float timer = 0f;
        while (timer < fadeoutTime) //Fade out
        {
            timer += Time.deltaTime;
            float progress = timer / fadeoutTime;
            fadeout.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, progress);
            yield return null;
        }
        fadeout.SetActive(false);
        yield return new WaitForSeconds(inbetweenTime);
        fadein.SetActive(true);
        if (fadein == panelMainMenu) EventSystem.current.SetSelectedGameObject(firstSelectedMenu);
        if (fadein == panelReceptari) EventSystem.current.SetSelectedGameObject(firstSelectedReceptari);
        if (fadein == panelLevels) EventSystem.current.SetSelectedGameObject(firstSelectedLevels);
        timer = 0f;
        while (timer < fadeinTime) //Fade out
        {
            timer += Time.deltaTime;
            float progress = timer / fadeinTime;
            fadein.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, progress);
            yield return null;
        }
    }
    #endregion
}
