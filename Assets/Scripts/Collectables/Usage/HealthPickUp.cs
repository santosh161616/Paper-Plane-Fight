using System;
using UnityEngine;
using Plane.Utils;

namespace collectables
{

    public class HealthPickUp : CollactableBase
    {
        [SerializeField] private int _value = 1;

        public override void ApplyEffect(GameObject collector)
        {
            //Add logic here for pickup effect
            Debug.Log($"Health PickUp collected! Restored " +
                $"{_value} health to {collector.name}");
        }
    }
}
