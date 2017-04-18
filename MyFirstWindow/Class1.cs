using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;



namespace MyFirstWindow
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class MyFirstWindow : MonoBehaviour
    {

        readonly int _windowId = 0;
        Rect _windowRect = new Rect(100, 100, 400, 200);

        VesselInfo vi = new VesselInfo();

        //private void Awake()
        //{
        //}

        private void OnDestroy()
        {
            save_cfg();
        }


        // Загрузка сохранённых данных окна
        void load_cfg()
        {
            //_windowRect = config.GetValue<Rect>("_windowRect", new Rect(100, 100, 400, 200));
        }

        // запись данных окна в конфиг файл
        void save_cfg()
        {
            //config.SetValue("_windowRect", _windowRect);

        }

        void OnGUI()
        {
            // отображение окна
            _windowRect = GUILayout.Window(_windowId, _windowRect, DoWindow0, "Vessel info");
            if (_windowRect.y < 30)
                _windowRect.y = 30;
        }

        // Окно(главное) "Basic Window"
        void DoWindow0(int windowID)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save"))
            {
                save_cfg();
                ScreenMessages.PostScreenMessage("Info saved", .5f, ScreenMessageStyle.LOWER_CENTER);
            }


            if (GUILayout.Button("Load"))
            {
                load_cfg();
                ScreenMessages.PostScreenMessage("Info loaded", .5f, ScreenMessageStyle.LOWER_CENTER);
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            vi.SetInfo(FlightGlobals.ActiveVessel);

            int la = (int)GUI.skin.label.alignment;

            GUI.skin.label.alignment = TextAnchor.MiddleRight;

            using (var hs = new GUILayout.HorizontalScope("scale"))
            {

                for (int i = 1; i != 10; i++)
                    GUILayout.Label(String.Format("{0}0", i), GUILayout.Width(10));
            }


            using (var hs = new GUILayout.HorizontalScope("Name vessel"))
            {
                GUILayout.Label("    Name vessel:");
                GUILayout.TextArea(vi.name);
            }

            using (var hs = new GUILayout.HorizontalScope("Altitude"))
            {

                GUILayout.Label("       Altitude:");
                GUILayout.TextArea(string.Format("{0,-10:f2}",vi.Altitude));
            }

            using (var hs = new GUILayout.HorizontalScope("Gee force"))
            {
                GUILayout.Label("      Gee force:");
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.gforce));
            }

            using (var hs = new GUILayout.HorizontalScope("Surface speed"))
            {
                GUILayout.Label("  Surface speed:");
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.surf_speed));
            }

            using (var hs = new GUILayout.HorizontalScope("Vertical speed"))
            {
                GUILayout.Label(" Vertical speed:");
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.vert_speed));
            }

            GUI.skin.label.alignment = (TextAnchor)la;

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }

    // вспомогательное окно 
    void DoPopupWindow(int _windowid)
    {
        GUILayout.Label(string.Format("width = {0,10:d}", GUILayout.);
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

internal class VesselInfo
    {
        public double Altitude { get; set; }
        public double gforce { get; set; }
        public double surf_speed { get; set; }
        public double vert_speed { get; set; }
        public string name { get; set; }

        public VesselInfo()
        {
            Altitude = gforce = surf_speed = vert_speed = 0;
            name = " ";
        }

        public void SetInfo(Vessel v)
        {
            Altitude = v.altitude;
            gforce = v.geeForce;
            surf_speed = v.srfSpeed;
            vert_speed = v.verticalSpeed;
            name = v.name;
        }

        public string ToString()
        {
            string s = "";
            s = name + Altitude + gforce + surf_speed + vert_speed;
            return s;
        }
    } 

}

