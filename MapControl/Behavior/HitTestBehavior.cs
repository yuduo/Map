using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using HongLi.MapControl.Component;
using HongLi.MapControl.Util;
using HongLi.MapControl.Util.TaskSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 用于选中一个Graphic的行为
    /// </summary>
    public class HitTestBehavior : BaseBehavior, IBehavior
    {
        public bool Once { get; set; }

        public GraphicsLayer Layer { get; set; }

        public Map MapObj { get; set; }
        [StructLayout(LayoutKind.Sequential)]
        public struct InfoStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] ID;
            public InfoStruct(int id)
            {
                ID = new char[64];
            }
        }
        [DllImport("User32.DLL")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, IntPtr lParam);
        private const int WM_USER = 0x400;
        public HitTestBehavior(string id, Map map, GraphicsLayer layer, Action<string> callback) : base(id, callback)
        {
            MapObj = map;
            Layer = layer;
            Once = false;
        }

        public async void Work(object obj)
        {
            try
            {
                var args = obj as MapViewInputEventArgs;
                if (args == null) return;
                MapObj.MapView.Overlays.Items.Clear();

                MapViewInputEventArgs e = args;
                var graphics = await Layer.HitTestAsync(MapObj.MapView, e.Position, 1) as List<Graphic>;
                if (graphics == null || !graphics.Any())
                {
                    return;
                }
                var message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"" + Id + "\" >";

                foreach (var g in graphics)
                {
                    message += "<PData>";

                    message += g.Attributes["_data"];

                    message += "</PData>";
                }
                message += "</Document>";
                Callback?.Invoke(message);

                if (graphics.Count != 1)
                {
                    return;
                }

                var xd = new XmlDocument();
                xd.LoadXml(message);

                if (TaskEventArgs.GetXmlNode(xd, "//PData//Callout") == null)
                {
                    return;
                }

                var point = graphics[0].Geometry as MapPoint;
                if (point != null)
                {
                    ShowCallout(TaskEventArgs.GetString(xd, "//PData//Callout//Title")
                        , TaskEventArgs.GetString(xd, "//PData//Callout//Content"), point);
                }
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
            }
        }

        private void ShowCallout(string title, string content, MapPoint location)
        {

            IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;

            InfoStruct info_struct = new InfoStruct(1);
            info_struct.ID = string.Copy(title).ToCharArray();
            Array.Resize(ref info_struct.ID, 64);
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(info_struct));
            Marshal.StructureToPtr(info_struct, pnt, false);
            if (windowHandle == IntPtr.Zero || pnt == IntPtr.Zero)
            {
                return;
            }
            SendMessage((int)windowHandle, WM_USER + 58, 8, pnt);
            return;
            var c = new Callout
            {
                CalloutTitle = title,
                CalloutContent = content
            };
            OverlayItemsControl f = new OverlayItemsControl();
            f.Items.Add(c);

            MapObj.MapView.Overlays.Items.Add(c);
            ViewBase.SetViewOverlayAnchor(c, location);
        }

    }
}
