using System.Collections;
using DG.Tweening;
using UnityEngine;

public partial class PlayerControl : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("EndOfPath"))
    {
      OnCollidedEndOfPath(other);
    }
    else if (other.gameObject.CompareTag("Uncollected"))
    {
      OnCollidedUncollected(other);
    }
    else if (other.gameObject.CompareTag("Obstacles"))
    {
      OnCollidedObstacle(other);
    }
    else if (other.gameObject.CompareTag("PowerPortal"))
    {
      OnCollidedPowerPortal(other);
    }
    else if (other.gameObject.CompareTag("VictoryBlock"))
    {
      OnCollidedVictoryBlock(other.GetComponent<VictoryBlockControl>());
    }
  }

  void InvokeVictoryRun()
  {
    var capacity = stackControl.STACK_CAPACITY;
    var givePeopleEachTime = 4;
    var range = MapRange(stackControl.CoffeeCupAmount, 0, capacity, 0, capacity / givePeopleEachTime);
    var targetPos = new Vector3(
      transform.position.x - range, transform.position.y, transform.position.z
    );
    var deltaDuration = .25f;

    transform
      .DOMove(targetPos, range * deltaDuration)
      .SetEase(Ease.Linear)
      .OnComplete(() =>
      {
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

  void OnCollidedEndOfPath(Collider other)
  {
    SoundManager.Instance.PlayHittingSfx();

    GameManager.Instance.SetGameState(GameState.Pause);
    InvokeVictoryRun();
  }

  void OnCollidedVictoryBlock(VictoryBlockControl victoryBlockControl)
  {
    for (int i = 0; i < victoryBlockControl.CrowdPeople.Length; i++)
    {
      victoryBlockControl.ChangeToCheeringAnimFor(i);
      var rightHand = victoryBlockControl.GetRightHandFor(i);
      // print("OnCollidedVictoryBlock.i " + i);
      var coffeeCups = stackControl.GetCoffeeCupsBy(2);
      for (int j = 0; j < coffeeCups.Count; ++j)
      {
        var cup = coffeeCups[j];
        cup.transform
          .DOMove(rightHand.transform.position, .2f)
          .OnComplete(() =>
          {
            cup.SetParent(rightHand);
          });
      }
      // Debug.Break();
    }
  }

  void OnCollidedPowerPortal(Collider other)
  {
    SoundManager.Instance.PlayHittingSfx();

    LevelManager.Instance.SpawnPowerUpEfxAt(other.transform.position);
    other.GetComponent<Collider>().enabled = false;

    var _operator = other.GetComponent<PowerPortalControl>().Operator;
    var _mathNumber = other.GetComponent<PowerPortalControl>().MathNumber;

    if (_operator == MathOperator.Plus)
    {
      GetComponent<StackControl>().AddCoffeeCupsBy(_mathNumber);
    }
    else if (_operator == MathOperator.Minus)
    {
      GetComponent<StackControl>().DropCoffeeCupsBy(_mathNumber);
    }
    else if (_operator == MathOperator.Multiple)
    {
      GetComponent<StackControl>().MultiplyCoffeeCupsBy(_mathNumber);
    }
    else if (_operator == MathOperator.Divide)
    {
      GetComponent<StackControl>().DivideCoffeeCupsBy(_mathNumber);
    }
  }

  void OnCollidedObstacle(Collider other)
  {
    SoundManager.Instance.PlayHittingSfx();

    LevelManager.Instance.SpawnHittingEfxAt(other.gameObject.transform.position);

    other.gameObject.GetComponent<IObstacle>().GoAway();
    other.gameObject.GetComponent<Collider>().enabled = false;
    Destroy(other.gameObject, 1.8f);

    GetComponent<StackControl>().DropCoffeeCupsBy(1);
  }

  void OnCollidedUncollected(Collider other)
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
}