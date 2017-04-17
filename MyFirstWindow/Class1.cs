using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;



namespace MyFirstWindow
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class MyFirstWindow : MonoBehaviour
    {

        readonly int _windowId = 0;
        Rect _windowRect = new Rect(100, 100, 400, 200);
        Vector2 _scrollPosition;
        float _sliderValue = 1;
        string _textValue = "demo";
        bool _toggleValue = true;
        bool _doWindow0 = true;

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
            PluginConfiguration config = PluginConfiguration.CreateForType<MyFirstWindow>();

            //if (config != null)
            //    return;

            config.load();

            _windowRect = config.GetValue<Rect>("_windowRect", new Rect(100, 100, 400, 200));

            _scrollPosition = config.GetValue<Vector2>("_scrollPosition", new Vector2(0, 0));

            _sliderValue = (float)config.GetValue<double>("_sliderValue", 1);

            _textValue = config.GetValue<string>("_textValue", " ");

            _toggleValue  = config.GetValue<Boolean>("_toggleValue", true);

            _doWindow0 = config.GetValue<Boolean>("_doWindow0", true);

            _popupRect = config.GetValue<Rect>("_popupRect", new Rect(500, 500, 200, 100));

            _popupflag = config.GetValue<Boolean>("_popupflag", false);
        }

        // запись данных окна в конфиг файл
        void save_cfg()
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<MyFirstWindow>();

            config.SetValue("_windowRect", _windowRect);

            config.SetValue("_scrollPosition", _scrollPosition);

            config.SetValue("_sliderValue", (double)_sliderValue);

            config.SetValue("_textValue", _textValue);

            config.SetValue("_toggleValue", _toggleValue);

            config.SetValue("_doWindow0", _doWindow0);

            config.SetValue("_popupRect", _popupRect);

            config.SetValue("_popupflag", _popupflag);

            config.save();

        }

        void OnGUI()
        {
            // переключатель видимости окна Basic Window
            _doWindow0 = GUI.Toggle(new Rect(_windowRect.x, _windowRect.y - 30, 100, 20), _doWindow0, "Window 0");

            if (_doWindow0)
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
            GUI.DragWindow();
        }
    }
}

