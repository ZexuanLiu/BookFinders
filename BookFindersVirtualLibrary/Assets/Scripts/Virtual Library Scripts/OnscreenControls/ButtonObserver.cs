using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Virtual_Library_Scripts.OnscreenControls
{
    public static class ButtonObserver
    {
        public static volatile ButtonMode currentButtonMode = ButtonMode.Default;


        public static event Action onButtonModeChanged;
        public static void ButtonModeChanged()
        {
            if (onButtonModeChanged != null)
            {
                onButtonModeChanged();
            }
        }












        private static Dictionary<OnScreenButtonType, IButtonClickTracker> buttons = new Dictionary<OnScreenButtonType, IButtonClickTracker>();

        static ButtonObserver()
        {
            buttons.Add(OnScreenButtonType.A, null);
            buttons.Add(OnScreenButtonType.B, null);
            buttons.Add(OnScreenButtonType.X, null);
            buttons.Add(OnScreenButtonType.Y, null);
            buttons.Add(OnScreenButtonType.Up, null);
            buttons.Add(OnScreenButtonType.Left, null);
            buttons.Add(OnScreenButtonType.Down, null);
            buttons.Add(OnScreenButtonType.Right, null);
            buttons.Add(OnScreenButtonType.LeftJoystick, null);
            buttons.Add(OnScreenButtonType.RightJoystick, null);
        }

        public static void AddButton(OnScreenButtonType buttonType, IButtonClickTracker button)
        {
            buttons[buttonType] = button;
        }

        public static bool IsButtonClicked(OnScreenButtonType buttonType)
        {
            if (buttons[buttonType] == null)
            {
                return false;
            }
            if (buttons[buttonType].IsButtonClicked())
            {
                buttons[buttonType].SetButtonClicked(false);
                return true;
            }
            return false;
        }

        public static bool IsButtonHeld(OnScreenButtonType buttonType)
        {
            if (buttons[buttonType] == null)
            {
                return false;
            }
            return buttons[buttonType].IsButtonHeld();
        }

        public static IEnumerable<OnScreenButtonType> GetAllButtons()
        {
            return buttons.Keys;
        }

        public static ButtonMode GetButtonMode()
        {
            foreach (OnScreenButtonType button in buttons.Keys)
            {
                if (buttons[button] != null)
                {
                    return buttons[button].GetButtonMode();
                }
            }
            return ButtonMode.Default;
                
        }

        public static void SetButtonsMode(ButtonMode buttonMode)
        {
            foreach (OnScreenButtonType button in buttons.Keys)
            {
                if (buttons[button] != null)
                {
                    buttons[button].SetButtonMode(buttonMode);
                }
            }
        }
    }
}
