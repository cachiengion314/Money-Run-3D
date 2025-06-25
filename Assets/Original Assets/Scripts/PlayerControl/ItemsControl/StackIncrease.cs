using System.Collections;
using UnityEngine;

//电子邮件puhalskijsemen@gmail.com
//源码网站 开vpn全局模式打开 http://web3incubators.com/
//电报https://t.me/gamecode999
//网页客服 http://web3incubators.com/kefu.html

public partial class StackIncrease : MonoBehaviour
{
  [Header("Stack Increase Stuffs")]
  [Header("Effects")]
  [SerializeField] private GameObject dollarEffect;

  //When hit another building block
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Uncollected"))
    {
      var dollarEffectPos = new Vector3(
        other.gameObject.transform.position.x,
        0,
        other.gameObject.transform.position.z
      );
      Instantiate(dollarEffect, dollarEffectPos, Quaternion.identity);

      OnCollected();

      Destroy(other.gameObject);
    }
  }
}
