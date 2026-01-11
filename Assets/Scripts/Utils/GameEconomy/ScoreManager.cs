using UnityEngine;

namespace Plane.Utils
{
    public static class ScoreManager
    {
        private const string SCORE_KEY = UniversalConstants.Score;
        private static int currentScore;

        public static int CurrentScore => currentScore;

        public static void Load()
        {
            currentScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
        }

        public static void Add(int value)
        {
            currentScore += value;
            Save(currentScore);
        }

        public static void Reset()
        {
            currentScore = 0;
            Save(currentScore);
        }

        private static void Save(int score)
        {
            PlayerPrefs.SetInt(SCORE_KEY, score);
            PlayerPrefs.Save();
        }
    }

}
