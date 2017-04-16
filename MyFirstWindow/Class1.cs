using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace MyFirstWindow
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class MyFirstWindow : MonoBehaviour
    {
        string _fnamesave = "GameData/MyFirstMod/myFirstWindow.sav";

        readonly int _windowId = 0;
        Rect _windowRect = new Rect(100, 100, 400, 200);
        Vector2 _scrollPosition;
        float _sliderValue = 1;
        string _textValue = "demo";
        bool _toggleValue = true;
        bool doWindow0 = true;

        readonly int _popupwindowId = 1;
        Rect _popupRect = new Rect(500, 500, 200, 100);
        bool _popupflag = false; // no visible

        private void Awake()
        {
            load_cfg();
        }

        private void OnDestroy()
        {
            save_cfg();
        }

        void load_cfg()
        {
            ConfigNode node = ConfigNode.Load(_fnamesave);

            if (node != null)
            {
                ConfigNode childnode1 = node.GetNode("Basic Window");
                if (childnode1 != null)
                {
                    if(childnode1.HasValue("_windowRect"))
                    {
                        string s = childnode1.GetValue("_windowRect");
                        int[] ia = Array.ConvertAll(s.Split(), int.Parse);
                        _windowRect.x = ia[0];
                    }
                 }
            }
        }

        void save_cfg()
        {
            ConfigNode node = new ConfigNode("MyFirstModConfig");

            ConfigNode childnode1 = node.AddNode("Basic Window");

            childnode1.AddValue("_windowRect", _windowRect);
            childnode1.AddValue("_scrollPosition", _scrollPosition);
            childnode1.AddValue("_sliderValue", _sliderValue);
            childnode1.AddValue("_textValue", _textValue);
            childnode1.AddValue("_toggleValue", _toggleValue);
            childnode1.AddValue("doWindow0", doWindow0);

            ConfigNode childnode2 = node.AddNode("Info");
            childnode2.AddValue("_popupRect", _popupRect);
            childnode2.AddValue("_popupflag", _popupflag);

            node.Save(_fnamesave);
        }

        void OnGUI()
        {
            doWindow0 = GUI.Toggle(new Rect(_windowRect.x, _windowRect.y - 30, 100, 20), doWindow0, "Window 0");
            if (doWindow0)
            {
                _windowRect = GUI.Window(_windowId, _windowRect, DoWindow0, "Basic Window");
                if (_windowRect.y < 30)
                    _windowRect.y = 30;
            }

            if (_popupflag)
            {
                _popupRect = GUILayout.Window(_popupwindowId, _popupRect, DoPopupWindow, "Info");
            }

        }

        void DoWindow0(int windowID)
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Press me"))
            {
                 _popupflag = true;
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            _sliderValue = GUILayout.HorizontalSlider(_sliderValue, 0, 10);

            GUILayout.Label("Simple text");

            if (GUILayout.RepeatButton("Simple repeat button"))
            {
                ScreenMessages.PostScreenMessage("Repeat button pressed!", .5f, ScreenMessageStyle.LOWER_CENTER);
            }

            GUILayout.TextArea("Demo text");

            _textValue = GUILayout.TextField(_textValue);

            _toggleValue = GUILayout.Toggle(_toggleValue, "Simple toggle");

            GUILayout.VerticalSlider(_sliderValue, 0, 10);

            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        void DoPopupWindow(int _windowid)
        {
            GUILayout.Label("Save and load config");
            if (GUILayout.Button("Save"))
            {
                _popupflag = false;
                save_cfg();
            }

            if (GUILayout.Button("Load"))
            {
                _popupflag = false;
                load_cfg();
            }

            if (GUILayout.Button("Cancel"))
            {
                _popupflag = false;
            }

        }
    }
}

