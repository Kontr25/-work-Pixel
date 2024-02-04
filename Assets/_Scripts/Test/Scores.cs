using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    [SerializeField] private Text _scoresText;

    private int _currentScores;

    private void Awake()
    {
        _currentScores = SaveGame.Load(Keys.TotalScores, 0);
        UpdateValues();
    }

    public void AddScore(int count =  0)
    {
        _currentScores += count;
        UpdateValues();
    }

    public void ClearScores()
    {
        _currentScores = 0;
        UpdateValues();
    }

    public void UpdateValues()
    {
        _scoresText.text = _currentScores.ToString();
        SaveGame.Save(Keys.TotalScores, _currentScores);
    }
}
