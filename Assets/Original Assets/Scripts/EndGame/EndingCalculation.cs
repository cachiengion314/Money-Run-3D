using UnityEngine;

public class EndingCalculation : MonoBehaviour
{
  public float stackCollected;

  void Start()
  {
    stackCollected = 0;
  }

  void Update()
  {
    CalculateGemByStar();
    CalculateGemByCompleteMap();
    CalculateGemWithIncome();
  }

  void CalculateGemByStar()
  {
    float starPercentage = stackCollected / GameManager.Instance.totalNumberOfStack;

    if (starPercentage <= 0.3f)
    {
      GameManager.Instance.gemByStar = 100;
    }
    else if (starPercentage > 0.3f && starPercentage < 0.6f)
    {
      GameManager.Instance.secondStar.gameObject.SetActive(true);
      GameManager.Instance.gemByStar = 200;
    }
    else if (starPercentage >= 0.6f)
    {
      GameManager.Instance.secondStar.gameObject.SetActive(true);
      GameManager.Instance.thirdStar.gameObject.SetActive(true);
      GameManager.Instance.gemByStar = 300;
    }
  }

  void CalculateGemByCompleteMap()
  {
    float stackMoneyAmount = 0;
    float stackMultiplier;
    if (stackMoneyAmount < 1500)
    {
      stackMultiplier = 1;
    }
    else if (stackMoneyAmount >= 1500 && stackMoneyAmount < 2500)
    {
      stackMultiplier = 2;
    }
    else if (stackMoneyAmount >= 2500 && stackMoneyAmount < 3500)
    {
      stackMultiplier = 3;
    }
    else if (stackMoneyAmount >= 3500 && stackMoneyAmount < 4500)
    {
      stackMultiplier = 4;
    }
    else if (stackMoneyAmount >= 4500 && stackMoneyAmount < 5500)
    {
      stackMultiplier = 5;
    }
    else if (stackMoneyAmount >= 5500 && stackMoneyAmount < 6500)
    {
      stackMultiplier = 6;
    }
    else if (stackMoneyAmount >= 6500 && stackMoneyAmount < 7500)
    {
      stackMultiplier = 7;
    }
    else if (stackMoneyAmount >= 7500 && stackMoneyAmount < 8500)
    {
      stackMultiplier = 8;
    }
    else if (stackMoneyAmount >= 8500 && stackMoneyAmount < 9500)
    {
      stackMultiplier = 9;
    }
    else
    {
      stackMultiplier = 10;
    }


    GameManager.Instance.gemByCompleteMap
      = GameManager.Instance.gemWithStackMoney * stackMultiplier;
  }

  void CalculateGemWithIncome()
  {
    GameManager.Instance.gemWithIncome
      = (GameManager.Instance.gemByCompleteMap +
        GameManager.Instance.gemByStar +
        GameManager.Instance.gemWithStackMoney) / 100 * MenuManager.instance.incomePercentage;
  }
}
