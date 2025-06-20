using UnityEngine;

public class StackPosController : MonoBehaviour
{
  public GameObject[] moneyStack;

  void Update()
  {
    var playerControl = GameManager.Instance.PlayerControl;
    transform.position
      = new Vector3(
        transform.position.x,
        transform.position.y,
        playerControl.transform.position.z
      );
  }
}
