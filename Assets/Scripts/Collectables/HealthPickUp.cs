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
            throw new NotImplementedException();
        }
    }
}
