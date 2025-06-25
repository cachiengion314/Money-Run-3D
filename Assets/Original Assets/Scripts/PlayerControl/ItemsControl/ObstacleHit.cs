using System.Collections;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
  [SerializeField] Animator characterAnim;
  [Header("Effects")]
  [SerializeField] ParticleSystem hittingEfx;

  private void OnTriggerEnter(Collider col)
  {
    if (col.gameObject.CompareTag("Obstacles"))
    {
      //minus some Money on Player
      SoundManager.instance.PlayHittingSfx();
      Instantiate(hittingEfx, col.gameObject.transform.position, Quaternion.identity);

      col.gameObject.GetComponent<IObstacle>().GoAway();
      col.gameObject.GetComponent<Collider>().enabled = false;
      gameObject
        .GetComponent<PlayerPowerController>()
        .moneyAmount -= col.gameObject.GetComponent<IObstacle>().GetValue();
      Destroy(col.gameObject, 1.8f);

      var player = GameManager.Instance.PlayerBlockMovement;
      player.GetComponent<StackIncrease>().DropCoffeeCups(1);
    }
  }
}
