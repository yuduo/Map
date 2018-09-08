using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongLi.MapControl.Behavior
{
    public class MapMeasureBehavior : BaseBehavior, IBehavior
    {
        private readonly Map _map;
        private List<MapPoint> _points;
        private List<Graphic> _tempTextGraphic;
        private double totalLength = 0;
        public MapMeasureBehavior(Map map, string id, Action<string> callback):base(id,callback)
        {
            _map = map;
        }

        /// <summary>
        /// 行为取消
        /// </summary>
        public void Cancel()
        {
            if (_map.MapView.Editor.IsActive)
            {
                _map.MapView.Editor.Cancel.Execute(null);
            }
        }

        public void Work(object obj)
        {
            int type = Convert.ToInt32(obj);
            DrawShape drawType;
            _points = null;
            _tempTextGraphic = null;
            totalLength = 0;
            Symbol symbol = null;
            switch (type)
            {
                case 0:
                    _points = new List<MapPoint>();
                    _tempTextGraphic = new List<Graphic>();
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol)symbol).Color = System.Windows.Media.Colors.Orange;
                    ((SimpleLineSymbol)symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol)symbol).Width = 2;
                    drawType = DrawShape.Polyline;
                    break;
                case 1:
                    symbol = new SimpleLineSymbol();
                    ((SimpleLineSymbol)symbol).Color = System.Windows.Media.Colors.Orange;
                    ((SimpleLineSymbol)symbol).Style = SimpleLineStyle.Dash;
                    ((SimpleLineSymbol)symbol).Width = 2;
                    drawType = DrawShape.Polygon;
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
                                          geo = await _map.MapView.Editor.RequestShapeAsync(drawType, symbol);
                                      }
                                      catch
                                      {
                                          geo = null;
                                          IsObsolete = true;
                                          //clearTextGraphic();
                                          Callback?.Invoke(null);
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
                                          MapPoint mp;
                                          string text;
                                          if (geo is Polyline)
                                          {
                                              mp = (geo as Polyline).Parts.Last().Last().EndPoint;
                                              text= distanceToStr(GeometryEngine.Length(geo)* 111000);
                                          }
                                          else
                                          {
                                              mp = (geo as Polygon).Parts.Last().Last().EndPoint;
                                              text = areaToStr(GeometryEngine.GeodesicArea(geo) * 111000 * 111000);
                                          }
                                          addTextSymbol(text, mp);
                                          _map.GetGraphicLayer("Drawing").Graphics.Add(graphic);
                                          IsObsolete = true;
                                          Callback?.Invoke(text);

                                      }
                                  }));
            }
        }

        private string distanceToStr(double distance)
        {
            if(distance>1000)
            {
                return (distance / 1000).ToString("0.00km");
            }
            else
            {
                return distance.ToString("0.m");
            }
        }

        private string areaToStr(double area)
        {
            if (area > 1000)
            {
                return (area / 1000000).ToString("0.00km^2");
            }
            else
            {
                return area.ToString("0.m^2");
            }
        }

       
        private void addTextSymbol(string text, MapPoint mp)
        {
            TextSymbol textSymbol = new TextSymbol();
            textSymbol.Text = text;
            SymbolFont font = new SymbolFont();
            font.FontSize = 16;
            textSymbol.Font = font;
            textSymbol.BorderLineColor = System.Windows.Media.Colors.White;
            textSymbol.BorderLineSize = 1;
            textSymbol.XOffset = 10;
            textSymbol.Color = System.Windows.Media.Colors.Orange;
            Graphic graphicText = new Graphic
            {
                Geometry = mp,
                Symbol = textSymbol
            };
            _tempTextGraphic.Add(graphicText);
            _map.GetGraphicLayer("Drawing").Graphics.Add(graphicText);
        }

        private void clearTextGraphic()
        {
            if (_tempTextGraphic != null)
            {
                foreach (Graphic g in _tempTextGraphic)
                {
                    _map.GetGraphicLayer("Drawing").Graphics.Remove(g);
                }
                _tempTextGraphic.Clear();
            }
        }

    }
}
