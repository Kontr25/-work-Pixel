using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewarder : MonoBehaviour
{
    [SerializeField] private Text _status;
    [SerializeField] private Button _claimButton;
    [Space]
    [SerializeField] private RewardPrefab _rewardPrefab;
    [SerializeField] private Transform _rewardPrefabParrent;
    [Space]
    [SerializeField] private List<DailyReward> _dailyRewards;

    private List<RewardPrefab> _rewardPrefabs;

    private bool _canClaimReward;
    private int _maxStreakCount = 8;
    private float _claimCountdown = 24f;
    private float _claimDeadline = 48f;

    private int _currentSctreak
    {
        get => SaveGame.Load(Keys.RewardStreak, 0);
        set => SaveGame.Save(Keys.RewardStreak, value);
    }

    private DateTime? _lastClaimTime
    {
        get
        {
            string data = SaveGame.Load<string>(Keys.RewardLastClaimTime);

            if (string.IsNullOrEmpty(data) == false)
                return DateTime.Parse(data);

            return null;
        }
        set
        {
            if (value != null)
                SaveGame.Save(Keys.RewardLastClaimTime, value.ToString());
            else
                SaveGame.Delete(Keys.RewardLastClaimTime);
        }
    }

    private void Start()
    {
        CreatePrefabs();
        StartCoroutine(UpdateTimer());
    }

    private void CreatePrefabs()
    {
        _rewardPrefabs = new List<RewardPrefab>();

        for (int i = 0; i < _maxStreakCount; i++)
        {
            _rewardPrefabs.Add(Instantiate(_rewardPrefab, _rewardPrefabParrent, false));
        }
    }

    private void UpdateRewardsStates()
    {
        _canClaimReward = true;

        if(_lastClaimTime.HasValue == true)
        {
            var timeSpan = DateTime.UtcNow - _lastClaimTime.Value;

            if(timeSpan.TotalHours > _claimDeadline)
            {
                _lastClaimTime = null;
                _currentSctreak = 0;
            }
            else if (timeSpan.TotalHours < _claimCountdown)
            {
                _canClaimReward = false;
            }
        }

        UpdateRewardsUI();
    }

    private void UpdateRewardsUI()
    {
        _claimButton.interactable = _canClaimReward;

        if(_canClaimReward == true)
        {
            _status.text = "Claim your reward";
        }
        else
        {
            var nextClaimTime = _lastClaimTime.Value.AddHours(_claimCountdown);
            var currentClaimCountdown = nextClaimTime - DateTime.UtcNow;

            string countdown = $"{currentClaimCountdown.Hours:D2}:{currentClaimCountdown.Minutes:D2}:{currentClaimCountdown.Seconds:D2}";
            _status.text = $"Come back in {countdown} for your next reward";

            for (int i = 0; i < _rewardPrefabs.Count; i++)
            {
                _rewardPrefabs[i].SetRewardData(i, _currentSctreak, _dailyRewards[i]);
            }
        }
    }

    public void ClaimReward()
    {
        if (_canClaimReward == false)
            return;

        var reward = _dailyRewards[_currentSctreak];

        switch (reward.Type)
        {
            case DailyRewardType.None:
                break;
            case DailyRewardType.Coins:
                Debug.Log("ADD COINS By REWARD");
                break;
            case DailyRewardType.Scores:
                Debug.Log("ADD SCORES By REWARD");
                break;
            default:
                break;
        }

        _lastClaimTime = DateTime.UtcNow;
        _currentSctreak = (_currentSctreak + 1) % _maxStreakCount;

        UpdateRewardsStates();
    }

    private IEnumerator UpdateTimer()
    {
        WaitForSeconds delayTime = new WaitForSeconds(1f);

        while (true)
        {
            UpdateRewardsStates();

            yield return delayTime;
        }
    }
}

public enum DailyRewardType
{
    None,
    Coins,
    Scores
}
