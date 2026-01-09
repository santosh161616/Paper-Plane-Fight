using UnityEngine;

namespace Plane.Utils
{
    public static class CoinManager
    {
        private const string COIN_KEY = UniversalConstants.Coins;
        private static int coins;

        public static int Coins => coins;

        public static void Load()
        {
            coins = PlayerPrefs.GetInt(COIN_KEY, 0);
        }

        public static void AddCoins(int amount)
        {
            if (amount <= 0) return;

            coins += amount;
            Save();
        }

        public static bool SpendCoins(int amount)
        {
            if (amount <= 0) return false;
            if (coins < amount) return false;

            coins -= amount;
            Save();
            return true;
        }

        public static void ResetCoins()
        {
            coins = 0;
            Save();
        }

        private static void Save()
        {
            PlayerPrefs.SetInt(COIN_KEY, coins);
            PlayerPrefs.Save();
        }
    }
}
