using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

[Serializable]
public enum LevelObjType
{
  None,
  CoffeeCup,
  Barrier,
  Helmet,
  PowerPortal
}

[Serializable]
public class LevelObj
{
  public LevelObjType type;
}

[Serializable]
public class LevelObjsPath
{
  [Tooltip("X value of this vector mean 'from position', Y value mean 'to position'")]
  public Vector2 RangePercentPosition;
  [Tooltip("The amount of items in RangePercentPosition, and the system will spawn items in vertical")]
  [Range(0, 100)]
  public int ItemsInRangeAmount;
  [Tooltip("The amount of items in horizontal, and the system will spawn items in horizontal")]
  public LevelObj[] LevelObjs;
  [Tooltip("If true the obj will not spawn base on its index of the LevelObjs but randomly spawn")]
  public bool IsRandomHorizontalPosition;
}

[Serializable]
public class LevelInformation
{
  public int Index;
  public LevelObjsPath[] LevelObjsPaths;
  [Range(10f, 250f)]
  public float TotalLevelLength;
}

public partial class LevelManager : MonoBehaviour
{
  [Header("Spawn Level Objects")]
  [SerializeField] Transform levelObjsParent;
  [SerializeField] LevelInformation levelInformation;
  [Space(10)]
  [Header("Level Editor")]
  [SerializeField][Range(1, 20)] int levelSelected = 1;
  [SerializeField] bool isSelectedMatchedCurrentLevel;
  public bool IsSelectedMatchedCurrentLevel { get { return isSelectedMatchedCurrentLevel; } }

  [NaughtyAttributes.Button]
  void LoadLevel()
  {
    LoadLevelFrom(levelSelected);
  }

  [NaughtyAttributes.Button]
  void SaveLevel()
  {
    levelInformation.Index = levelSelected - 1;
    HoangNam.SaveSystem.Save(
      levelInformation,
      "Resources/" + Constants.NAME_LEVEL_FILE + levelSelected
    );
    print("Save level successfully");
  }

  void FitBoxColliderToSplineMesh(SplineComputer splineComputer, float totalLength)
  {
    if (!splineComputer.TryGetComponent<BoxCollider>(out var boxCollider))
      boxCollider = splineComputer.gameObject.AddComponent<BoxCollider>();

    var centerPos
      = (splineComputer.GetPoint(0).position + splineComputer.GetPoint(1).position) / 2f;
    centerPos -= new Vector3(0, boxCollider.size.y / 2f, 0);
    boxCollider.center = centerPos;
    boxCollider.size = new Vector3(totalLength, boxCollider.size.y, boxCollider.size.z);
  }

  public void SpawnLevelObjsFrom(LevelInformation information)
  {
    var levelObjsPaths = information.LevelObjsPaths;
    var totalLevelLength = information.TotalLevelLength;

    var start = curvedPath.GetCurvedStart();
    var end = curvedPath.GetCurvedEnd();

    var targetPos = start.position + Vector3.left * totalLevelLength;
    end.transform.position = targetPos;
    endOfPathCollider.transform.position = targetPos;
    splineComputer.SetPointPosition(1, targetPos);
    FitBoxColliderToSplineMesh(splineComputer, totalLevelLength);

    for (int i = 0; i < levelObjsPaths.Length; ++i)
    {
      var path = levelObjsPaths[i];
      var range = path.RangePercentPosition;
      if ((range.y - range.x) > 0 && path.ItemsInRangeAmount > 0)
      {
        float deltaLength = (range.y - range.x) / path.ItemsInRangeAmount;
        for (int j = 0; j < path.ItemsInRangeAmount; ++j)
          SpawnObjsAt(range.x + j * deltaLength, path.LevelObjs, path.IsRandomHorizontalPosition);
        continue;
      }

      SpawnObjsAt(range.x, path.LevelObjs, path.IsRandomHorizontalPosition);
    }
  }

  void SpawnObjsAt(float percentPosition, LevelObj[] levelObjs, bool _isRandomPos = false)
  {
    var percent = (float)percentPosition / 100;
    var centerPos = curvedPath.FindCurvedPosAt(percent);

    var _levelObjIndexes = new List<int>(levelObjs.Length);
    for (int j = 0; j < levelObjs.Length; ++j)
      _levelObjIndexes.Add(j);

    for (int j = 0; j < levelObjs.Length; ++j)
    {
      var obj = levelObjs[j];
      if (obj.type == LevelObjType.None) continue;

      GameObject spawnedObj = null;
      if (obj.type == LevelObjType.CoffeeCup)
      {
        spawnedObj = Instantiate(coffeeCupPref, levelObjsParent);
      }
      else if (obj.type == LevelObjType.Barrier)
      {
        spawnedObj = Instantiate(barrierBarPref, levelObjsParent);
      }
      else if (obj.type == LevelObjType.Helmet)
      {
        spawnedObj = Instantiate(helmetPref, levelObjsParent);
      }
      else if (obj.type == LevelObjType.PowerPortal)
      {
        spawnedObj = Instantiate(powerPortal, levelObjsParent);
        var mathNumbers = new int[4] { 1, 2, 3, 4 };
        var mathOperators = new MathOperator[4] {
          MathOperator.Divide,
          MathOperator.Minus,
          MathOperator.Multiple,
          MathOperator.Plus,
        };
        spawnedObj.GetComponent<PowerPortalControl>().SetMathNumber(
          mathNumbers[UnityEngine.Random.Range(0, mathNumbers.Length)]
        );
        spawnedObj.GetComponent<PowerPortalControl>().SetMathOperator(
          mathOperators[UnityEngine.Random.Range(0, mathOperators.Length)]
        );
      }
      if (spawnedObj == null) continue;

      var randIdx = UnityEngine.Random.Range(0, _levelObjIndexes.Count);
      int J = _levelObjIndexes[randIdx];
      _levelObjIndexes.RemoveAt(randIdx);

      if (levelObjs.Length == 1)
      {
        if (!_isRandomPos)
          spawnedObj.transform.position = centerPos + new Vector3(0, 0, .0f + j);
        else
          spawnedObj.transform.position
          = centerPos + new Vector3(0, 0, .0f + J);
      }
      if (levelObjs.Length == 2)
      {
        if (!_isRandomPos)
          spawnedObj.transform.position = centerPos + new Vector3(0, 0, -.5f + j);
        else
          spawnedObj.transform.position
          = centerPos + new Vector3(0, 0, -.5f + J);
      }
      if (levelObjs.Length == 3)
      {
        if (!_isRandomPos)
          spawnedObj.transform.position = centerPos + new Vector3(0, 0, -.5f + .5f * j);
        else
          spawnedObj.transform.position
          = centerPos + new Vector3(0, 0, -.5f + .5f * J);
      }
      if (levelObjs.Length == 4)
      {
        if (!_isRandomPos)
          spawnedObj.transform.position = centerPos + new Vector3(0, 0, -1.0f + .5f * j);
        else
          spawnedObj.transform.position
          = centerPos + new Vector3(0, 0, -1.0f + .5f * J);
      }
    }
  }

  public void LoadLevelFrom(int level)
  {
    var levelInfo = HoangNam.SaveSystem.Load<LevelInformation>(
      "Resources/" + Constants.NAME_LEVEL_FILE + level
    );
    if (levelInfo == null) { print("Level not existed!"); return; }
    levelInformation = levelInfo;
    print("Load level successfully");
  }
}