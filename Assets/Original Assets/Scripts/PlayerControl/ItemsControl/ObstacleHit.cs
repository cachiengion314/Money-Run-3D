using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
  [SerializeField] private GameObject invisibleTrack;
  [SerializeField] private GameObject playerBlock;
  [SerializeField] private float knockBackDistance;
  [SerializeField] private GameObject stackPos;
  [SerializeField] private GameObject moneyIndicator;
  [SerializeField] private GameObject moneyIndicatorPos;
  [SerializeField] Animator characterAnim;
  [Header("Effects")]
  [SerializeField] ParticleSystem hittingEfx;

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Obstacles"))
    {
      //minus some Money on Player
      SoundManager.instance.PlayHittingSfx();
      Instantiate(hittingEfx, collision.gameObject.transform.position, Quaternion.identity);

      gameObject
        .GetComponent<PlayerPowerController>()
        .moneyAmount -= collision.gameObject.GetComponent<ObstacleValue>().obstacleValue;

      // show up money are minus
      GameObject newIndicator
        = Instantiate(
          moneyIndicator,
          moneyIndicatorPos.transform.position,
          Quaternion.identity
        );
      newIndicator
        .GetComponent<MoneyIndicatorValue>()
        .impactValue.text
        = string.Format(
          "-" + "{0:0}", collision.gameObject.GetComponent<ObstacleValue>().obstacleValue
        );
      newIndicator.transform.SetParent(playerBlock.transform);
      StartCoroutine(nameof(DelayIndicatorDisable), newIndicator);

      GameManager.Instance.PlayerBlockMovement.IsHit = true;
      if (gameObject.GetComponent<PlayerPowerController>().moneyAmount < 0)
      {
        // case: player have no money
        characterAnim.SetTrigger("Dead");
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.Instance.SetGameState(GameState.Pause);
        return;
      }

      // case: player still have money
      characterAnim.SetBool("Hit", true);
      playerBlock.transform.DOMoveX(transform.position.x + knockBackDistance, 1f);

      StartCoroutine(nameof(PlayerMovingToPosition));
    }
  }

  IEnumerator PlayerMovingToPosition()
  {
    yield return new WaitForSeconds(1.2f);

    characterAnim.SetBool("Hit", false);
    GameManager.Instance.PlayerBlockMovement
      .GetComponentInChildren<Animator>().SetBool("IsIdle", true);
    GameManager.Instance.PlayerBlockMovement.IsHit = false;

    GameManager.Instance.PlayerControl.enabled = true;
    gameObject.GetComponent<Rigidbody>().isKinematic = false;
  }

  IEnumerator DelayIndicatorDisable(GameObject newIndicator)
  {
    yield return new WaitForSeconds(0.36f);
    Destroy(newIndicator);
  }
}
