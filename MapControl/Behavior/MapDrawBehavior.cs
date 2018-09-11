using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Linq;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 绘图行为
    /// </summary>
    public class MapDrawBehavior : BaseBehavior, IBehavior
    {
        private readonly Map _map;

        private readonly string _dataType;
        public MapDrawBehavior(Map map, string dataType, string id, Action<string> callback) : base(id, callback)
        {
            _map = map;
            _dataType = dataType;
        }

        public void Work(object obj)
        {
            IsObsolete = true;

            string message;

            DrawShape type = (DrawShape)obj;
            Symbol symbol = null;
            switch (type)
            {
                case DrawShape.Point:
                    symbol = new SimpleMarkerSymbol();
                    ((SimpleMarkerSymbol) symbol).Color = System.Windows.Media.Colors.Blue;
                    ((SimpleMarkerSymbol) symbol).Size = 15;
                    break;
                case DrawShape.Polyline:
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol) symbol).Color = System.Windows.Media.Colors.Blue;
                    ((SimpleLineSymbol) symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol) symbol).Width = 2;
                    break;
                case DrawShape.Polygon:
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol) symbol).Color = System.Windows.Media.Colors.Blue;
                    ((SimpleLineSymbol) symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol) symbol).Width = 2;
                    break;
                case DrawShape.Circle:
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol) symbol).Color = System.Windows.Media.Colors.Blue;
                    ((SimpleLineSymbol) symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol) symbol).Width = 2;
                    break;
                case DrawShape.Envelope:
                    break;
                case DrawShape.Rectangle:
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol)symbol).Color = System.Windows.Media.Colors.Blue;
                    ((SimpleLineSymbol)symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol)symbol).Width = 2;
                    break;
                case DrawShape.Freehand:
                    break;
                case DrawShape.Arrow:
                    break;
                case DrawShape.Triangle:
                    break;
                case DrawShape.Ellipse:
                    break;
                case DrawShape.LineSegment:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Geometry geo;

            if (symbol != null)
            {
                _map.Dispatcher.Invoke(
                              new Action(
                                  async delegate
                                  {
                                      if (_map.MapView.Editor.IsActive)
                                      {
                                          _map.MapView.Editor.Cancel.Execute(null);
                                      }
                                     
                                      try
                                      {
                                          geo = await _map.MapView.Editor.RequestShapeAsync(type, symbol);
                                      }
                                      catch
                                      {
                                          geo = null;
                                          return;
                                      }
                                      if (geo != null)
                                      {
                                          // create a new graphic; set the Geometry and Symbol
                                          Graphic graphic = new Graphic
                                          {
                                              Geometry = geo,
                                              Symbol = symbol
                                          };
                                          // add the graphic to the graphics layer
                                          _map.GetGraphicLayer("Drawing").Graphics.Add(graphic);
                                          message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"" + Id + "\" DataType=\"" + _dataType + "\">";

                                          if (geo is MapPoint)
                                          {
                                              MapPoint pt = geo as MapPoint;
                                              message += "<Geometry Type=\"MapPoint\"><X>" + pt.X + "</X><Y>" + pt.Y + "</Y>";
                                          }
                                          else if (geo is Polyline)
                                          {
                                              Polyline pl = geo as Polyline;
                                              message += "<Geometry Type=\"Polyline\">";
                                              foreach (ReadOnlySegmentCollection p in pl.Parts)
                                              {
                                                  message += "<Part>";
                                                  foreach (MapPoint pt in p.GetPoints())
                                                  {
                                                      message += "<X>" + pt.X + "</X><Y>" + pt.Y + "</Y>";
                                                  }
                                                  message += "</Part>";
                                              }
                                          }
                                          else if (geo is Polygon)
                                          {
                                              Polygon pg = geo as Polygon;
                                              message += "<Geometry Type=\"Polygon\">";
                                              foreach (ReadOnlySegmentCollection p in pg.Parts)
                                              {
                                                  message += "<Part>";
                                                  message = p.GetPoints().Aggregate(message, (current, pt) => current + ("<X>" + pt.X + "</X><Y>" + pt.Y + "</Y>"));
                                                  message += "</Part>";
                                              }
                                          }


                                          message += "</Geometry></Document>";
                                          Callback?.Invoke(message);
                                      }
                                      else
                                      {
                                          message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"" + Id + "\" DataType=\"" + _dataType + "\">绘制失败</Document>";

                                          Callback?.Invoke(message);
                                      }
                                  }
                              )
                          );

            }
            else
            {
                message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"" + Id + "\" DataType=\"" + _dataType + "\">异常的绘制类型</Document>";

                Callback?.Invoke(message);
            }

        }
    }
}
