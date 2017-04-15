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
//        [Persistent]
        Rect _windowRect = new Rect(100, 100, 400, 200);
//        [Persistent]
        Vector2 _scrollPosition;
//        [Persistent]
        float _sliderValue = 1;
//        [Persistent]
        string _textValue = "demo";
//        [Persistent]
        bool _toggleValue = true;
//        [Persistent]
        bool doWindow0 = true;

        readonly int _popupwindowId = 1;
//        [Persistent]
        Rect _popupRect = new Rect(500, 500, 200, 100);
//        [Persistent]
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
                //                ConfigNode.LoadObjectFromConfig(this, node);
                ConfigNode childnode = node.GetNode("Basic_Window");
                if (childnode != null)
                {
                    if (childnode.HasValue("_windowRect"))
                    {
                        string s = childnode.GetValue("_windowRect");
                        try
                        {
                            int[] ia = Array.ConvertAll(s.Split(), int.Parse);
                            _windowRect.Set(ia[0], ia[1], ia[2], ia[3]);
                        }
                        catch
                        {
                            _windowRect.Set(100, 100, 400, 200);
                        }
                    }

                    if (childnode.HasValue("_scrollPosition"))
                    {
                        string s = childnode.GetValue("_scrollPosition");
                        try
                        {
                            float[] fa = Array.ConvertAll(s.Split(), float.Parse);
                            _scrollPosition.Set(fa[0], fa[1]);
                        }
                        catch
                        {
                            _scrollPosition = Vector2.zero;
                        }
                    }

                    if (childnode.HasValue("_sliderValue"))
                    {
                        string s = childnode.GetValue("_sliderValue");
                        try
                        {
                            _sliderValue = float.Parse(s);
                        }
                        catch
                        {
                            _sliderValue = 1;
                        }
                    }

                    if (childnode.HasValue("_textValue"))
                        _textValue = childnode.GetValue("_textValue");
                    else
                        _textValue = " ";

                    if (childnode.HasValue("_toggleValue"))
                        _toggleValue = Convert.ToBoolean(childnode.GetValue("_toggleValue"));
                    else
                        _toggleValue = true;

                    if (childnode.HasValue("doWindow0"))
                        doWindow0 = Convert.ToBoolean(childnode.GetValue("doWindow0"));
                    else
                        doWindow0 = true;
                }

                childnode = node.GetNode("Info");
                if (childnode != null)
                {
                    if (childnode.HasValue("_popupRect"))
                    {
                        string s = childnode.GetValue("_popupRect");
                        try
                        {
                            int[] ia = Array.ConvertAll(s.Split(), int.Parse);
                            _popupRect.Set(ia[0], ia[1], ia[2], ia[3]);
                        }
                        catch
                        {
                            _popupRect.Set(500, 500, 200, 100);
                        }
                    }

                    if (childnode.HasValue("_popupflag"))
                        _popupflag = Convert.ToBoolean(childnode.GetValue("_popupflag"));
                    else
                        _popupflag = false;
                }
            }
        }

        void save_cfg()
        {
            //           var node = ConfigNode.CreateConfigFromObject(this);
            ConfigNode node = new ConfigNode("MyFirstModConfig");

            ConfigNode childnode1 = node.AddNode("Basic_Window");

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
                //                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 
                //                                            "Info", "Hello! Button pressed.", "Ok", false, HighLogic.UISkin);
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

