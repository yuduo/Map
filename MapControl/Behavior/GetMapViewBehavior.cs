using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using System;

namespace HongLi.MapControl.Behavior
{
    class GetMapViewBehavior : BaseBehavior, IBehavior
    {
        public GetMapViewBehavior(string id, Action<string> callback):base(id,callback)
        {

        }

        public void Work(object obj)
        {
            Envelope extent = obj as Envelope;
            MapPoint center=  extent.GetCenter();
            string message = "";
            if (extent !=null)
            {
                message = $"<Document TaskGuid=\"{Map.TaskGuid}\" DataGuid = \"{ Id }\" DataType=\"Region\">"+
                    $"<Center><LON Type=\"SINGLE\">{center.X}</LON><LAT Type=\"SINGLE\">{center.Y}</LAT></Center>" +
                    $"<LeftBottom><LON Type=\"SINGLE\">{extent.XMin}</LON><LAT Type=\"SINGLE\">{extent.YMin}</LAT></LeftBottom>" +
                    $"<RightTop><LON Type=\"SINGLE\">{extent.XMax}</LON><LAT Type=\"SINGLE\">{extent.YMax}</LAT></RightTop>" +
                    "</Document>";
            }

            base.Callback?.Invoke(message);

        }
    }
}
