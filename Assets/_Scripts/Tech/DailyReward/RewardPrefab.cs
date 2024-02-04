using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class RewardPrefab : MonoBehaviour
{
    [SerializeField] private Image _backImage;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _unactiveColor;

    [SerializeField] private Text _dayText;
    [SerializeField] private Text _rewardValue;

    [SerializeField] private Image _rewardIcon;
    [SerializeField] private Sprite _coinsSprite;
    [SerializeField] private Sprite _scoresSprite;

    public void SetRewardData(int day, int currentStreak, DailyReward dailyReward)
    {
        _dayText.text = $"Day {day + 1}";

        _rewardIcon.sprite = dailyReward.Type == DailyRewardType.Coins ? _coinsSprite : _scoresSprite;

        _rewardValue.text = dailyReward.Value.ToString();

        _backImage.color = day == currentStreak ? _activeColor : _unactiveColor;
    }

}
