using System;
using UnityEngine;

namespace Plane.popups
{
    public interface IPopup 
    {
        abstract void Show(string message);
        abstract void Show(string message, string title);
        abstract void Show(string message, string title, Action onYes);
        abstract void Show(string message, string title, Action onYes, Action onNo);
    }
}
