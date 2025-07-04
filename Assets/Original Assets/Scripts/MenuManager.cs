using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
  public static MenuManager instance;
  [SerializeField] private int valueLevel;
  [SerializeField] private int incomeLevel;
  public Text valueLevelText;
  public Text incomeLevelText;
  [SerializeField] private List<int> immunePricePerLevel;
  [SerializeField] private List<int> valuePricePerLevel;
  [SerializeField] private List<int> incomePricePerLevel;
  public Text valuePriceText;
  public Text incomePriceText;
  public List<float> valuePerLevel;
  public List<int> incomePerLevel;
  public int multiplierValue;
  public float incomePercentage;
  public float moneyStackMod;
  [SerializeField] private GameObject setting;
  private Animator settingAnim;

  private void Awake()
  {
    instance = this;
  }

  private void Start()
  {
    valueLevel = PlayerPrefs.GetInt("Value_Level", 1);
    incomeLevel = PlayerPrefs.GetInt("Income_Level", 1);

    moneyStackMod = valuePerLevel[valueLevel - 1];
    incomePercentage = incomePerLevel[incomeLevel - 1];

    settingAnim = setting.GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    valueLevelText.text = string.Format("Level " + "{0:0}", valueLevel);
    incomeLevelText.text = string.Format("Level " + "{0:0}", incomeLevel);

    valuePriceText.text = string.Format("{0:0}", valuePricePerLevel[valueLevel]);
    incomePriceText.text = string.Format("{0:0}", incomePricePerLevel[incomeLevel]);
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void BuyValueUpgrade()
  {
    if (GameManager.totalGemAmount >= valuePricePerLevel[valueLevel])
    {
      GameManager.totalGemAmount -= valuePricePerLevel[valueLevel];
      moneyStackMod = valuePerLevel[valueLevel];
      valueLevel++;
      PlayerPrefs.SetInt("Value_Level", valueLevel);
    }
  }

  public void BuyIncomeUpgrade()
  {
    if (GameManager.totalGemAmount >= incomePricePerLevel[incomeLevel])
    {
      GameManager.totalGemAmount -= incomePricePerLevel[incomeLevel];
      incomePercentage = incomePerLevel[incomeLevel];
      incomeLevel++;
      PlayerPrefs.SetInt("Income_Level", incomeLevel);
    }
  }

  public void ContinueGame()
  {
    //Xem ads complete
    GameManager.Instance.levelNo++;
    GameManager.totalGemAmount += GameManager.Instance.currentGemCollected * multiplierValue;

    GameManager.Instance.CurrentLevelIndex++;
    PlayerPrefs.SetFloat("Total_Gem", GameManager.totalGemAmount);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void ReviveButton()
  {
    GameManager.totalGemAmount += GameManager.Instance.currentGemCollected;
    PlayerPrefs.SetFloat("Total_Gem", GameManager.totalGemAmount);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void SkipButton()
  {
    GameManager.Instance.levelNo++;

    GameManager.Instance.CurrentLevelIndex++;

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void ContinueWithoutMultiplierButton()
  {
    GameManager.Instance.levelNo++;
    GameManager.totalGemAmount += GameManager.Instance.currentGemCollected * multiplierValue;

    GameManager.Instance.CurrentLevelIndex++;
    PlayerPrefs.SetFloat("Total_Gem", GameManager.totalGemAmount);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void settingButton()
  {
    if (settingAnim.GetBool("isOpen") == false)
    {
      settingAnim.SetBool("isOpen", true);
    }
    else
    {
      settingAnim.SetBool("isOpen", false);
    }
  }
}
