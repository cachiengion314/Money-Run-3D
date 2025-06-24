using System.Collections;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
  [SerializeField] Animator characterAnim;
  [Header("Effects")]
  [SerializeField] ParticleSystem hittingEfx;

  private void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.CompareTag("Obstacles"))
    {
      //minus some Money on Player
      SoundManager.instance.PlayHittingSfx();
      Instantiate(hittingEfx, col.gameObject.transform.position, Quaternion.identity);

      col.gameObject.GetComponent<IObstacle>().GoAway();
      gameObject
        .GetComponent<PlayerPowerController>()
        .moneyAmount -= col.gameObject.GetComponent<IObstacle>().GetValue();

      var player = GameManager.Instance.PlayerBlockMovement;
      player.IsHit = true;
      player.GetComponent<StackIncrease>().DropCoffeeCups(1);
      characterAnim.SetBool("Hit", true);
      StartCoroutine(nameof(PlayerMovingToPosition));
    }
  }

  IEnumerator PlayerMovingToPosition()
  {
    yield return null;
    characterAnim.SetBool("Hit", false);
    GameManager.Instance.PlayerBlockMovement.IsHit = false;
  }
}
