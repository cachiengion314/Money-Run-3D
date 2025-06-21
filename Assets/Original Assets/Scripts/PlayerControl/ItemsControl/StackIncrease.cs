using System.Collections;
using UnityEngine;

//电子邮件puhalskijsemen@gmail.com
//源码网站 开vpn全局模式打开 http://web3incubators.com/
//电报https://t.me/gamecode999
//网页客服 http://web3incubators.com/kefu.html

public partial class StackIncrease : MonoBehaviour
{
  [Header("Stack Increase Stuffs")]
  [SerializeField] private GameObject stackPos;
  [SerializeField] private GameObject moneyIndicator;
  [SerializeField] private GameObject moneyIndicatorPos;
  [SerializeField] private GameObject playerBlock;
  [Header("Effects")]
  [SerializeField] private GameObject dollarEffect;

  //When hit another building block
  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Uncollected"))
    {
      collision.gameObject.SetActive(false);

      var dollarEffectPos = new Vector3(
        collision.gameObject.transform.position.x,
        0,
        collision.gameObject.transform.position.z
      );
      Instantiate(dollarEffect, dollarEffectPos, Quaternion.identity);

      GameObject newIndicator
        = Instantiate(
          moneyIndicator,
          moneyIndicatorPos.transform.position,
          Quaternion.identity
        );
      newIndicator
        .GetComponent<MoneyIndicatorValue>()
        .impactValue.text = string.Format(
          "+" + "{0:0}", collision.gameObject.GetComponent<MoneyStackValue>().moneyValue
        );
      newIndicator.transform.SetParent(playerBlock.transform);
      StartCoroutine(nameof(DelayIndicatorDisable), newIndicator);

      //Gain some money
      gameObject
        .GetComponent<PlayerPowerController>()
        .moneyAmount += collision.gameObject.GetComponent<MoneyStackValue>().moneyValue;

      GameManager.Instance.gemWithStackMoney
        += collision.gameObject.GetComponent<MoneyStackValue>().stackValue;

      gameObject.GetComponent<EndingCalculation>().stackCollected++;

      OnCollected();
    }
  }

  IEnumerator DelayIndicatorDisable(GameObject newIndicator)
  {
    yield return new WaitForSeconds(0.36f);
    Destroy(newIndicator);
  }
}
