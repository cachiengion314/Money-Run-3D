using UnityEngine;

public class BarrierControl : MonoBehaviour, IObstacle
{
  [SerializeField] Rigidbody rig;
  [SerializeField][Range(0, 10000)] int value;

  public int GetValue()
  {
    return value;
  }

  public void GoAway()
  {
    var arr = new int[2] { -1, 1 };
    var coefficient = arr[Random.Range(0, arr.Length)];
    rig.AddForce(200 * (
        -1 * rig.transform.forward +
        1 * rig.transform.up +
        coefficient * 1 * rig.transform.right
      )
    );
  }
}