using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
  public float moneyAmount;
  [Header("Effects")]
  [SerializeField] ParticleSystem powerUpEfx;

  private void Start()
  {
    moneyAmount = 0;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("PowerPortal"))
    {
      other.GetComponent<Collider>().enabled = false;
      Instantiate(powerUpEfx, other.transform.position, Quaternion.identity);
      var _operator = other.GetComponent<PowerPortalControl>().Operator;
      var _mathNumber = other.GetComponent<PowerPortalControl>().MathNumber;

      var player = GameManager.Instance.PlayerBlockMovement;
      if (_operator == MathOperator.Plus)
      {
        player.GetComponent<StackIncrease>().AddCoffeeCupsWith(_mathNumber);
      }
      else if (_operator == MathOperator.Minus)
      {
        player.GetComponent<StackIncrease>().DropCoffeeCups(_mathNumber);
      }
      else if (_operator == MathOperator.Multiple)
      {
        player.GetComponent<StackIncrease>().MultiplyCoffeeCupsWith(_mathNumber);
      }
      else if (_operator == MathOperator.Divide)
      {
        player.GetComponent<StackIncrease>().DivideCoffeeCupsWith(_mathNumber);
      }
    }
  }
}
