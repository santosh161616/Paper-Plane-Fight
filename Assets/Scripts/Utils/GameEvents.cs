using System;
using UnityEngine;

namespace Plane.Utils
{
    public class GameEvents : SingletonMonoBehaviour<GameEvents>
    {
        public Action<int> OnRewardReceived = delegate { };
        public void RewardReceived(int rewardAmount)
        {
            OnRewardReceived?.Invoke(rewardAmount);
        }

        public Action<int> OnHealthReceived = delegate { };
        public void HealthReceived(int healthAmount)
        {
            OnHealthReceived?.Invoke(healthAmount);
        }

        public Action<int> OnUpdateHealthUI = delegate { };
        public void UpdateHealthUI(int currentHealth)
        {
            OnUpdateHealthUI?.Invoke(currentHealth);
        }

        public Action OnGameOver = delegate { };
        public void GameOver()
        {
            OnGameOver?.Invoke();
        }

        public Action<int> OnAddtoScore = delegate { };
        public void AddtoScore(int scoreValue)
        {
            OnAddtoScore?.Invoke(scoreValue);
        }

        public Action<int> OnScoreReceived = delegate { }; 
        public void ScoreReceived(int score)
        {
            OnScoreReceived?.Invoke(score);
        }

        public Action<int> OnGetCoin = delegate { };
        public void GetCoins(int coinValue)
        {
            OnGetCoin?.Invoke(coinValue);
        }
    }
}
