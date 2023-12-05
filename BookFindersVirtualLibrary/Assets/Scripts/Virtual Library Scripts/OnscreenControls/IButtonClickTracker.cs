using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Virtual_Library_Scripts.OnscreenControls
{
    public interface IButtonClickTracker
    {
        public OnScreenButtonType GetButtonType();

        public bool IsButtonClicked();

        public void SetButtonClicked(bool buttonClicked);

        public bool IsButtonHeld();

        public void SetButtonMode(ButtonMode buttonMode);

        public ButtonMode GetButtonMode();

        public void AddToButtonObserver();
    }
}
