﻿
using HongLi.MapControl;
using HongLi.MapControl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfMapControlBasedOnArcGIS.Core;

namespace WpfMapControlBasedOnArcGIS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer = new Timer();
        public MainWindow()
        {
            //log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            if (LogUtil.Log == null)
            {
                LogUtil.Log = new LogWriter();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Map.Init(AppDomain.CurrentDomain.BaseDirectory + "/map.json");
            Map.InitMap();

            timer.Interval = 30 * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        delegate void DelegateHandler();
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Map.Dispatcher.Invoke(new Action(delegate
            {
                string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                    + "<Target Type = \"TEXT\">Drawing</Target>"
                    + "</Document>";
                Map.SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);

                Map.Init(AppDomain.CurrentDomain.BaseDirectory + "/map.json");
                Map.InitMap();

                
                Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "006", "DrawPointCollection", xml, (string x) =>
                {
                    Console.WriteLine(x);
                });
            }));
        }

        private void TestView(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                  + "<Target Type = \"TEXT\">Drawing</Target>"
                  + "</Document>";
            Map.SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);

            Map.Init(AppDomain.CurrentDomain.BaseDirectory + "/map.json");
            Map.InitMap();

            DrawPointCollection1(sender, e);
            //xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"4d9b3024-7151-4462-9199-4732915b144d\" DataType = \"DrawPointCollection\"><LName Type = \"TEXT\">Drawing</LName><AutoZoom Type =\"TEXT\">1</AutoZoom><AutoPan Type =\"TEXT\">1</AutoPan><IsCluster Type =\"TEXT\">1</IsCluster><PData><Guid Type=\"GUID\">12163ed7-9bad-44d0-a54f-19ca90420723</Guid><Label Type = \"TEXT\">测试</Label><LON Type =\"SINGLE\">120.636597</LON><LAT Type =\"SINGLE\">31.290572</LAT><Time Type =\"DATE\">2015-05-19 11:52:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Detail Type = \"TEXT\" >qweqweqweqwe\nqweqwe</Detail><Callout><Title Type =\"TEXT\">111</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">e0af09dd-c9b0-4111-b346-f68a206d00e3</Guid><Label Type = \"TEXT\">0</Label><LON Type =\"SINGLE\">120.636868</LON><LAT Type =\"SINGLE\">31.288129</LAT><Time Type =\"DATE\">2015-05-19 11:53:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">222</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">596a7500-859b-44b5-a747-49ba0db91137</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.637385</LON><LAT Type =\"SINGLE\">31.288175</LAT><Time Type =\"DATE\">2015-05-19 11:15:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">333</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">08ce6dc0-0857-49c5-9cc2-0447934f200e</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.638077</LON><LAT Type =\"SINGLE\">31.288183</LAT><Time Type =\"DATE\">2015-05-19 11:20:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">444</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">f5da8f14-2563-4881-a5eb-be7950c2ad50</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.639765</LON><LAT Type =\"SINGLE\">31.288314</LAT><Time Type =\"DATE\">2015-05-19 11:30:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">555</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">2a086aff-9517-4e2f-85bb-415b13a27b86</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.640847</LON><LAT Type =\"SINGLE\">31.288287</LAT><Time Type =\"DATE\">2015-05-10 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">666</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">b940e89e-af26-4499-9732-648e7f22d3f2</Guid><Label Type = \"TEXT\">1222</Label><LON Type =\"SINGLE\">120.64211</LON><LAT Type =\"SINGLE\">31.288272</LAT><Time Type =\"DATE\">2015-05-19 11:40:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">777</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">aa663dd0-b83a-4260-917b-ecf7e8ece677</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.643448</LON><LAT Type =\"SINGLE\">31.288214</LAT><Time Type =\"DATE\">2015-05-19 11:45:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">888</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">14792bdf-d5ad-406b-87f7-7bcbe19107af</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.645339</LON><LAT Type =\"SINGLE\">31.288060</LAT><Time Type =\"DATE\">2015-05-19 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">999</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">14792bdf-d5ad-406b-87f7-7bcbe19107af</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.645339</LON><LAT Type =\"SINGLE\">31.288060</LAT><Time Type =\"DATE\">2015-05-19 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">999</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData></Document>";
            //Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "006", "DrawPointCollection", xml, (string x) =>
            //{
            //    Console.WriteLine(x);
            //});
        }

        private void SetExtent(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"001\" DataType = \"SetExtent\">"
                + "<LON Type = \"SINGLE\">120.650871</LON>"
                + "<LAT Type = \"SINGLE\" >31.290146</LAT>"
                + "<Level Type = \"LONG\" ></Level></Document>";
            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "SetExtent", xml, (string x) =>
            {
                //MessageBox.Show(x.ToString());
            });
        }

        private void GetMapPoint(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "GetMapPoint", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void IdentifyByClick(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"002\" DataType = \"IdentifyByClick\">"
               + "<Identifys Once=\"TRUE\"><Identify Service=\"zy\" Layer=\"sz_bus\"/></Identifys>"
               + "</Document>";

            Map.TransformDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", xml, (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void LocateObject(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"003\" DataType = \"LocateObject\">"
               + "<Service Type=\"TEXT\">zy</Service>"
               + "<Layer Type=\"TEXT\">桩号1</Layer>"
               + "<Where Type=\"TEXT\">PILENO = 'K3'</Where>"
               + "<Level Type = \"LONG\" ></Level></Document>";

            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "003", "LocateObject", xml, (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void LocateObjectAll(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"003\" DataType = \"LocateObjectAll\">"
               + "<Service Type=\"TEXT\">zy</Service>"
               + "<Fields Type=\"TEXT\">PILENO</Fields>"
               + "<Value Type=\"TEXT\">K23+130</Value>"
               + "<Level Type = \"LONG\" ></Level></Document>";

            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "003", "LocateObjectAll", xml, (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void DrawPic(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Asset\\img.png");

            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"DrawPicture\">"
                  + "<Target Type = \"TEXT\">Highlight</Target>"
                   + "<LON Type = \"SINGLE\">120.650871</LON>"
                   + "<LAT Type = \"SINGLE\" >31.290146</LAT>"
                   + "<Picture Type=\"TEXT\">" + ImageUtil.ImageToBase64(img) + "</Picture>"
                   + "</Document>";
            Map.SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "DrawPicture", xml);
        }

        private void PlayTrack(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"4d9b3024-7151-4462-9199-4732915b144d\" DataType = \"HisLocation\"><LName Type = \"TEXT\">GPS</LName><PData><CallBack  Type =\"TEXT\">1</CallBack><PageStart  Type =\"TEXT\">1</PageStart><PageEnd  Type =\"TEXT\">1</PageEnd><StartIcon  Type =\"TEXT\">0001</StartIcon><Icon  Type =\"TEXT\">0002</Icon></PData><PLocation><Guid Type=\"GUID\">12163ed7-9bad-44d0-a54f-19ca90420723</Guid> <Label  Type = \"TEXT\">0</Label><LON   Type =\"SINGLE\">120.636597</LON><LAT   Type =\"SINGLE\">31.290572</LAT><Time   Type =\"DATE\">2015-05-19 11:52:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">e0af09dd-c9b0-4111-b346-f68a206d00e3</Guid> <Label  Type = \"TEXT\">0</Label><LON   Type =\"SINGLE\">120.636597</LON><LAT   Type =\"SINGLE\">31.290572</LAT><Time   Type =\"DATE\">2015-05-19 11:53:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">596a7500-859b-44b5-a747-49ba0db91137</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.636597</LON><LAT   Type =\"SINGLE\">31.290572</LAT><Time   Type =\"DATE\">2015-05-19 11:15:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">08ce6dc0-0857-49c5-9cc2-0447934f200e</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.638077</LON><LAT   Type =\"SINGLE\">31.288183</LAT><Time   Type =\"DATE\">2015-05-19 11:20:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">f5da8f14-2563-4881-a5eb-be7950c2ad50</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.639765</LON><LAT   Type =\"SINGLE\">31.288314</LAT><Time   Type =\"DATE\">2015-05-19 11:30:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">2a086aff-9517-4e2f-85bb-415b13a27b86</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.640847</LON><LAT   Type =\"SINGLE\">31.288287</LAT><Time Type =\"DATE\">2015-05-10 11:51:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">b940e89e-af26-4499-9732-648e7f22d3f2</Guid> <Label  Type = \"TEXT\">1222</Label><LON   Type =\"SINGLE\">120.64211</LON><LAT   Type =\"SINGLE\">31.288272</LAT><Time Type =\"DATE\">2015-05-19 11:40:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">aa663dd0-b83a-4260-917b-ecf7e8ece677</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.643448</LON><LAT   Type =\"SINGLE\">31.288214</LAT><Time   Type =\"DATE\">2015-05-19 11:45:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><PLocation><Guid Type=\"GUID\">14792bdf-d5ad-406b-87f7-7bcbe19107af</Guid> <Label  Type = \"TEXT\">1</Label><LON   Type =\"SINGLE\">120.645339</LON><LAT   Type =\"SINGLE\">31.288060</LAT><Time   Type =\"DATE\">2015-05-19 11:51:00</Time> <Speed   Type = \"SINGLE\">10</Speed> <Direction   Type = \"LONG\">1</Direction> <Status   Type = \"TEXT\">1</Status> <Icon   Type = \"TEXT\">1017</Icon> <Remark    Type = \"TEXT\"></Remark> <Angle    Type = \"TEXT\">0</Angle></PLocation><LSymbol><LColor   Type =\"TEXT\">#11BB99</LColor><LStyle   Type =\"TEXT\">dash</LStyle><LWight   Type =\"TEXT\">1</LWight><Transpatecy   Type =\"TEXT\">1</Transpatecy></LSymbol></Document>";

            xml = System.IO.File.ReadAllText("D:\\1.txt", Encoding.Default);
            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "006", "HisLocation", xml, (string x) =>
            {
                Console.WriteLine(x);
            });
        }

        private void DrawPointCollection(object sender, RoutedEventArgs e)
        {

            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "006", "DrawPointCollection", xml, (string x) =>
            {
                Console.WriteLine(x);
            });
        }

        private void DrawPointCollection1(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"4d9b3024-7151-4462-9199-4732915b144d\" DataType = \"DrawPointCollection\"><LName Type = \"TEXT\">Drawing</LName><AutoZoom Type =\"TEXT\">1</AutoZoom><AutoPan Type =\"TEXT\">1</AutoPan><IsCluster Type =\"TEXT\">1</IsCluster><PData><Guid Type=\"GUID\">12163ed7-9bad-44d0-a54f-19ca90420723</Guid><Label Type = \"TEXT\">测试</Label><LON Type =\"SINGLE\">120.636597</LON><LAT Type =\"SINGLE\">31.290572</LAT><Time Type =\"DATE\">2015-05-19 11:52:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Detail Type = \"TEXT\" >qweqweqweqwe\nqweqwe</Detail><Callout><Title Type =\"TEXT\">111</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">e0af09dd-c9b0-4111-b346-f68a206d00e3</Guid><Label Type = \"TEXT\">0</Label><LON Type =\"SINGLE\">120.636868</LON><LAT Type =\"SINGLE\">31.288129</LAT><Time Type =\"DATE\">2015-05-19 11:53:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">222</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">596a7500-859b-44b5-a747-49ba0db91137</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.637385</LON><LAT Type =\"SINGLE\">31.288175</LAT><Time Type =\"DATE\">2015-05-19 11:15:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">333</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">08ce6dc0-0857-49c5-9cc2-0447934f200e</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.638077</LON><LAT Type =\"SINGLE\">31.288183</LAT><Time Type =\"DATE\">2015-05-19 11:20:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">444</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">f5da8f14-2563-4881-a5eb-be7950c2ad50</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.639765</LON><LAT Type =\"SINGLE\">31.288314</LAT><Time Type =\"DATE\">2015-05-19 11:30:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">555</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">2a086aff-9517-4e2f-85bb-415b13a27b86</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.640847</LON><LAT Type =\"SINGLE\">31.288287</LAT><Time Type =\"DATE\">2015-05-10 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">666</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">b940e89e-af26-4499-9732-648e7f22d3f2</Guid><Label Type = \"TEXT\">1222</Label><LON Type =\"SINGLE\">120.64211</LON><LAT Type =\"SINGLE\">31.288272</LAT><Time Type =\"DATE\">2015-05-19 11:40:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">777</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">aa663dd0-b83a-4260-917b-ecf7e8ece677</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.643448</LON><LAT Type =\"SINGLE\">31.288214</LAT><Time Type =\"DATE\">2015-05-19 11:45:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">888</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">14792bdf-d5ad-406b-87f7-7bcbe19107af</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.645339</LON><LAT Type =\"SINGLE\">31.288060</LAT><Time Type =\"DATE\">2015-05-19 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">999</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData><PData><Guid Type=\"GUID\">14792bdf-d5ad-406b-87f7-7bcbe19107af</Guid><Label Type = \"TEXT\">1</Label><LON Type =\"SINGLE\">120.645339</LON><LAT Type =\"SINGLE\">31.288060</LAT><Time Type =\"DATE\">2015-05-19 11:51:00</Time><Speed Type = \"SINGLE\">10</Speed><Direction Type = \"LONG\">1</Direction><Status Type = \"TEXT\">1</Status><Remark Type = \"TEXT\"/><Angle Type = \"TEXT\">0</Angle><Icon Type = \"TEXT\">0001</Icon><DetailURL Type = \"TEXT\" >http://www.baidu.com</DetailURL><IsLabel Type =\"TEXT\">0</IsLabel><Callout><Title Type =\"TEXT\">999</Title><Content Type =\"TEXT\">测试内容\n测试内容</Content><Url Type =\"TEXT\">http://www.baidu.com</Url></Callout></PData></Document>";
            Map.SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "006", "DrawPointCollection", xml, (string x) =>
            {
                Console.WriteLine(x);
            });
        }

        private void DrawMapPoint(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "010", "DrawMapPoint", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }



        private void DrawPolyline(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "011", "DrawPolyline", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void DrawPolygon(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "012", "DrawPolygon", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

        private void DrawCircle(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "013", "DrawCircle", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }


        private void ClearHighlight(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                  + "<Target Type = \"TEXT\">Highlight</Target>"
                  + "</Document>";
            Map.SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);
        }

        private void ClearDrawing(object sender, RoutedEventArgs e)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                  + "<Target Type = \"TEXT\">Drawing</Target>"
                  + "</Document>";
            Map.SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);

            Map.Init(AppDomain.CurrentDomain.BaseDirectory + "/map.json");
            Map.InitMap();
        }

        private TestForm frm = null;
        private void ShowMap(object sender, RoutedEventArgs e)
        {
            if (frm == null)
            {
                frm = new TestForm();
                frm.Closed += Frm_Closed;
            }
            frm.Show();
        }

        private void Frm_Closed(object sender, EventArgs e)
        {
            frm = null;
        }

        private void CloseMap(object sender, RoutedEventArgs e)
        {
            if (frm == null)
            {
                return;
            }
            frm.Close();

        }

        private void GetMapView(object sender, RoutedEventArgs e)
        {
            Map.GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "015", "GetMapView", (string x) =>
            {
                MessageBox.Show(x + "");
            });
        }

    }
}