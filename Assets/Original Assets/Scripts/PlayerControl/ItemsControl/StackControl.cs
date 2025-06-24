using Unity.Mathematics;
using UnityEngine;

public partial class StackIncrease : MonoBehaviour
{
  [Header("Coffee Cup Control Center")]
  readonly int _COFFEE_CUP_CAPACITY = 80;
  [SerializeField] Transform coffeeCupParent;
  public int CoffeeCupAmount { get { return coffeeCupParent.childCount; } }
  [SerializeField] CurvedPath curvedPath;
  readonly float _verticalDeltaLength = .14f;
  readonly float _horizontalDeltaLength = .09f;

  public void DropAllCoffeeCups()
  {
    DropCoffeeCups(coffeeCupParent.childCount);
  }

  public void DropCoffeeCups(int dropAmount)
  {
    var amount = math.max(0, coffeeCupParent.childCount - dropAmount);
    for (int i = coffeeCupParent.childCount - 1; i >= amount; i--)
    {
      var cup = coffeeCupParent.GetChild(i);
      if (cup.TryGetComponent<PhysicMovement>(out var movement))
      {
        movement.SetAutoUpdate(true);
      }
      if (cup.TryGetComponent<Collider>(out var col))
      {
        col.enabled = false;
      }
      cup.SetParent(null);
      Destroy(cup.gameObject, 2.5f);
    }
    if (dropAmount > 0)
      UpdateCurvedPosCups();
  }

  public void UpdateCurvedPosCups()
  {
    for (int i = 0; i < coffeeCupParent.childCount; ++i)
    {
      var cup = coffeeCupParent.GetChild(i);
      var localPos = CalculateLocalPosCupAt(i);
      var worldPos = coffeeCupParent.TransformPoint(localPos);
      var direction = curvedPath.FindDirectionAt(worldPos);

      cup.transform.up = direction.normalized;
      cup.transform.localPosition = localPos;
    }
  }

  public void UpdateCurvedEndPosition()
  {
    var tier = math.floor((float)coffeeCupParent.childCount / 4);
    Vector3 centerPos = curvedPath.GetCurvedStartPos();
    var verticalCenterPos = centerPos + tier * _verticalDeltaLength * Vector3.up;
    verticalCenterPos += 1.0f * Vector3.up;
    curvedPath.GetCurvedEnd().position = verticalCenterPos;
    curvedPath.GetCurvedCenterControl().position = verticalCenterPos;

    curvedPath.UpdateTotalLength();
  }

  public void DivideCoffeeCupsWith(int amount)
  {
    var newCupsAmount = (int)math.ceil((float)coffeeCupParent.childCount / amount);
    var _amount = coffeeCupParent.childCount - newCupsAmount;
    DropCoffeeCups(_amount);
  }

  public void MultiplyCoffeeCupsWith(int amount)
  {
    var newCupsAmount = coffeeCupParent.childCount * amount;
    var additionAmount = newCupsAmount - coffeeCupParent.childCount;
    AddCoffeeCupsWith(additionAmount);
  }

  public void AddCoffeeCupsWith(int amount)
  {
    for (int i = 0; i < amount; ++i)
    {
      var index = coffeeCupParent.childCount;
      if (coffeeCupParent.childCount >= _COFFEE_CUP_CAPACITY)
      {
        UpdateCurvedEndPosition();
        return;
      }
      var cup = LevelManager.Instance.SpawnCoffeeCupAt(coffeeCupParent);
      cup.transform.localPosition = CalculateLocalPosCupAt(index);
    }
    UpdateCurvedEndPosition();
  }

  public void AddOneCoffeeCup()
  {
    if (coffeeCupParent.childCount >= _COFFEE_CUP_CAPACITY) return;

    var index = coffeeCupParent.childCount;

    var cup = LevelManager.Instance.SpawnCoffeeCupAt(coffeeCupParent);
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

    var upRight = coffeeCupParent.transform.forward + coffeeCupParent.transform.right;
    var downLeft = -upRight;
    var upLeft = coffeeCupParent.transform.forward - coffeeCupParent.transform.right;
    var downRight = -upLeft;

    Vector3 centerLocalPos = coffeeCupParent.transform.InverseTransformPoint(
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
      = centerLocalPos + tier * _verticalDeltaLength * coffeeCupParent.transform.up;

    verticalCenterLocalPos = coffeeCupParent.transform.InverseTransformPoint(
      curvedPath.FindCurvedPosAt(coffeeCupParent.TransformPoint(verticalCenterLocalPos))
    );

    return localPos + verticalCenterLocalPos;
  }

  void OnCollected()
  {
    AddOneCoffeeCup();
  }
}
