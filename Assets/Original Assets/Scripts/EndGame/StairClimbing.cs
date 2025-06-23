using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StairClimbing : MonoBehaviour
{
  [SerializeField] private GameObject playerBlock;
  [SerializeField] private GameObject stair;
  [SerializeField] private GameObject stackPos;
  public GameObject moneyPath;
  [SerializeField] Animator characterAnim;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Stair"))
    {
      var player = GameManager.Instance.PlayerBlockMovement;
      player.GetComponentInChildren<Animator>().SetBool("IsIdle", true);
      GameManager.Instance.SetGameState(GameState.Pause);

      print("StairClimbing ");
      // StartCoroutine(nameof(StartClimbing));
    }
  }

  IEnumerator StartClimbing()
  {
    yield return new WaitForSeconds(0.5f);

    gameObject.GetComponent<Rigidbody>().isKinematic = true;

    float stackMoneyCollected = gameObject.GetComponent<PlayerPowerController>().moneyAmount;
    int checkpoint = 1000;

    for (int i = 0; i < stair.transform.childCount; i++)
    {
      if (stackMoneyCollected < checkpoint)
      {
        playerBlock.transform.DOMove(stair.transform.GetChild(i).transform.position + new Vector3(0, 0.9f, 0), 0.5f + (0.5f * i)).SetEase(Ease.Linear);
        StartCoroutine(nameof(StairClimbingDelay), i);
        yield break;
      }
      else if (stackMoneyCollected == checkpoint)
      {
        int j = i + 1;
        playerBlock.transform.DOMove(stair.transform.GetChild(j).transform.position + new Vector3(0, 0.9f, 0), 0.5f + (0.5f * i)).SetEase(Ease.Linear);
        StartCoroutine(nameof(StairClimbingDelay), j);
        yield break;
      }
      else if (checkpoint >= 10000)
      {
        playerBlock.transform.DOMove(stair.transform.GetChild(i + 1).transform.position + new Vector3(0, 0.9f, 0), 0.5f + (0.5f * i)).SetEase(Ease.Linear);
        StartCoroutine(nameof(StairClimbingDelay), i);
        yield break;
      }
      else
      {
        checkpoint += 500;
      }
    }
  }

  IEnumerator StairClimbingDelay(int i)
  {
    yield return new WaitForSeconds(0.5f + (0.5f * i));
    stackPos.SetActive(false);
    moneyPath.SetActive(false);
    characterAnim.SetTrigger("Interupt");

    StartCoroutine(nameof(FallingInTheStaircase));
  }

  IEnumerator FallingInTheStaircase()
  {
    yield return new WaitForSeconds(0.4f);
    gameObject.GetComponent<Rigidbody>().isKinematic = false;

    StartCoroutine(nameof(Celebrating));
  }

  IEnumerator Celebrating()
  {
    yield return new WaitForSeconds(0.5f);
    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
    characterAnim.SetTrigger("Won");

    StartCoroutine(nameof(DelayGameWinning));
  }

  IEnumerator DelayGameWinning()
  {
    yield return new WaitForSeconds(3f);
    GameManager.Instance.hasWon = true;
  }
}
