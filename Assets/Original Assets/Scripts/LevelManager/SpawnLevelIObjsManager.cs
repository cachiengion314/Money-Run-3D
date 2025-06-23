using System;
using Dreamteck.Splines;
using UnityEngine;

[Serializable]
public enum LevelObjType
{
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
  [Range(0, 100)]
  public int PercentPosition;
  public LevelObj[] LevelObjs;
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
    print("Save successfully");
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

      var percent = (float)path.PercentPosition / 100;
      var centerPos = curvedPath.FindCurvedPosAt(percent);
      var levelObjs = path.LevelObjs;

      for (int j = 0; j < levelObjs.Length; ++j)
      {
        var obj = levelObjs[j];
        GameObject spawnedObj = null;
        if (obj.type == LevelObjType.CoffeeCup)
        {
          spawnedObj = Instantiate(coffeeCupPref, levelObjsParent);
        }
        spawnedObj.transform.position = centerPos + new Vector3(0, 0, -.5f + j);
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
    print("Load successfully");
  }
}