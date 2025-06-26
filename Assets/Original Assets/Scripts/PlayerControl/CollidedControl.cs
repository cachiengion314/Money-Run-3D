using System.Collections;
using DG.Tweening;
using UnityEngine;

public partial class PlayerBlockMovement : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("EndOfPath"))
    {
      SoundManager.instance.PlayHittingSfx();

      GameManager.Instance.SetGameState(GameState.Pause);
      InvokeVictoryRun();
    }

    if (other.gameObject.CompareTag("Uncollected"))
    {
      var dollarEffectPos = new Vector3(
        other.gameObject.transform.position.x,
        0,
        other.gameObject.transform.position.z
      );
      LevelManager.Instance.SpawnDollarEffectAt(dollarEffectPos);

      stackControl.OnCollected();

      Destroy(other.gameObject);
    }

    if (other.gameObject.CompareTag("Obstacles"))
    {
      //minus some Money on Player
      SoundManager.instance.PlayHittingSfx();

      LevelManager.Instance.SpawnHittingEfxAt(other.gameObject.transform.position);

      other.gameObject.GetComponent<IObstacle>().GoAway();
      other.gameObject.GetComponent<Collider>().enabled = false;
      Destroy(other.gameObject, 1.8f);

      GetComponent<StackControl>().DropCoffeeCupsBy(1);
    }

    if (other.gameObject.CompareTag("PowerPortal"))
    {
      SoundManager.instance.PlayHittingSfx();

      LevelManager.Instance.SpawnPowerUpEfxAt(other.transform.position);
      other.GetComponent<Collider>().enabled = false;

      var _operator = other.GetComponent<PowerPortalControl>().Operator;
      var _mathNumber = other.GetComponent<PowerPortalControl>().MathNumber;

      var player = GameManager.Instance.PlayerBlockMovement;
      if (_operator == MathOperator.Plus)
      {
        player.GetComponent<StackControl>().AddCoffeeCupsBy(_mathNumber);
      }
      else if (_operator == MathOperator.Minus)
      {
        player.GetComponent<StackControl>().DropCoffeeCupsBy(_mathNumber);
      }
      else if (_operator == MathOperator.Multiple)
      {
        player.GetComponent<StackControl>().MultiplyCoffeeCupsBy(_mathNumber);
      }
      else if (_operator == MathOperator.Divide)
      {
        player.GetComponent<StackControl>().DivideCoffeeCupsBy(_mathNumber);
      }
    }
  }

  void InvokeVictoryRun()
  {
    var capacity = stackControl.COFFEE_CUP_CAPACITY;
    var givePeopleEachTime = 4;
    var range = MapRange(stackControl.CoffeeCupAmount, 0, capacity, 0, capacity / givePeopleEachTime);
    var targetPos = new Vector3(
      transform.position.x - range, transform.position.y, transform.position.z
    );
    var deltaDuration = .2f;
    var _timer = 0.0f;

    transform
      .DOMove(targetPos, range * deltaDuration)
      .OnUpdate(() =>
      {
        _timer += Time.deltaTime;
        if (_timer >= deltaDuration)
        {
          _timer = 0;
          stackControl.DropCoffeeCupsBy(givePeopleEachTime);
        }
      })
      .SetEase(Ease.Linear)
      .OnComplete(() =>
      {
        stackControl.DropAllCoffeeCups();
        StartCoroutine(Celebrating());
      });
  }

  IEnumerator Celebrating()
  {
    GetComponentInChildren<Animator>().SetBool("IsIdle", true);
    yield return new WaitForSeconds(0.5f);

    transform.rotation = Quaternion.Euler(0, 90, 0);
    GetComponentInChildren<Animator>().SetTrigger("Won");

    yield return new WaitForSeconds(2.2f);
    GameManager.Instance.ShowWinScreenPopup();
  }
}