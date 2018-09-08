using Esri.ArcGISRuntime.Controls;
using System;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 获取地图点
    /// </summary>
    public class GetMapPointBehavior : BaseBehavior,IBehavior
    {
        public GetMapPointBehavior(string id,Action<string> callback):base(id,callback)
        {

        }

        public void Work(object obj)
        {
            IsObsolete = true;
            var message = "";
            var args = obj as MapViewInputEventArgs;
            if(args != null)
            {
                var e = args;
                message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \""+Id+"\" DataType=\"GetMapPoint\"><LON>{0}</LON><LAT>{1}</LAT></Document>";
                message = string.Format(message, e.Location.X, e.Location.Y);
            }
            
            Callback?.Invoke(message);
           
        }
    }
}
