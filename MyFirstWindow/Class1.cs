﻿using System;
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


        delegate void SerializeAndWrite(VesselInfo v, FileStream file);
        
        // запись данных окна в конфиг файл
        void save_cfg()
        {
            try
            {
                SerializeAndWrite swrite = (v, file) =>
                {
                    var result = IOUtils.SerializeToBinary(v);
                    file.Write(result, 0, result.Length);
                };

                if (!File.Exists<MyFirstWindow>("save.sav"))
                {
                    using (var file = File.Create<MyFirstWindow>("save.sav"))
                    {
                        swrite(vi, file);
                    }
                }
                else
                {
                    using (var file = File.Open<MyFirstWindow>("save.sav", FileMode.Open))
                    {
                        swrite(vi, file);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }

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

            using (var hs = new GUILayout.HorizontalScope("Name vessel"))
            {
                GUILayout.Label("Name vessel:", GUILayout.Width(200));
                GUILayout.TextArea(vi.name, GUILayout.Width(200));
            }

            using (var hs = new GUILayout.HorizontalScope("Altitude"))
            {

                GUILayout.Label("Altitude:", GUILayout.Width(200));
                GUILayout.TextArea(string.Format("{0,-10:f2}",vi.Altitude), GUILayout.Width(200));
            }

            using (var hs = new GUILayout.HorizontalScope("Gee force"))
            {
                GUILayout.Label("Gee force:", GUILayout.Width(200));
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.gforce), GUILayout.Width(200));
            }

            using (var hs = new GUILayout.HorizontalScope("Surface speed"))
            {
                GUILayout.Label("Surface speed:", GUILayout.Width(200));
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.surf_speed), GUILayout.Width(200));
            }

            using (var hs = new GUILayout.HorizontalScope("Vertical speed"))
            {
                GUILayout.Label("Vertical speed:", GUILayout.Width(200));
                GUILayout.TextArea(string.Format("{0,-10:f2}", vi.vert_speed), GUILayout.Width(200));
            }

            GUI.skin.label.alignment = (TextAnchor)la;

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }

    [Serializable]
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

        public override string ToString()
        {
            string s = "";
            s = name + Altitude + gforce + surf_speed + vert_speed;
            return s;
        }
    } 

}

