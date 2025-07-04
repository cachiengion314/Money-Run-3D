using UnityEngine;

public partial class GameManager : MonoBehaviour
{
  private int _currentLevelIndex;
  public int CurrentLevelIndex
  {
    get
    {
      return _currentLevelIndex;
    }
    set
    {
      _currentLevelIndex = value;
      PlayerPrefs.SetInt(Constants.KEY_CURRENT_LVL_INDEX, value);
    }
  }

  private bool _isSoundOn;
  public bool IsSoundOn
  {
    get
    {
      return _isSoundOn;
    }
    set
    {
      _isSoundOn = value;
      var _intOn = _isSoundOn ? 1 : 0;
      PlayerPrefs.SetInt(Constants.KEY_IS_SOUND_ON, _intOn);
    }
  }

  private bool _isHapticOn;
  public bool IsHapticOn
  {
    get
    {
      return _isHapticOn;
    }
    set
    {
      _isHapticOn = value;
      var _intOn = _isHapticOn ? 1 : 0;
      PlayerPrefs.SetInt(Constants.KEY_IS_HAPTIC_ON, _intOn);
    }
  }

  void InitUserData()
  {
    var isSoundInt = PlayerPrefs.GetInt(Constants.KEY_IS_SOUND_ON, 1);
    _isSoundOn = isSoundInt == 1;
    var isHapticInt = PlayerPrefs.GetInt(Constants.KEY_IS_HAPTIC_ON, 1);
    _isHapticOn = isHapticInt == 1;

    _currentLevelIndex = PlayerPrefs.GetInt(Constants.KEY_CURRENT_LVL_INDEX, 0);
  }
}