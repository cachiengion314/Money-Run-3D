using Unity.Mathematics;
using UnityEngine;

public partial class StackIncrease : MonoBehaviour
{
  [Header("Stack Control Stuffs")]
  [SerializeField] Transform cupStackParent;
  [SerializeField] CurvedPath curvedPath;

  void Update()
  {
    for (int i = 0; i < cupStackParent.childCount; ++i)
    {
      var cup = cupStackParent.GetChild(i);
      var localPos = CalculateOriginalPosAt(i);
      cup.transform.localPosition = localPos;
    }
  }

  void OnCollected()
  {
    var index = cupStackParent.childCount;

    var cup = LevelManager.Instance.SpawnCoffeeCupAt(cupStackParent);
    cup.transform.localPosition = CalculateOriginalPosAt(index);
  }

  Vector3 CalculateOriginalPosAt(int index)
  {
    // 1 --- 3 
    //    0
    // 0 --- 2
    var i = index % 4;
    var tier = math.floor((float)index / 4);

    var horizontalDeltaLength = .09f;
    var verticalDeltaLength = .18f;

    var upRight = cupStackParent.transform.forward + cupStackParent.transform.right;
    var downLeft = -upRight;
    var upLeft = cupStackParent.transform.forward - cupStackParent.transform.right;
    var downRight = -upLeft;

    Vector3 centerLocalPos = cupStackParent.transform.InverseTransformPoint(
      curvedPath.GetCurvedStartPos()
    );
    Vector3 localPos = centerLocalPos;
    if (i == 0)
      localPos += downLeft * horizontalDeltaLength;
    else if (i == 1)
      localPos += upLeft * horizontalDeltaLength;
    else if (i == 2)
      localPos += downRight * horizontalDeltaLength;
    else if (i == 3)
      localPos += upRight * horizontalDeltaLength;

    var verticalCenterLocalPos
      = centerLocalPos + tier * verticalDeltaLength * cupStackParent.transform.up;

    verticalCenterLocalPos = cupStackParent.transform.InverseTransformPoint(
      curvedPath.FindCurvedPosAt(cupStackParent.TransformPoint(verticalCenterLocalPos))
    );

    return localPos + verticalCenterLocalPos;
  }
}
