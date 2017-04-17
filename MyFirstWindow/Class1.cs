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


        // Загрузка сохранённых данных окна
        void load_cfg()
        {
            ConfigNode node = ConfigNode.Load(_fnamesave);

            if (node != null)
            {
                ConfigNode childnode = node.GetNode("Basic_Window");
                if (childnode != null)
                {
                    if (childnode.HasValue("_windowRect"))
                    {
                        Debug.Log("[MyFirstWindow]_____________if (childnode.HasValue(\"_windowRect\"))");

                        if (!StrKSPToRect(childnode.GetValue("_windowRect"), ref _windowRect))
                             _windowRect.Set(100, 100, 400, 200);

                        Debug.Log("****0" + _windowRect);
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
                        Debug.Log("[MyFirstWindow]_____________\nif (childnode.HasValue(\"_popupRect\"))");

                        if (!StrKSPToRect(childnode.GetValue("_popupRect"), ref _popupRect))
                            _popupRect.Set(500, 500, 200, 100);

                        Debug.Log("****0" + _popupRect);
                    }

                    if (childnode.HasValue("_popupflag"))
                        _popupflag = Convert.ToBoolean(childnode.GetValue("_popupflag"));
                    else
                        _popupflag = false;
                }
            }
        }

        delegate int GetIntFromSubstring(string Ss);
        // преобразование строки(Rect) когфиг файла KSP в Rect (int)
        Boolean StrKSPToRect(string s, ref Rect rect)
        {
            int idx, idx1;

            GetIntFromSubstring GetInt = (String Ss) =>
            {
                idx = s.IndexOf(Ss);
                if (idx == -1 && idx + Ss.Length < s.Length)
                    throw new Exception();

                idx += Ss.Length;
                idx1 = s.IndexOf(".", idx);
                if (idx1 == -1)
                    throw new Exception();

                return int.Parse(s.Substring(idx, idx1 - idx));
            };

            try
            {
                rect.x = GetInt("x:");
                rect.y = GetInt("y:");
                rect.width = GetInt("width:");
                rect.height = GetInt("height:");
                return true;
            }
            catch {
                return false;
            }
        }

        // запись данных окна в конфиг файл
        void save_cfg()
        {
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
            // переключатель видимости окна Basic Window
            doWindow0 = GUI.Toggle(new Rect(_windowRect.x, _windowRect.y - 30, 100, 20), doWindow0, "Window 0");

            if (doWindow0)
            {
                // отображение окна
                _windowRect = GUI.Window(_windowId, _windowRect, DoWindow0, "Basic Window");
                if (_windowRect.y < 30)
                    _windowRect.y = 30;
            }

            if (_popupflag)
            {
                // Отображение окна
                _popupRect = GUILayout.Window(_popupwindowId, _popupRect, DoPopupWindow, "Info");
            }

        }

        // Окно(главное) "Basic Window"
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

        // вспомогательное окно "Info" 
        void DoPopupWindow(int _windowid)
        {
            GUILayout.Label("Save and load config");
            if (GUILayout.Button("Save"))
            {
                _popupflag = false;
                save_cfg(); // записать данные окон
            }

            if (GUILayout.Button("Load"))
            {
                _popupflag = false;
                load_cfg(); // загрузить данные окон
            }

            if (GUILayout.Button("Cancel"))
            {
                _popupflag = false; // просто закрыть вспомогательное окно
            }

        }
    }
}

