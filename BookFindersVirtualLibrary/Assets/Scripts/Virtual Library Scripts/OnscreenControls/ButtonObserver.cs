using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Virtual_Library_Scripts.OnscreenControls
{
    public enum ButtonMode
    {
        Default, VirtualLibrary, LibraryGuide, Menu
    }

    public static class ButtonObserver
    {
        public static volatile ButtonMode currentButtonMode = ButtonMode.Default;
    }
}
