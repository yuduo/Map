using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using HongLi.MapControl.Util;
using HongLi.MapControl.Util.TrackSupport;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace HongLi.MapControl.Component
{
    /// <summary>
    /// 轨迹播放控件
    /// </summary>
    public partial class TrackPlayer
    {
        private Map _map;

        public TrackModel Tm { get; set; }

        public void SetMap(Map map)
        {
            _map = map;
            _map.MapView.ExtentChanged += MapView_ExtentChanged;
        }

        private void MapView_ExtentChanged(object sender, EventArgs e)
        {
            if (Visibility == Visibility.Hidden)
            {
                return;
            }
            IsPlaying = false;
            if (_storyboard == null) return;
            _storyboard.Stop();
            InitCar(_currentIndex);
        }

        public TrackPlayer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IsPlaying = false;
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                {
                    Btn.Source = new BitmapImage(new Uri("../Asset/pause.png", UriKind.Relative));
                    if (_currentIndex + 1 >= Tm.TrackPoints.Count)
                    {
                        _currentIndex = 0;
                    }
                    Play();
                }
                else
                {
                    Btn.Source = new BitmapImage(new Uri("../Asset/play.png", UriKind.Relative));
                    if (_storyboard != null)
                    {
                        _storyboard.Stop();
                        InitCar(_currentIndex);
                    }
                }

            }
        }

        private void Btn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlaying = !IsPlaying;
        }

        public async void SetModel(TrackModel tm)
        {
            _map.GetGraphicLayer("Drawing").IsCluster = false;
            _map.GetGraphicLayer("Drawing").Labeling.IsEnabled = false;

            Tm = tm;
            if (tm.TrackPoints.Count < 2)
            {
                MessageBox.Show("错误的轨迹数据");
                Visibility = Visibility.Hidden;
                return;
            }
            Slider.Minimum = 0;
            Slider.Maximum = tm.TrackPoints.Count - 1;
            Slider.TickFrequency = 1;
            Slider.Value = 0;
            _map.ClearGraphic("Drawing");
            _currentIndex = 0;
            _map.AddGraphic(tm.TrackLine, "Drawing");

            bool tag = false;
            foreach (Graphic g in tm.TrackPoints)
            {
                if (!tag)
                {
                    tag = true;
                    g.Symbol = tm.StartSymbol;
                }
                _map.AddGraphic(g, "Drawing");
            }

            GisUtil util = new GisUtil(_map);

            await util.Locate(tm.TrackLine.Geometry, 20);

        }

        private Image _car;
        private ToolTip _tip;

        private void InitCar(int currentIndex)
        {
            MapPoint pt = Tm.TrackPoints[currentIndex].Geometry as MapPoint;
            Point p = _map.MapView.LocationToScreen(pt);
            _map.AnimationCav.Children.Clear();
            _car = new Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Tm.Icon);
            var hBitmap = bmp.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            _car.Source = wpfBitmap;
            _car.Width = 24;
            _car.Height = 24;
            _map.AnimationCav.Children.Add(_car);
            Canvas.SetLeft(_car, p.X - 12);
            Canvas.SetTop(_car, p.Y - 12);


            _tip = new ToolTip();
            _map.AnimationCav.Children.Add(_tip);
            Canvas.SetLeft(_tip, p.X - 12 + 20);
            Canvas.SetTop(_tip, p.Y - 12 - 20);
            SetData(currentIndex);
        }

        public void Play()
        {
            InitCar(_currentIndex);
            if (_storyboard == null)
            {
                _storyboard = new Storyboard();
                _storyboard.Completed += PlayNext;
            }

            _storyboard.Children.Clear();
            PlayNext(this, null);
        }


        private void PlayNext(object sender, EventArgs e)
        {
            if (_currentIndex + 1 >= Tm.TrackPoints.Count)
            {
                IsPlaying = false;
                _currentIndex = 0;
                return;
            }
            _storyboard.Stop();
            Slider.Value = _currentIndex + 1;
            SetData(_currentIndex);
            MapPoint fromPoint = Tm.TrackPoints[_currentIndex].Geometry as MapPoint;
            MapPoint toPoint = Tm.TrackPoints[_currentIndex + 1].Geometry as MapPoint;
            MoveTo(fromPoint, toPoint);
            _currentIndex++;
        }

        private int _lastIndex = -1;
        private void SetData(int currentIndex)
        {
            IDictionary<string, object> attrs = Tm.TrackPoints[currentIndex].Attributes;

            if (currentIndex != _lastIndex)
            {
                _lastIndex = currentIndex;

                string str = "<Guid Type=\"GUID\">{0}</Guid>"
                + "<Label  Type = \"TEXT\" >{1}</Label>"
                + "<LON Type = \"SINGLE\" >{2}</LON>"
                + "<LAT Type = \"SINGLE\" >{3}</LAT>"
                + "<Time Type = \"DATE\" >{4}</Time>"
                + "<Speed Type = \"SINGLE\" >{5}</Speed>"
                + "<Direction Type = \"LONG\" >{6}</Direction>"
                + "<Status Type = \"TEXT\" >{7}</Status>"
                + "<Icon Type = \"TEXT\" >{8}</Icon>"
                + "<Remark Type = \"TEXT\" >{9}</Remark>"
                + "<Angle Type = \"TEXT\" >{10}</Angle> ";

                str = string.Format(str, attrs["ID"], attrs["Label"]
                    , attrs["Lon"], attrs["Lat"], attrs["Time"]
                    , attrs["Speed"], attrs["Direction"], attrs["Status"]
                    , attrs["Icon"], attrs["Remark"], attrs["Angle"]);

                Tm.Callback?.Invoke(str);
            }



            PId.Content = _tip.Tip = attrs["Label"];
            PSpeed.Content = attrs["Speed"];
            PTime.Content = attrs["Time"];
            PLat.Content = attrs["Lat"];

        }

        private Storyboard _storyboard;
        private int _currentIndex;

        /// <summary>
        /// 动画的方式播放轨迹
        /// </summary>
        /// <param name="fromPoint">起始点</param>
        /// <param name="toPoint">终止点</param>
        private void MoveTo(MapPoint fromPoint, MapPoint toPoint)
        {
            //Point p = e.GetPosition(body);  
            Point deskPoint = _map.MapView.LocationToScreen(toPoint);
            Point curPoint = _map.MapView.LocationToScreen(fromPoint);

            curPoint.X -= 12;
            curPoint.Y -= 12;
            deskPoint.X -= 12;
            deskPoint.Y -= 12;

            var s = Math.Sqrt(Math.Pow((deskPoint.X - curPoint.X), 2) + Math.Pow((deskPoint.Y - curPoint.Y), 2));

            var secNumber = s <= 0.5 ? 2 : (s / 1000) * 2000;

            //创建X轴方向动画
            DoubleAnimation doubleAnimation = new DoubleAnimation(
              //Canvas.GetLeft(_car),
                curPoint.X,
              deskPoint.X,
              new Duration(TimeSpan.FromMilliseconds(secNumber))
            );
            Storyboard.SetTarget(doubleAnimation, _car);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            _storyboard.Children.Add(doubleAnimation);
            //创建Y轴方向动画  
            doubleAnimation = new DoubleAnimation(
               //Canvas.GetTop(_car),
                curPoint.Y,
              deskPoint.Y,
              new Duration(TimeSpan.FromMilliseconds(secNumber))
            );
            Storyboard.SetTarget(doubleAnimation, _car);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            _storyboard.Children.Add(doubleAnimation);



            //创建X轴方向动画
            doubleAnimation = new DoubleAnimation(
              //Canvas.GetLeft(_tip),
                curPoint.X + 20,
              deskPoint.X + 20,
              new Duration(TimeSpan.FromMilliseconds(secNumber))
            );
            Storyboard.SetTarget(doubleAnimation, _tip);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            _storyboard.Children.Add(doubleAnimation);
            //创建Y轴方向动画  
            doubleAnimation = new DoubleAnimation(
                //Canvas.GetTop(_tip),
                curPoint.Y - 20,
              deskPoint.Y - 20,
              new Duration(TimeSpan.FromMilliseconds(secNumber))
            );
            Storyboard.SetTarget(doubleAnimation, _tip);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            _storyboard.Children.Add(doubleAnimation);

            //Canvas.SetLeft(_car, curPoint.X);
            //Canvas.SetTop(_car, curPoint.Y);
            //Canvas.SetLeft(_tip, curPoint.Y + 20);
            //Canvas.SetTop(_tip, curPoint.Y - 20);
            //动画播放  
            _storyboard.Begin();

        }

        private void Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlaying = false;
            Visibility = Visibility.Hidden;
            _map.ClearGraphic("Drawing");
            _map.AnimationCav.Children.Clear();

            Tm.Callback?.Invoke("Quit");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsPlaying)
            {
                return;
            }
            _currentIndex = (int)Slider.Value;
            InitCar(_currentIndex);
            SetData(_currentIndex);
        }
    }
}
