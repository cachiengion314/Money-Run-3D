using TMPro;
using UnityEngine;

public enum MathOperator
{
  Plus,
  Minus,
  Multiple,
  Divide
}

public class PowerPortalControl : MonoBehaviour, ICollectable
{
  [SerializeField] TMP_Text mathNumberTxt;
  [SerializeField] TMP_Text mathOperatorTxt;
  public MathOperator Operator { get; private set; }
  public int MathNumber { get; private set; }

  public int GetValue()
  {
    return MathNumber;
  }

  public void SetMathNumber(int number)
  {
    MathNumber = number;
    mathNumberTxt.text = MathNumber.ToString();
  }

  public void SetMathOperator(MathOperator op)
  {
    Operator = op;
    var _operator = "";
    switch (Operator)
    {
      case MathOperator.Plus:
        _operator = "+";
        break;
      case MathOperator.Minus:
        _operator = "-";
        break;
      case MathOperator.Multiple:
        _operator = "x";
        break;
      case MathOperator.Divide:
        _operator = ":";
        break;
    }
    mathOperatorTxt.text = _operator;
  }
}