using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//电子邮件puhalskijsemen@gmail.com
//源码网站 开vpn全局模式打开 http://web3incubators.com/
//电报https://t.me/gamecode999
//网页客服 http://web3incubators.com/kefu.html

public partial class StackControl : MonoBehaviour
{
  [Header("Coffee Cup Control Center")]
  public readonly int STACK_CAPACITY = 100;
  [SerializeField] Transform coffeeCupParent;
  public int CoffeeCupAmount { get { return coffeeCupParent.childCount; } }
  [SerializeField] CurvedPath curvedPath;
  readonly float _verticalDeltaLength = .14f;
  readonly float _horizontalDeltaLength = .09f;

  public void DropAllCoffeeCups()
  {
    DropCoffeeCupsBy(coffeeCupParent.childCount);
  }

  public List<Transform> GetCoffeeCupsBy(int amount)
  {
    var remainAmount = math.max(0, coffeeCupParent.childCount - amount);
    var list = new List<Transform>(amount);
    for (int i = coffeeCupParent.childCount - 1; i >= remainAmount; i--)
    {
      var cup = coffeeCupParent.GetChild(i);
      if (cup.TryGetComponent<Collider>(out var col))
      {
        col.enabled = false;
      }
      list.Add(cup);
    }
    return list;
  }

  public void DropCoffeeCupsBy(int dropAmount)
  {
    var remainAmount = math.max(0, coffeeCupParent.childCount - dropAmount);

    for (int i = coffeeCupParent.childCount - 1; i >= remainAmount; i--)
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

      Destroy(cup.gameObject, 1.8f);
    }
    // if (dropAmount > 0)
    // {
    //   UpdateCurvedEndPosition(math.max(1, remainAmount));
    //   UpdateCurvedPosCups();
    // }

    LevelManager.Instance.CheckLoseCondition();
  }

  public void DivideCoffeeCupsBy(int amount)
  {
    var newCupsAmount = (float)(coffeeCupParent.childCount / amount);
    var _dropAmount = (int)(coffeeCupParent.childCount - math.ceil(newCupsAmount));
    _dropAmount = math.min(_dropAmount, coffeeCupParent.childCount - 1);

    DropCoffeeCupsBy(_dropAmount);
  }

  public void MultiplyCoffeeCupsBy(int amount)
  {
    var newCupsAmount = coffeeCupParent.childCount * amount;
    var additionAmount = newCupsAmount - coffeeCupParent.childCount;
    AddCoffeeCupsBy(additionAmount);
  }

  public void AddCoffeeCupsBy(int amount)
  {
    UpdateCurvedEndPosition(
      math.min(coffeeCupParent.childCount + amount, STACK_CAPACITY)
    );

    for (int i = 0; i < amount; ++i)
    {
      var index = coffeeCupParent.childCount;
      if (coffeeCupParent.childCount >= STACK_CAPACITY)
      {
        return;
      }
      var cup = LevelManager.Instance.SpawnCoffeeCupAt(coffeeCupParent);
      if (cup.TryGetComponent<Collider>(out var col))
      {
        col.enabled = false;
      }
      cup.transform.localPosition = CalculateLocalPosCupAt(index);
    }
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

  public void UpdateCurvedEndPosition(int updatedCupCount)
  {
    var tier = math.floor((float)updatedCupCount / 4);
    Vector3 centerPos = curvedPath.GetCurvedStartPos();
    var verticalCenterPos = centerPos + tier * _verticalDeltaLength * Vector3.up;
    verticalCenterPos += 1.0f * Vector3.up;
    curvedPath.GetCurvedEnd().position = verticalCenterPos;
    curvedPath.GetCurvedCenterControl().position = verticalCenterPos;

    curvedPath.UpdateTotalLength();
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

  public void OnCollected()
  {
    AddCoffeeCupsBy(1);
  }
}
