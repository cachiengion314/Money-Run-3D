using Unity.Mathematics;
using UnityEngine;

public partial class StackIncrease : MonoBehaviour
{
  [Header("Stack Control Stuffs")]
  [SerializeField] Transform cupStackParent;
  [SerializeField] CurvedPath curvedPath;
  readonly float _verticalDeltaLength = .18f;
  readonly float _horizontalDeltaLength = .09f;

  public void UpdateCurvedPosCups()
  {
    for (int i = 0; i < cupStackParent.childCount; ++i)
    {
      var cup = cupStackParent.GetChild(i);
      var localPos = CalculateLocalPosCupAt(i);
      var worldPos = cupStackParent.TransformPoint(localPos);
      var direction = curvedPath.FindDirectionAt(worldPos);

      cup.transform.up = direction.normalized;
      cup.transform.localPosition = localPos;
    }
  }

  public void UpdateCurvedEndPosition()
  {
    var tier = math.floor((float)cupStackParent.childCount / 4);
    Vector3 centerPos = curvedPath.GetCurvedStartPos();
    var verticalCenterPos = centerPos + tier * _verticalDeltaLength * Vector3.up;
    curvedPath.GetCurvedEnd().position = verticalCenterPos + 1.0f * Vector3.up;

    curvedPath.UpdateTotalLength();
  }

  void OnCollected()
  {
    var index = cupStackParent.childCount;

    var cup = LevelManager.Instance.SpawnCoffeeCupAt(cupStackParent);
    cup.transform.localPosition = CalculateLocalPosCupAt(index);

    UpdateCurvedEndPosition();
  }

  Vector3 CalculateLocalPosCupAt(int index)
  {
    // 1 --- 3 
    //    0
    // 0 --- 2
    var i = index % 4;
    var tier = math.floor((float)index / 4);

    var upRight = cupStackParent.transform.forward + cupStackParent.transform.right;
    var downLeft = -upRight;
    var upLeft = cupStackParent.transform.forward - cupStackParent.transform.right;
    var downRight = -upLeft;

    Vector3 centerLocalPos = cupStackParent.transform.InverseTransformPoint(
      curvedPath.GetCurvedStartPos()
    );
    Vector3 localPos = centerLocalPos;
    if (i == 0)
      localPos += downLeft * _horizontalDeltaLength;
    else if (i == 1)
      localPos += upLeft * _horizontalDeltaLength;
    else if (i == 2)
      localPos += downRight * _horizontalDeltaLength;
    else if (i == 3)
      localPos += upRight * _horizontalDeltaLength;

    var verticalCenterLocalPos
      = centerLocalPos + tier * _verticalDeltaLength * cupStackParent.transform.up;

    verticalCenterLocalPos = cupStackParent.transform.InverseTransformPoint(
      curvedPath.FindCurvedPosAt(cupStackParent.TransformPoint(verticalCenterLocalPos))
    );

    return localPos + verticalCenterLocalPos;
  }
}
