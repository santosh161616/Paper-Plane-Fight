using System;
using UnityEngine;

namespace collectables
{
    public interface ICollectable
    {
        void PickUp(GameObject collector){}   
    }
}