using UnityEngine;

public class HelmetControl : MonoBehaviour, IObstacle
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
    rig.AddForce(50 * (
        -2 * rig.transform.forward +
        5 * rig.transform.up +
        coefficient * 6 * rig.transform.right
      )
    );
  }
}