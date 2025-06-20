using UnityEngine;

public class TrapRotation : MonoBehaviour
{
  [SerializeField] private float rotateSpeed;

  // Update is called once per frame
  void Update()
  {
    transform.Rotate(rotateSpeed * Time.unscaledDeltaTime * Vector3.up);
  }
}
//电子邮件puhalskijsemen@gmail.com
//源码网站 开vpn全局模式打开 http://web3incubators.com/
//电报https://t.me/gamecode999
//网页客服 http://web3incubators.com/kefu.html
