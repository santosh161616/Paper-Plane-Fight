using Plane.Utils;
using UnityEngine;

namespace collectables
{
    public class CoinPickup : CollactableBase
    {
        [SerializeField] private int _coinValue = 1;
        public override void ApplyEffect(GameObject collector)
        {
            //Add the logic to add coins to the player's total here
            Debug.Log($"Coin PickUp collected! Added " +
                $"{_coinValue} coins to {collector.name}"); 
            GameEvents.Instance.GetCoins(_coinValue);
        }
    }
}
