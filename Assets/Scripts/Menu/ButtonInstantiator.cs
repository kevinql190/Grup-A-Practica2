using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInstantiator : MonoBehaviour
{
    [Header("Receptari Instantiate")]
    [SerializeField] private GameObject buttonsReceptariParent;
    [SerializeField] private GameObject buttonsReceptariPrefab;
    [Header("Levels Instantiate")]
    [SerializeField] private GameObject buttonsLevelsParent;
    [SerializeField] private GameObject buttonsLevelsPrefab;
    #region Receptari Instantiate
    public void GenerateReceptariPanel()
    {
        foreach (ReceptariInfo info in GameManager.Instance.receptariInfo)
        {
            GameObject button = Instantiate(buttonsReceptariPrefab, buttonsReceptariParent.transform);
            button.GetComponent<Button>().interactable = info.found;
            if (info.FoodType.receptariSprite != null)
            {
                GameObject image = new();
                Image component = image.AddComponent<Image>();
                component.sprite = info.FoodType.receptariSprite;
                component.SetNativeSize();
                Instantiate(image, button.transform);
            }
        }
    }
    #endregion
    #region Levels Instantiate

    public void GenerateLevelPanel()
    {
        foreach (LevelInfo info in GameManager.Instance.levels)
        {
            GameObject button = Instantiate(buttonsLevelsPrefab, buttonsLevelsParent.transform);
            button.GetComponent<TMP_Text>().text = info.levelName;
            button.GetComponent<Button>().interactable = info.unlocked;
            button.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.GetComponent<ASyncLoader>().LoadLevelBtn(info.levelScene));
        }
    }
    #endregion
}
