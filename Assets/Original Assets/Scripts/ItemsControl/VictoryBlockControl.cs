using UnityEngine;

public class VictoryBlockControl : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] GameObject[] crowdPeople;
  public GameObject[] CrowdPeople { get { return crowdPeople; } }

  public Transform GetRightHandFor(int personIdx)
  {
    return crowdPeople[personIdx].GetComponent<HumannoidAnimControl>().RightHand;
  }

  public Transform GetLeftHandFor(int personIdx)
  {
    return crowdPeople[personIdx].GetComponent<HumannoidAnimControl>().LeftHand;
  }

  public void SetPersonColorFor(int personIdx, Color color)
  {
    if (personIdx > crowdPeople.Length - 1) return;

    var rend = crowdPeople[personIdx].GetComponentInChildren<Renderer>();
    rend.material.SetColor("_Color", color);
  }

  public void ChangeToCheeringAnimFor(int personIdx)
  {
    if (personIdx > crowdPeople.Length - 1) return;

    crowdPeople[personIdx].GetComponent<Animator>().SetTrigger("Cheering");
  }

  public void ChangeToSadAnimFor(int personIdx)
  {
    if (personIdx > crowdPeople.Length - 1) return;

    crowdPeople[personIdx].GetComponent<Animator>().SetBool("IsSad", true);
  }
}
