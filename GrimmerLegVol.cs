// 
// Copyright (C) 2016, Affordable Indicators <www.affordableindicators.com>.
// Affordable Indicators reserves the right to modify or overwrite this NinjaScript component with each release.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Core;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
using SharpDX.DirectWrite;


using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.IO;
using System.Globalization;


// This namespace holds indicators in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Indicators
{
	[Gui.CategoryOrder("Parameters", 1)]
	
	
	[Gui.CategoryOrder("Swings", 6)]
	[Gui.CategoryOrder("Text", 7)]
	
	[Gui.CategoryOrder("Pattern Boxes", 9)]
	[Gui.CategoryOrder("Background", 20)]
	
	
	
	[Gui.CategoryOrder("Levels (Open)", 40)]
	[Gui.CategoryOrder("Levels (Open) Global", 50)]
	[Gui.CategoryOrder("Levels (Closed)", 60)]
	
	
//	[Gui.CategoryOrder("Inside Bars", 10)]
//	[Gui.CategoryOrder("Chart Buttons", 11)]
	
	[Gui.CategoryOrder("Menu", 100)]
	
	
	
	/// <summary>
	/// Plots the open, high, low and close values from the session starting on the prior day.
	/// </summary>
	public class GrimmerLegVol : Indicator
	{
		private string ThisName = "GrimmerLegVol";
		
		private Stroke pHighlightStroke = new Stroke(Brushes.White, DashStyleHelper.Solid, 3);
		private SharpDX.Direct2D1.Brush VerticalLineHighlightDX = null;
		
		
		private int barsago = 0;
		private double price = 0;
		
		private double FinalXPixel = 0;
		private	double FinalYPixel = 0;		
		
		
		private int pLineHoverOffset = 8;

		private int currentbuttonhover3 = -10;
		private int currentbuttonhover5 = -10;
		
		private double dpiX = 0;
		private	double dpiY = 0;
		private int LaunchNumber = 0;
		
private Point MP;
		
        private int space = 5;

        SortedDictionary<double, ButtonZ> AllButtonZ = new SortedDictionary<double, ButtonZ>();
        const float fontHeight = 15f;

        private int PriceDigits = 0;
        private string PriceString = string.Empty;

        private List<double> All50Levels = new List<double>();
        private List<double> All100Levels = new List<double>();

        public class ButtonZ
        {
            string iText;
            string iName;
            int iWidth;
            bool iSwitch;
            SharpDX.RectangleF iRect;
            bool iHovered;

            public string Text { get { return iText; } set { iText = value; } }
            public string Name { get { return iName; } set { iName = value; } }
            public int Width { get { return iWidth; } set { iWidth = value; } }
            public bool Switch { get { return iSwitch; } set { iSwitch = value; } }
            public SharpDX.RectangleF Rect { get { return iRect; } set { iRect = value; } }
            public bool Hovered { get { return iHovered; } set { iHovered = value; } }

        }
		

        private SharpDX.RectangleF B2 = new SharpDX.RectangleF(0, 0, 0, 0);

        private bool InMenu;
        private bool InMenuP;
		
		
		private List<SharpDX.RectangleF> BlockTradeButtons = new List<SharpDX.RectangleF>();
		
		private List<int> BarsToHide = new List<int>();
		private bool InBlockNow = false;
		private bool InBlockNowP = false;
		
		
		private int CurrentHoveredLineBar = 0;
		private int CurrentHoveredLineBarP = 0;
		
		private double CurrentMousePrice = 0;
		
		private SharpDX.RectangleF HoverRect = new SharpDX.RectangleF(0,0,0,0);
		
			Stroke ThisStroke = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2);
			Stroke ThisStrokeH = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2);

		
		
		
		
		
		private string ThisHighsStatus = string.Empty;
		private string ThisLowsStatus = string.Empty;
		private double ThisRatioOut = 0;
		private double ThisBarsOut = 0;	
		
		private Series<double> AllRatio;
		private Series<double> Direction;
		private Series<double> AllVolume;
		private Series<string> AllDirection;
		private Series<double> AllSignals;
		private Series<double> AllCounterTrendBars;
		
		
		
		private Series<double> FinalOutput;
		private Series<double> FinalOutput2;
		private Series<double> FinalOutput3;
		private Series<double> LastSignal;
		
		private Series<string> HighStatus;
		private Series<string> LowStatus;
		
		
		
		
		
		
		
		
        private Series<double> AllPivots;
		
		private Series<double> BodyHigh;
		private Series<double> BodyLow;
		
		private Series<double> FinalHigh;
		private Series<double> FinalLow;		
		private Series<double> FinalVolume;
		private Series<double> ThisVolume;
		
		private Series<double> MainHigh;
		private Series<double> MainLow;			
		
		private Series<double> HighCount;
		private Series<double> LowCount;	
		private Series<double> HighCurrent;
		private Series<double> LowCurrent;	
		
		private Series<double> LastFinalSwing;	
	
		private Series<double> CurrentHighPrice2;
		private Series<int> CurrentHighBar2;
		private Series<double> CurrentLowPrice2;
		private Series<int> CurrentLowBar2;
		
		private Series<int> LastSig;		
		
		
		private Series<double> AllInsideBars;
		
		
		private Series<double> HighLeftGood;
		private Series<double> LowLeftGood;
		
		
	private int thisx, thisy = 0;
		
		private int B1X1, B1X2, B1Y1, B1Y2 = 0;
		private int B2X1, B2X2, B2Y1, B2Y2 = 0;
		private int B3X1, B3X2, B3Y1, B3Y2 = 0;
		private int B4X1, B4X2, B4Y1, B4Y2 = 0;
		private int B5X1, B5X2, B5Y1, B5Y2 = 0;
		
		
       	private struct Bar {   //LIST
			public int BarsAgo;
			public int XMin;
			public int XMax;
			public int YMin;
			public int YMax;
			public Bar(int barsAgo, int xMin, int xMax, int yMin, int yMax) {this.BarsAgo = barsAgo; this.XMin = xMin; this.XMax = xMax; this.YMin = yMin; this.YMax = yMax;}
		}
		
		
		 private struct Pivot {   //LIST
			public int StartBar;
			public int EndBar;
			public double Price;
			public Pivot(int startBar, int endBar, double slope) {this.StartBar = startBar; this.EndBar = endBar; this.Price = slope; }
		}
		
		private Pivot PI;
		
		SortedDictionary<int, Pivot> HighPV = new SortedDictionary<int, Pivot>();
		SortedDictionary<int, Pivot> LowPV = new SortedDictionary<int, Pivot>();	
				
				
		 private bool Permission = false;
		 
		 
		private int HoveredBar = 0;
		private List<Bar> bars;	
		
		private bool WaitingForHigh = true;
		private bool WaitingForLow = true;
		
		private Color BackUpColor, BackDnColor, BackNuColor;
		
		private double CurrentHighPrice, CurrentLowPrice, TradeHighPrice, TradeLowPrice = 0;
		
		SortedDictionary<int, double> HighP = new SortedDictionary<int, double>();
		SortedDictionary<int, double> LowP = new SortedDictionary<int, double>();
		
		SortedDictionary<int, double> HighLA = new SortedDictionary<int, double>();
		SortedDictionary<int, double> LowLA = new SortedDictionary<int, double>();
		
		SortedDictionary<int, double> DeleteLA = new SortedDictionary<int, double>();
		
		SortedDictionary<int, int> HighLF = new SortedDictionary<int, int>();
		SortedDictionary<int, int> LowLF = new SortedDictionary<int, int>();			
		
		 
		SortedDictionary<int, SharpDX.RectangleF> AllLineRects = new SortedDictionary<int, SharpDX.RectangleF>();
		 
		private int CurrentHighBar, CurrentLowBar = 0;
		private int CurrentHighBarMTF, CurrentLowBarMTF = 0;
		private double CurrentHighPriceMTF, CurrentLowPriceMTF = 0;
		
		private bool IsCurrentBar = false;
		

		
		private double PreviousHigh, PreviousLow = 0;
		private bool HigherHighs = false;
		private	bool HigherLows = false;
		
		//private ATR iATR;
		
		
		//private PeriodType AcceptableBasePeriodType = PeriodType.Tick;
		
		private int DS = 0;
		
		private DateTime Launched = new DateTime(2010, 1, 18);
		private TimeSpan LaunchTime = new TimeSpan(1, 2, 0, 30, 0);
		
		
		
		private double preval = 0;
		private double val = 0;
		
		private double prey = 0;
		private bool FirstOne = false;
		 
		 
		private NinjaTrader.Gui.Chart.ChartTab		chartTab;
		private NinjaTrader.Gui.Chart.Chart			chartWindow;
		private bool								isToolBarButtonAdded;
		private System.Windows.DependencyObject		searchObject;
		private System.Windows.Controls.TabItem		tabItem;
		private System.Windows.Controls.Menu		theMenu;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItem;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem1;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem2;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem3;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem4;				 
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem5;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem6;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem7;				 
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem8;
		private NinjaTrader.Gui.Tools.NTMenuItem	topMenuItemSubItem9;		 

		private bool ChangingThisBar = false;
	
		private int ChangeThisBar = 0;
				 
		 
		 
		protected void InsertWPFControls()
		{
			chartWindow = System.Windows.Window.GetWindow(ChartControl.Parent) as Chart;

			chartWindow.MainTabControl.SelectionChanged += MySelectionChangedHandler;

			foreach (System.Windows.DependencyObject item in chartWindow.MainMenu)
				if (System.Windows.Automation.AutomationProperties.GetAutomationId(item) == "GrimmerLegVol")
					return;

			// this is the actual object that you add to the chart windows Main Menu
			// which will act as a container for all the menu items
			theMenu = new System.Windows.Controls.Menu
			{
				// important to set the alignment, otherwise you will never see the menu populated
				VerticalAlignment			= VerticalAlignment.Top,
				VerticalContentAlignment	= VerticalAlignment.Top,

				// make sure to style as a System Menu	
				Style						= System.Windows.Application.Current.TryFindResource("SystemMenuStyle") as Style
			};

			System.Windows.Automation.AutomationProperties.SetAutomationId(theMenu, "GrimmerLegVol");

			// thanks to Jesse for these figures to use t
			System.Windows.Media.Geometry topMenuItem1Icon = System.Windows.Media.Geometry.Parse("m 70.5 173.91921 c -4.306263 -1.68968 -4.466646 -2.46776 -4.466646 -21.66921 0 -23.88964 -1.364418 -22.5 22.091646 -22.5 23.43572 0 22.08568 -1.36412 22.10832 22.33888 0.0184 19.29356 -0.19638 20.3043 -4.64473 21.85501 -2.91036 1.01455 -32.493061 0.99375 -35.08859 -0.0247 z M 21 152.25 l 0 -7.5 20.25 0 20.25 0 0 7.5 0 7.5 -20.25 0 -20.25 0 0 -7.5 z m 93.75 0 0 -7.5 42.75 0 42.75 0 0 7.5 0 7.5 -42.75 0 -42.75 0 0 -7.5 z m 15.75 -38.33079 c -4.30626 -1.68968 -4.46665 -2.46775 -4.46665 -21.66921 0 -23.889638 -1.36441 -22.5 22.09165 -22.5 23.43572 0 22.08568 -1.364116 22.10832 22.338885 0.0185 19.293555 -0.19638 20.304295 -4.64473 21.855005 -2.91036 1.01455 -32.49306 0.99375 -35.08859 -0.0247 z M 21 92.25 l 0 -7.5 50.25 0 50.25 0 0 7.5 0 7.5 -50.25 0 -50.25 0 0 -7.5 z m 153.75 0 0 -7.5 12.75 0 12.75 0 0 7.5 0 7.5 -12.75 0 -12.75 0 0 -7.5 z M 55.5 53.919211 C 51.193737 52.229528 51.033354 51.451456 51.033354 32.25 51.033354 8.3603617 49.668936 9.75 73.125 9.75 96.560723 9.75 95.210685 8.3858835 95.23332 32.088887 95.25177 51.382441 95.03694 52.393181 90.588593 53.943883 87.678232 54.95844 58.095529 54.93764 55.5 53.919211 Z M 21 32.25 l 0 -7.5 12.75 0 12.75 0 0 7.5 0 7.5 -12.75 0 -12.75 0 0 -7.5 z m 78.75 0 0 -7.5 50.25 0 50.25 0 0 7.5 0 7.5 -50.25 0 -50.25 0 0 -7.5 z");

			// this is the menu item which will appear on the chart's Main Menu
			topMenuItem = new Gui.Tools.NTMenuItem()
			{
				Header				= "Grimmer",
				Foreground			= pMenuBrush,
				//Icon				= topMenuItem1Icon,
				Margin				= new System.Windows.Thickness(0),
				Padding				= new System.Windows.Thickness(1),
				VerticalAlignment	= VerticalAlignment.Center,
				Style				= System.Windows.Application.Current.TryFindResource("MainMenuItem") as Style
			};

			if (pMenuEnabled)
			theMenu.Items.Add(topMenuItem);

			// ITEM 1
			
			topMenuItemSubItem1 = new Gui.Tools.NTMenuItem()
			{
				BorderThickness		= new System.Windows.Thickness(0),
				Header				= "Levels (Open)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem1.IsCheckable = true;
			topMenuItemSubItem1.IsChecked = pLevelsEnabled;
			
			topMenuItemSubItem1.Click += TopMenuItem1SubItem1_Click;
			topMenuItem.Items.Add(topMenuItemSubItem1);

			// ITEM 2
			
			topMenuItemSubItem2 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Levels (Closed)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem2.IsCheckable = true;
			topMenuItemSubItem2.IsChecked = pHistoricalLevelsEnabled;
			
			topMenuItemSubItem2.Click += TopMenuItem1SubItem2_Click;
			topMenuItem.Items.Add(topMenuItemSubItem2);			

			// ITEM 3
			
			topMenuItemSubItem3 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Swing Lines",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem3.IsCheckable = true;
			topMenuItemSubItem3.IsChecked = pLinesEnabled;
			
			topMenuItemSubItem3.Click += TopMenuItem1SubItem3_Click;
			topMenuItem.Items.Add(topMenuItemSubItem3);				
			
			// ITEM 4
			
			topMenuItemSubItem4 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Pattern Boxes",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem4.IsCheckable = true;
			topMenuItemSubItem4.IsChecked = pS3SE;
			
			topMenuItemSubItem4.Click += TopMenuItem1SubItem4_Click;
			topMenuItem.Items.Add(topMenuItemSubItem4);					
			
			// ITEM 5
			
			topMenuItemSubItem5 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Text (Status)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem5.IsCheckable = true;
			topMenuItemSubItem5.IsChecked = pShowStatus;
			
			topMenuItemSubItem5.Click += TopMenuItem1SubItem5_Click;
			topMenuItem.Items.Add(topMenuItemSubItem5);	
			
			// ITEM 6
			
			topMenuItemSubItem6 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Text (Ticks)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem6.IsCheckable = true;
			topMenuItemSubItem6.IsChecked = pShowTicks;
			
			topMenuItemSubItem6.Click += TopMenuItem1SubItem6_Click;
			topMenuItem.Items.Add(topMenuItemSubItem6);				
		
			// ITEM 7
			
			topMenuItemSubItem7 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Text (Volume)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem7.IsCheckable = true;
			topMenuItemSubItem7.IsChecked = pShowVolume;
			
			topMenuItemSubItem7.Click += TopMenuItem1SubItem7_Click;
			topMenuItem.Items.Add(topMenuItemSubItem7);	
			
			// ITEM 8
			
			topMenuItemSubItem8 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Text (Ratio)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem8.IsCheckable = true;
			topMenuItemSubItem8.IsChecked = pShowRatios;
			
			topMenuItemSubItem8.Click += TopMenuItem1SubItem8_Click;
			topMenuItem.Items.Add(topMenuItemSubItem8);				
					
			// ITEM 9
			
			topMenuItemSubItem9 = new Gui.Tools.NTMenuItem()
			{
				Header				= "Text (Bars)",
				Style				= System.Windows.Application.Current.TryFindResource("InstrumentMenuItem") as Style
			};
			
			topMenuItemSubItem9.IsCheckable = true;
			topMenuItemSubItem9.IsChecked = pShowBars;
			
			topMenuItemSubItem9.Click += TopMenuItem1SubItem9_Click;
			topMenuItem.Items.Add(topMenuItemSubItem9);				
					
			
			
		
		
		
			
		
		
			
			// add the menu which contains all menu items to the chart
			chartWindow.MainMenu.Add(theMenu);

			foreach (System.Windows.Controls.TabItem tab in chartWindow.MainTabControl.Items)
				if ((tab.Content as ChartTab).ChartControl == ChartControl && tab == chartWindow.MainTabControl.SelectedItem)
					topMenuItem.Visibility = Visibility.Visible;

			chartWindow.MainTabControl.SelectionChanged += MySelectionChangedHandler;
		}

		private void MySelectionChangedHandler(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count <= 0)
				return;

			tabItem = e.AddedItems[0] as System.Windows.Controls.TabItem;
			if (tabItem == null)
				return;

			chartTab = tabItem.Content as NinjaTrader.Gui.Chart.ChartTab; 
			if (chartTab != null)
				if (topMenuItem != null)
					topMenuItem.Visibility = chartTab.ChartControl == ChartControl ? Visibility.Visible : Visibility.Collapsed;
		}

		

		protected void RemoveWPFControls()
		{
			if (topMenuItemSubItem1 != null)
				topMenuItemSubItem1.Click -= TopMenuItem1SubItem1_Click;

			if (topMenuItemSubItem2 != null)
				topMenuItemSubItem2.Click -= TopMenuItem1SubItem2_Click;

			if (theMenu != null)
				chartWindow.MainMenu.Remove(theMenu);

			chartWindow.MainTabControl.SelectionChanged -= MySelectionChangedHandler;
		}

		protected void TopMenuItem1SubItem1_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pLevelsEnabled = topMenuItemSubItem1.IsChecked;

			ChartControl.InvalidateVisual();
		}

		protected void TopMenuItem1SubItem2_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pHistoricalLevelsEnabled = topMenuItemSubItem2.IsChecked;

			ChartControl.InvalidateVisual();
		}
		
		protected void TopMenuItem1SubItem3_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pLinesEnabled = topMenuItemSubItem3.IsChecked;

			ChartControl.InvalidateVisual();
		}

		protected void TopMenuItem1SubItem4_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pS3SE = topMenuItemSubItem4.IsChecked;

			ChartControl.InvalidateVisual();
		}
		
		protected void TopMenuItem1SubItem5_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pShowStatus = topMenuItemSubItem5.IsChecked;

			ChartControl.InvalidateVisual();
		}

		protected void TopMenuItem1SubItem6_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pShowTicks = topMenuItemSubItem6.IsChecked;

			ChartControl.InvalidateVisual();
		}
		 
		protected void TopMenuItem1SubItem7_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pShowVolume = topMenuItemSubItem7.IsChecked;

			ChartControl.InvalidateVisual();
		}
		
		protected void TopMenuItem1SubItem8_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pShowRatios = topMenuItemSubItem8.IsChecked;

			ChartControl.InvalidateVisual();
		}		
		
		protected void TopMenuItem1SubItem9_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pShowBars = topMenuItemSubItem9.IsChecked;

			ChartControl.InvalidateVisual();
		}		
		
		
		
			
		
	
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					= ThisName;
				Name						= ThisName;
				IsAutoScale					= false;
				IsOverlay					= true;
				IsSuspendedWhileInactive	= true;
				DrawOnPricePanel			= false;

				ArePlotsConfigurable = false;
				
				DisplayInDataBox = false;
				
				Calculate				= Calculate.OnPriceChange;

				AddPlot(new Stroke(Brushes.Red, DashStyleHelper.Solid, 2),	PlotStyle.Dot, "High Dots");
				AddPlot(new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2), PlotStyle.Dot, "Low Dots");
				
				pBrush01.Opacity = 40;
				pBrush02.Opacity = 40;
				pBrush03.Opacity = 80;
				pBrush04.Opacity = 80;
				pBrush05.Opacity = 40;
				pBrush06.Opacity = 40;
				
				
				TextFont						= new SimpleFont("Arial",11);
				
			}
			else if (State == State.Configure)
			{
				
				Permission = true;
			

				if (!Permission)
					return;
			
				//ZOrder = ChartBars.ZOrder - 7;
				
				Launched = DateTime.Now;
				
				//Plots[0].Width		= pDotSize;
				//Plots[1].Width		= pDotSize;
            
                
	
			}
			else if (State == State.Terminated)
			{
				
				if (ChartControl != null)
		                {
							ChartControl.MouseMove -= new MouseEventHandler(OnMouseMove);
		                    ChartControl.MouseDown -= new MouseButtonEventHandler(OnMouseDown);
							ChartControl.MouseUp -= new MouseButtonEventHandler(OnMouseUp);
							
							ChartControl.MouseLeave -= new MouseEventHandler(OnMouseLeave);
							
							
							ChartPanel.MouseDoubleClick -= new MouseButtonEventHandler(OnMouseDoubleClick);
							//this.ChartPanel.DragOver += new DragEventHandler(this.DragOver);
							
												
					
//							ChartControl.Dispatcher.InvokeAsync(() =>
//							{
								
//								ChartBarsSwitch2(true);
//							});		
							
							
						
		                }
						
				
			}
			else if (State == State.DataLoaded)
			{
				
				//AddButtonZ("TRADES", "ButtonOff", 40, ChartBars.Properties.PlotExecutions != ChartExecutionStyle.DoNotPlot);
				
				
				AddButtonZ("Reset Lines", "Reset Lines", 40, false);

				
				
		
				
								
				AllRatio = new Series<double>(this, MaximumBarsLookBack.Infinite);
                Direction = new Series<double>(this, MaximumBarsLookBack.Infinite);
                AllVolume = new Series<double>(this, MaximumBarsLookBack.Infinite);
				AllDirection = new Series<string>(this, MaximumBarsLookBack.Infinite);
				AllSignals = new Series<double>(this, MaximumBarsLookBack.Infinite);
				AllCounterTrendBars = new Series<double>(this, MaximumBarsLookBack.Infinite);
			
				FinalOutput = new Series<double>(this, MaximumBarsLookBack.Infinite);
                FinalOutput2 = new Series<double>(this, MaximumBarsLookBack.Infinite);
                FinalOutput3 = new Series<double>(this, MaximumBarsLookBack.Infinite);				
				LastSignal = new Series<double>(this, MaximumBarsLookBack.Infinite);	
				
				HighStatus = new Series<string>(this, MaximumBarsLookBack.Infinite);	
				LowStatus = new Series<string>(this, MaximumBarsLookBack.Infinite);	
				
				
				
				
				AllInsideBars = new Series<double>(this, MaximumBarsLookBack.Infinite);
							
				AllPivots = new Series<double>(this, MaximumBarsLookBack.Infinite);
                BodyHigh = new Series<double>(this, MaximumBarsLookBack.Infinite);
                BodyLow = new Series<double>(this, MaximumBarsLookBack.Infinite);
				FinalHigh = new Series<double>(this, MaximumBarsLookBack.Infinite);
				FinalLow = new Series<double>(this, MaximumBarsLookBack.Infinite);
				FinalVolume = new Series<double>(this, MaximumBarsLookBack.Infinite);
				ThisVolume = new Series<double>(this, MaximumBarsLookBack.Infinite);
				
				
				MainHigh = new Series<double>(this, MaximumBarsLookBack.Infinite);
				MainLow = new Series<double>(this, MaximumBarsLookBack.Infinite);
				
				HighCount = new Series<double>(this, MaximumBarsLookBack.Infinite);
				LowCount = new Series<double>(this, MaximumBarsLookBack.Infinite);
				HighCurrent = new Series<double>(this, MaximumBarsLookBack.Infinite);
				LowCurrent = new Series<double>(this, MaximumBarsLookBack.Infinite);				

				LastFinalSwing = new Series<double>(this, MaximumBarsLookBack.Infinite);	
				
				CurrentHighPrice2 = new Series<double>(this, MaximumBarsLookBack.Infinite);
				CurrentHighBar2 = new Series<int>(this, MaximumBarsLookBack.Infinite);				
				CurrentLowPrice2 = new Series<double>(this, MaximumBarsLookBack.Infinite);
				CurrentLowBar2 = new Series<int>(this, MaximumBarsLookBack.Infinite);
				
				HighLeftGood = new Series<double>(this, MaximumBarsLookBack.Infinite);
				LowLeftGood = new Series<double>(this, MaximumBarsLookBack.Infinite);				
				
				
		
				//LastSIG = new Series<int>(this, MaximumBarsLookBack.Infinite);
				
				
									//ChartPanel.Dispatcher.InvokeAsync(() =>
					//{
						if (ChartControl != null)
		                {
							ChartControl.MouseMove += new MouseEventHandler(OnMouseMove);
		                    ChartControl.MouseDown += new MouseButtonEventHandler(OnMouseDown);
							ChartControl.MouseUp += new MouseButtonEventHandler(OnMouseUp);
							
							
							//ChartPanel.MouseLeave += new System.EventHandler(this.OnMouseLeave);
							
						
							ChartControl.MouseLeave += new MouseEventHandler(OnMouseLeave);
		
							ChartPanel.MouseDoubleClick += new MouseButtonEventHandler(OnMouseDoubleClick);
							//this.ChartPanel.DragOver += new DragEventHandler(this.DragOver);
		                }
			
						
			}
			
			
			
			if (State == State.Historical)
			{
				if (pShowMenu)
				if (ChartControl != null && !isToolBarButtonAdded)
				{
					ChartControl.Dispatcher.InvokeAsync((Action)(() =>
					{
						InsertWPFControls();
					}));
				}
			}
			else if (State == State.Terminated)
			{
				if (ChartControl != null)
				{
					ChartControl.Dispatcher.InvokeAsync((Action)(() =>
					{
						RemoveWPFControls();
					}));
				}
			}
			
			
		}

		
	
			
        protected override void OnBarUpdate()
        {
			
			
			
			
			
			
			
			
			
				if (Calculate == Calculate.OnBarClose)
					IsCurrentBar = CurrentBars[0]+2 == BarsArray[0].Count;
				else
					IsCurrentBar = CurrentBars[0]+1 == BarsArray[0].Count;			
		
			
			
				if (IsCurrentBar)
				{
					LaunchTime = DateTime.Now - Launched;
					//Print(LaunchTime.Seconds + "   " + LaunchTime.Milliseconds);
				}
				
			if (CurrentBar == 0)
				return;
				
			if (BarsArray.Length == 2)
			{
				DS = 1;
				
				if (CurrentBars[1] < 0)
					return;
				
				
				
				
			}
			
			BodyHigh[0] = Math.Max(Close[0],Open[0]);
			BodyLow[0] = Math.Min(Close[0],Open[0]);
			
			if (pSwingBase == "Body")
			{
				FinalHigh[0] = BodyHigh[0];
				FinalLow[0] = BodyLow[0];
			}
			else
			{
				FinalHigh[0] = High[0];
				FinalLow[0] = Low[0];				
				
			}
				
			//pTicksMove = (int) Math.Ceiling(iATR[0]/TickSize*pATRPercent/100);

			if (DS == 0 && BarsInProgress == 0)
			{
			
				if (Close[0] > Open[0])
					Direction[0] = 1;
				else if (Close[0] < Open[0])
					Direction[0] = -1;
				else
					Direction[0] = 0;
				
				
				if (WaitingForHigh && IsCurrentBar)
				{
					
					if (HighLA.ContainsKey(CurrentHighBar))
						HighLA.Remove(CurrentHighBar);
					
				}
				if (WaitingForLow && IsCurrentBar)
				{
					
					if (LowLA.ContainsKey(CurrentLowBar))
						LowLA.Remove(CurrentLowBar);
					
					if (LowPV.ContainsKey(CurrentLowBar))
						LowPV.Remove(CurrentLowBar);	
					
				}

				if (WaitingForHigh)
				{

					if (FinalHigh[0] > CurrentHighPrice)
					{
						if (CurrentBar > 1)
						{
							Values[0].Reset(CurrentBar-CurrentHighBar);
							//Values[1].Reset(1);
							AllPivots[CurrentBar-CurrentHighBar] = 0;
							AllVolume[CurrentBar-CurrentHighBar] = 0;
							AllCounterTrendBars[CurrentBar-CurrentHighBar] = 0;
							AllRatio[CurrentBar-CurrentHighBar] = 0;
							AllDirection[CurrentBar-CurrentHighBar] = string.Empty;
							AllSignals[CurrentBar-CurrentHighBar] = 0;
						}
						
						
						//if (IsCurrentBar) 
						//	HighP.Remove(CurrentHighBar);
						

						
						CurrentHighPrice = FinalHigh[0];
						CurrentHighBar = CurrentBar;
						
						//if (IsCurrentBar && !HighP.ContainsKey(CurrentHighBar))
						//	HighP.Add(CurrentHighBar,CurrentHighPrice);
						
						
					}
					
					
					bool IsNewLow =  FinalLow[0] <= CurrentHighPrice - pTicksMove*TickSize+TickSize*0.5;
					IsNewLow = Low[0] < MIN(Low, pStrength)[1];
					
					if (CurrentHighBar != CurrentBar && IsNewLow)
					{
						
						
						//if (pDotsEnabled) 
							
							//DrawDot(CurrentBar.ToString()+"HD",true,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize,pSellColor);
					
						//if (pDotsEnabled) 
						//Values[0].Set(CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize);
						
						AllPivots[CurrentBar-CurrentHighBar] = 1;
						AllVolume[CurrentBar-CurrentHighBar] = GetVolume(CurrentBar-CurrentHighBar,CurrentHighBar-CurrentLowBar);
						AllCounterTrendBars[CurrentBar-CurrentHighBar] = GetBars(CurrentBar-CurrentHighBar,CurrentHighBar-CurrentLowBar,-1);
						AllRatio[CurrentBar-CurrentHighBar] = GetVolumeAgo(2) / AllVolume[CurrentBar-CurrentHighBar];
						AllDirection[CurrentBar-CurrentHighBar] = HighsStatus();
						
						ThisHighsStatus = HighsStatus();
						ThisLowsStatus = LowsStatus();
						ThisRatioOut = AllRatio[CurrentBar-CurrentHighBar] * -1;
						ThisBarsOut = AllCounterTrendBars[CurrentBar-CurrentHighBar];
						
						AllSignals[CurrentBar-CurrentHighBar] = 0;
						
						if (PriceTrend(ThisHighsStatus,ThisLowsStatus) == 1 || PriceTrend(ThisHighsStatus,ThisLowsStatus) == -1)
						{							
							ThisRatioOut = AllRatio[CurrentBar-CurrentHighBar];
						}
						
						if (AllRatio[CurrentBar-CurrentHighBar] >= pMinRatio)
						{
							AllSignals[CurrentBar-CurrentHighBar] = 1;
						}
						
						
//						if (pLinesEnabled) 
//							DrawLine(CurrentBar.ToString()+"HL", false, CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar],CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar], pLineColor, pSwingDashStyle, pSwingWidth);
//						else
//							DrawLine(CurrentBar.ToString()+"HL", false, CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar],CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar], Color.Transparent, pSwingDashStyle, pSwingWidth);
							
//						string t = Math.Round((High[CurrentBar-CurrentHighBar] - Low[CurrentBar-CurrentLowBar])/TickSize,0).ToString();
//						
//						if (pTextEnabled) 
//							DrawText(CurrentBar.ToString()+"HT",false,t,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize,pTextOffset,pSellColor,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
//						else
//							DrawText(CurrentBar.ToString()+"HT",false,t,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize,pTextOffset,Color.Transparent,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
							
						WaitingForLow = true;
						WaitingForHigh = false;
						
						CurrentLowPrice = FinalLow[0];
						CurrentLowBar = CurrentBar;
						

						if (!HighLA.ContainsKey(CurrentHighBar))
							HighLA.Add(CurrentHighBar,CurrentHighPrice);					
						
						
					}
					
				}
				
				if (WaitingForLow)
				{

					if (FinalLow[0] < CurrentLowPrice)
					{
						if (CurrentBar > 1)
						{
							//Values[0].Reset(CurrentBar-CurrentHighBar);
							Values[1].Reset(CurrentBar-CurrentLowBar);
							
							AllPivots[CurrentBar-CurrentLowBar] = 0;
							AllVolume[CurrentBar-CurrentLowBar] = 0;
							AllCounterTrendBars[CurrentBar-CurrentLowBar] = 0;
							AllRatio[CurrentBar-CurrentLowBar] = 0;
							AllDirection[CurrentBar-CurrentLowBar] = string.Empty;
							AllSignals[CurrentBar-CurrentLowBar] = 0;
							
						}
						
						///if (IsCurrentBar)
						//	LowP.Remove(CurrentLowBar);
						
						CurrentLowPrice = FinalLow[0];
						CurrentLowBar = CurrentBar;
						
						//if (IsCurrentBar && !LowP.ContainsKey(CurrentLowBar))
						//	LowP.Add(CurrentLowBar,CurrentLowPrice);
						
				
					}
					
					
					bool IsNewHigh =  High[0] >= CurrentLowPrice + pTicksMove*TickSize-TickSize*0.5;
					IsNewHigh = High[0] > MAX(High, pStrength)[1];
					
					//if (IsNewHigh)
					if (CurrentLowBar != CurrentBar && IsNewHigh)
					{
						//if (pDotsEnabled) 
						
						//Values[1].Set(CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize);	
							
						
						
						AllPivots[CurrentBar-CurrentLowBar] = -1;
						AllVolume[CurrentBar-CurrentLowBar] = GetVolume(CurrentBar-CurrentLowBar, CurrentLowBar-CurrentHighBar);
						AllCounterTrendBars[CurrentBar-CurrentLowBar] = GetBars(CurrentBar-CurrentLowBar, CurrentLowBar-CurrentHighBar,1);
						AllRatio[CurrentBar-CurrentLowBar] = GetVolumeAgo(2) / AllVolume[CurrentBar-CurrentLowBar];
						AllDirection[CurrentBar-CurrentLowBar] = LowsStatus();
						
						ThisHighsStatus = HighsStatus();
						ThisLowsStatus = LowsStatus();
						ThisRatioOut = AllRatio[CurrentBar-CurrentLowBar] * -1;
						ThisBarsOut = AllCounterTrendBars[CurrentBar-CurrentLowBar];
						
						AllSignals[CurrentBar-CurrentLowBar] = 0;
						
						if (PriceTrend(ThisHighsStatus,ThisLowsStatus) == 1 || PriceTrend(ThisHighsStatus,ThisLowsStatus) == -1)
						{
							ThisRatioOut = AllRatio[CurrentBar-CurrentLowBar];
						}
						
						if (AllRatio[CurrentBar-CurrentLowBar] >= pMinRatio)
						{
							AllSignals[CurrentBar-CurrentLowBar] = 1;
						}
						
						
//						Print("FINAL LOW");
//						
//						Print(AllRatio[CurrentBar-CurrentLowBar] + " RATIO");
//						Print(AllVolume[CurrentBar-CurrentLowBar] + " THIS VOL");
//						Print(GetVolumeAgo(2) + " PRE VOL");
//						Print(LowsStatus() + " STATUS");
//						Print("----------------------------");
//						
						
						
							//DrawDot(CurrentBar.ToString()+"LD",true,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize,pBuyColor);
						
//						if (pLinesEnabled)
//							DrawLine(CurrentBar.ToString()+"LL",false,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar],CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar], pLineColor, pSwingDashStyle, pSwingWidth);
//						else
//							DrawLine(CurrentBar.ToString()+"LL",false,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar],CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar], Color.Transparent, pSwingDashStyle, pSwingWidth);
							
//						string t = Math.Round((High[CurrentBar-CurrentHighBar] - Low[CurrentBar-CurrentLowBar])/TickSize,0).ToString();
//						if (pTextEnabled) 
//							DrawText(CurrentBar.ToString()+"LT",false,t,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize,-pTextOffset,pBuyColor,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
//						else
//							DrawText(CurrentBar.ToString()+"LT",false,t,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize,-pTextOffset,Color.Transparent,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
							
						WaitingForLow = false;
						WaitingForHigh = true;
						
						CurrentHighPrice = FinalHigh[0];
						CurrentHighBar = CurrentBar;
						
						
						
						PI = new Pivot(CurrentLowBar,0,CurrentLowPrice);
						
						if (!LowPV.ContainsKey(CurrentLowBar))
							LowPV.Add(CurrentLowBar,PI);	
						
							if (!LowLA.ContainsKey(CurrentLowBar))
						LowLA.Add(CurrentLowBar,CurrentLowPrice);			
						
					}				
				
					
					
				}

			
			
						
				//if (IsCurrentBar)
				{
					
					RemoveDrawObject("temp");
					RemoveDrawObject("HL");
					RemoveDrawObject("temp3H");
					RemoveDrawObject("temp3L");
					
					if (WaitingForHigh)
					{
						//if (pDotsEnabled) 
						
						
							//Values[0].Set(CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize);
						
						AllPivots[CurrentBar-CurrentHighBar] = 1;
						AllVolume[CurrentBar-CurrentHighBar] = GetVolume(CurrentBar-CurrentHighBar, CurrentHighBar-CurrentLowBar);
						AllCounterTrendBars[CurrentBar-CurrentHighBar] = GetBars(CurrentBar-CurrentHighBar, CurrentHighBar-CurrentLowBar, -1);
						AllRatio[CurrentBar-CurrentHighBar] = GetVolumeAgo(2) / AllVolume[CurrentBar-CurrentHighBar];
						AllDirection[CurrentBar-CurrentHighBar] = HighsStatus();
						
						ThisHighsStatus = HighsStatus();
						ThisLowsStatus = LowsStatus();
						ThisRatioOut = AllRatio[CurrentBar-CurrentHighBar] * -1;
						ThisBarsOut = AllCounterTrendBars[CurrentBar-CurrentHighBar];
						
						AllSignals[CurrentBar-CurrentHighBar] = 0;
						
						if (PriceTrend(ThisHighsStatus,ThisLowsStatus) == 1 || PriceTrend(ThisHighsStatus,ThisLowsStatus) == -1)
						{
							AllSignals[CurrentBar-CurrentHighBar] = 1;
							ThisRatioOut = AllRatio[CurrentBar-CurrentHighBar];
						}
						
						if (AllRatio[CurrentBar-CurrentHighBar] >= pMinRatio)
						{
							AllSignals[CurrentBar-CurrentHighBar] = 1;
						}
						
							//DrawDot("temp",true,CurrentBar-CurrentHighBar,CurrentHighPrice+pOffsetTicks*TickSize,pSellColor);
//						if (pLinesEnabled) 
//							DrawLine("HL",false,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar],CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar], pLineColor, pSwingDashStyle, pSwingWidth);
//						else
//							DrawLine("HL",false,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar],CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar], Color.Transparent, pSwingDashStyle, pSwingWidth);
							
//						string t = Math.Round((High[CurrentBar-CurrentHighBar] - Low[CurrentBar-CurrentLowBar])/TickSize,0).ToString();
//						if (pTextEnabled) 
//							DrawText("temp3H",false,t,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize,pTextOffset,pSellColor,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
//						else
//							DrawText("temp3H",false,t,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar]+pOffsetTicks*TickSize,pTextOffset,Color.Transparent,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);

						if (!HighLA.ContainsKey(CurrentHighBar))
							HighLA.Add(CurrentHighBar,CurrentHighPrice);	
						
					
					}
					if (WaitingForLow)
					{
						//if (pDotsEnabled) 
						
						
						//	Values[1].Set(CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize);
						
						
						
						
							//DrawDot("temp",true,CurrentBar-CurrentLowBar,CurrentLowPrice-pOffsetTicks*TickSize,pBuyColor);
						
						AllPivots[CurrentBar-CurrentLowBar] = -1;
						AllVolume[CurrentBar-CurrentLowBar] = GetVolume(CurrentBar-CurrentLowBar,CurrentLowBar-CurrentHighBar);
						AllCounterTrendBars[CurrentBar-CurrentLowBar] = GetBars(CurrentBar-CurrentLowBar, CurrentLowBar-CurrentHighBar, 1);
						AllRatio[CurrentBar-CurrentLowBar] = GetVolumeAgo(2) / AllVolume[CurrentBar-CurrentLowBar];
						AllDirection[CurrentBar-CurrentLowBar] = LowsStatus();
						
						ThisHighsStatus = HighsStatus();
						ThisLowsStatus = LowsStatus();
						ThisRatioOut = AllRatio[CurrentBar-CurrentLowBar] * -1;
						ThisBarsOut = AllCounterTrendBars[CurrentBar-CurrentLowBar];
						
						AllSignals[CurrentBar-CurrentLowBar] = 0;
						
						if (PriceTrend(ThisHighsStatus,ThisLowsStatus) == 1 || PriceTrend(ThisHighsStatus,ThisLowsStatus) == -1)
						{
							AllSignals[CurrentBar-CurrentLowBar] = 1;
							ThisRatioOut = AllRatio[CurrentBar-CurrentLowBar];
						}
						
						if (AllRatio[CurrentBar-CurrentLowBar] >= pMinRatio)
						{
							AllSignals[CurrentBar-CurrentLowBar] = 1;
						}
						
//						if (pLinesEnabled) 
//							DrawLine("HL",false,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar],CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar], pLineColor, pSwingDashStyle, pSwingWidth);
//						else
//							DrawLine("HL",false,CurrentBar-CurrentHighBar,High[CurrentBar-CurrentHighBar],CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar], Color.Transparent, pSwingDashStyle, pSwingWidth);
						
//						string t = Math.Round((High[CurrentBar-CurrentHighBar] - Low[CurrentBar-CurrentLowBar])/TickSize,0).ToString();
//						if (pTextEnabled) 
//							DrawText("temp3L",false,t,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize,-pTextOffset,pBuyColor,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
//						else
//							DrawText("temp3L",false,t,CurrentBar-CurrentLowBar,Low[CurrentBar-CurrentLowBar]-pOffsetTicks*TickSize,-pTextOffset,Color.Transparent,pTextFont2,StringAlignment.Center,Color.Transparent,Color.Transparent,0);
							
					
						PI = new Pivot(CurrentLowBar,0,CurrentLowPrice);
						
						if (!LowPV.ContainsKey(CurrentLowBar))
							LowPV.Add(CurrentLowBar,PI);				
						
						if (!LowLA.ContainsKey(CurrentLowBar))
							LowLA.Add(CurrentLowBar,CurrentLowPrice);	
						
					}
				
					
					
				}
				
				
			
				

					FinalOutput[0] = ThisRatioOut;
					FinalOutput2[0] = PriceTrend(ThisHighsStatus,ThisLowsStatus);
					FinalOutput3[0] = ThisBarsOut;
				
				
					if (AllSignals[0] != 0)
						LastSignal[0] = AllSignals[0];
					else
						LastSignal[0] = AllSignals[1];
					
					//Print(FinalOutput[0]);
			
					if (pBackEnabled)
					{
						
//						BackColorAllSeries[0] = Color.Empty;
//						BackColorSeries[0] = Color.Empty;

						
						
						
						
		//				if (Signal[0] == 1)
		//				{
		//					
		//						if (pColorAll)
		//							BackColorAllSeries[0] = Color.FromArgb((int)((double) pOpacityBL/100*(double)255),pColorBuyB);
		//						else
		//							BackColorSeries[0] = Color.FromArgb((int)((double) pOpacityBL/100*(double)255),pColorBuyB);	
		//					
		//				}				
		//				else if (Signal[0] == -1)
		//				{
							
						
						
						BackBrushes[0] = null;
						
						if (FinalOutput[0] >= pMinRatio && FinalOutput3[0] <= pZMaxBars)
						{
							
							
//								if (pColorAll)
//									BackColorAllSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),pColorSellB);	
//								else
//									BackColorSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),pColorSellB);
							
							if (pColorAll)
								BackBrushesAll[0] = pBrush09;
							else
								BackBrushes[0] = pBrush09;
							
						}
						
						
						
						
						// test price trend
						
//						if (FinalOutput2[0] == 1)
//						{
//								if (pColorAll)
//									BackColorAllSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),Color.Green);	
//								else
//									BackColorSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),Color.Green);
//							
//						}						
//						if (FinalOutput2[0] == -1)
//						{
//								if (pColorAll)
//									BackColorAllSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),Color.Red);	
//								else
//									BackColorSeries[0] = Color.FromArgb((int)((double) pOpacityBS/100*(double)255),Color.Red);
//							
//						}							
						
						
					}
					
					HighStatus[0] = ThisHighsStatus;
					LowStatus[0] = ThisLowsStatus;
					
					
					
				foreach (KeyValuePair<int,double> L in HighLA)
				{
					if (FinalHigh[0] > L.Value)
						DeleteLA.Add(L.Key,L.Value);
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					HighLA.Remove(L.Key);
					//if (pHistoricalLevelsEnabled) 
					HighLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();
				
				foreach (KeyValuePair<int,double> L in LowLA)
				{
					//Print(L.Value.Price);
					
					if (FinalLow[0] < L.Value)
					{
						//if (!DeleteLA.ContainsKey(L.Key))
							DeleteLA.Add(L.Key,L.Value);
						
					}
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					LowLA.Remove(L.Key);
					
					if (!LowLF.ContainsKey(L.Key))
					LowLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();				
				

			}
			
			
			
			
			
			// MTF
			
			
	
		
			
			
				
			
			
			
				foreach (KeyValuePair<int,double> L in HighLA)
				{
					
					
					if (FinalHigh[0] > L.Value)
						DeleteLA.Add(L.Key,L.Value);
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					HighLA.Remove(L.Key);
					//if (pHistoricalLevelsEnabled) 
						HighLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();
				
				foreach (KeyValuePair<int,double> L in LowLA)
				{
					if (FinalLow[0] < L.Value)
						DeleteLA.Add(L.Key,L.Value);
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					LowLA.Remove(L.Key);
					//if (pHistoricalLevelsEnabled) 
					LowLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();				
				

			
			
			
			//CurrentLowPriceMTF = 999999;
			//CurrentHighPriceMTF = 0;
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			return;
			
			
			
			
			
			
			//if (BarsArray.Length > 1)
			
			
			
		
//			if (High[0] <= High[1] && Low[0] >= Low[1])
//				BackBrushes[0] = Brushes.Navy;
			
	
				
				FinalHigh[0] = High[0];
				FinalLow[0] = Low[0];	
						
			
			if (CurrentBars[0] == 0)
			{
				MainHigh[0] = High[0];
				MainLow[0] = Low[0];	
				
				HighCount[0] = 0;
				LowCount[0] = 0;
				
				LastFinalSwing[0] = 1;
				
				
				FinalHigh[0] = High[0];
				FinalLow[0] = Low[0];	
								
				return;
				
				HighLeftGood[0] = 0;
				LowLeftGood[0] = 0;
			}
			
			
			CurrentHighPrice2[0] = CurrentHighPrice2[1];
			CurrentHighBar2[0] = CurrentHighBar2[1];
			CurrentLowPrice2[0] = CurrentLowPrice2[1];
			CurrentLowBar2[0] = CurrentLowBar2[1];
			
			MainHigh[0] = MainHigh[1];
			MainLow[0] = MainLow[1];
			LastFinalSwing[0]  = LastFinalSwing[1];
			HighCount[0]  = HighCount[1];
			LowCount[0]  = LowCount[1];
					
			HighCount[0]  = HighCount[1];
			LowCount[0]  = LowCount[1];
			
			
			
			HighLeftGood[0] = HighLeftGood[1];
			LowLeftGood[0] = LowLeftGood[1];		
			
			AllInsideBars[0] = 0;
			
			if (High[0] <= MainHigh[1] && Low[0] >= MainLow[1])
				AllInsideBars[0] = 1;
			

			if (AllInsideBars[0] == 0)
			{
					
				MainHigh[0] = High[0];
				MainLow[0] = Low[0];	
				
				
				if (LastFinalSwing[0] == 1)
				{	
			
//					AllPivots[CurrentBar-CurrentHighBar2[0]] = 0;
				
//					if (HighLA.ContainsKey(CurrentHighBar2[0]))
//						HighLA.Remove(CurrentHighBar2[0]);
			
					
				
					if (MainHigh[0] >= CurrentHighPrice2[0])
					{

						CurrentHighPrice2[0] = MainHigh[0];
						CurrentHighBar2[0] = CurrentBar;
						HighCount[0] = 0;
						
						HighLeftGood[0] = 0;
						
//						if (High[1] < High[0] && High[2] < High[0])
//							HighLeftGood[0] = 1;
			
						
					}
					
					
					if (High[0] < CurrentHighPrice2[0])
						HighCount[0] = HighCount[1] + 1;
					
					
					
				}
				
				
				if (LastFinalSwing[0] == -1)
				{			
					
//					AllPivots[CurrentBar-CurrentLowBar2[0]] = 0;
			
//					if (LowLA.ContainsKey(CurrentLowBar2[0]))
//						LowLA.Remove(CurrentLowBar2[0]);
					
				
				
					if (MainLow[0] <= CurrentLowPrice2[0])
					{
						
						
						CurrentLowPrice2[0] = MainLow[0];
						CurrentLowBar2[0] = CurrentBar;
						LowCount[0] = 0;
					}
					
					
					if (Low[0] > CurrentLowPrice2[0])
						LowCount[0] = LowCount[1] + 1;	
					
				}
								
				
			}
			
			
			//Print(HighCount[0] + "    " + LastFinalSwing[0]);
			
			BackBrushes[0] = null;
			
			
//			if (pShowSB5)
//				if (pShowSB3)
//				if (AllInsideBars[0] == 1)
//					BackBrushes[0] = pBrush10;
				
//				if (pShowSB)
//				if (pShowSB4)			
//				if (AllInsideBars[0] == 0)
//				{
					
//					if (HighCount[0] == 1 || HighCount[0] == 2)
//						BackBrushes[0] = pBrush08;
					
//					if (LowCount[0] == 1 || LowCount[0] == 2)
//						BackBrushes[0] = pBrush09;		
					
//				}
				
			
		
//			if (HighCount[0] == 2)
//				BackBrushes[0] = pBrush09;
			
//			if (LowCount[0] == 2)
//				BackBrushes[0] = pBrush08;		
			
			
			int pNumberSIGBars = 2;

				if (HighCount[0] >= pNumberSIGBars) // && HighLeftGood[0] == 1
				{
					
					
					
					LastFinalSwing[0] = -1;
					HighCount[0] = 0;
					LowCount[0] = 0;
				}
				
				if (LowCount[0] >= pNumberSIGBars)
				{
				
					
					
					LastFinalSwing[0] = 1;
					LowCount[0] = 0;
					HighCount[0] = 0;
				}
				
			

			
			
				if (LastFinalSwing[0] == 1)
				{
//					if (pShowSB)
//					BackBrushes[0] = pBrush09;
					
					
					if (LastFinalSwing[1] == -1)
					{
						//CurrentHighPrice2[0] = FinalHigh[0];
						//CurrentHighBar2[0] = CurrentBar;		
						
							int i = 0;
						int FinalBar = 0;
						double FinalPrice = 0;		
						
						do 
						{
		//					Print(i);
		//					Print(AllPivots[i]);
							if (AllInsideBars[i] == 0)
						   if (FinalHigh[i] >= FinalPrice)
						   {
							   FinalPrice = FinalHigh[i];
							   FinalBar = CurrentBar-i;
							   
							   
		//					   Print(Time[0]);
		//					   Print(FinalPrice);
		//					   Print(FinalBar);
		//					   Print("--------------------------");
							   
						   }
						   i++;
						} 
						while (AllPivots[i] != -1 && i<100 && i < CurrentBar);				
						
						
						CurrentHighPrice2[0] = FinalPrice;
						CurrentHighBar2[0] = FinalBar;
						
						
					}		
					else
					{
						AllPivots[CurrentBar-CurrentHighBar2[1]] = 0;
					
						if (HighLA.ContainsKey(CurrentHighBar2[1]))
							HighLA.Remove(CurrentHighBar2[1]);		
						
						//if (pLevelsEnabled3) RemoveDrawObject(CurrentHighBar2[1].ToString());
					
					}

					
					AllPivots[CurrentBar-CurrentHighBar2[0]] = 1;
					
					if (!HighLA.ContainsKey(CurrentHighBar2[0]))
						HighLA.Add(CurrentHighBar2[0],CurrentHighPrice2[0]);			
					
					barsago = CurrentBars[0] - CurrentHighBar2[0];
					price = CurrentHighPrice2[0];
					
					//if (pLevelsEnabled3) Draw.Ray(this, CurrentHighBar2[0].ToString(), barsago, price, barsago-1, price, true, pResistanceName);
					
					
					
				}
									
					

				if (LastFinalSwing[0] == -1)
				{
//					if (pShowSB)
//					BackBrushes[0] = pBrush08;
					
					if (LastFinalSwing[1] == 1)
					{
						//CurrentLowPrice2[0] = FinalLow[0];
						//CurrentLowBar2[0] = CurrentBar;	
						
							int i = 0;
							int FinalBar = 0;
							double FinalPrice = 999999;		
							
							do 
							{
			//					Print(i);
			//					Print(AllPivots[i]);
								
								if (AllInsideBars[i] == 0)
							   if (FinalLow[i] <= FinalPrice)
							   {
								   FinalPrice = FinalLow[i];
								   FinalBar = CurrentBar-i;
								   
								   
			//					   Print(Time[0]);
			//					   Print(FinalPrice);
			//					   Print(FinalBar);
			//					   Print("--------------------------");
								   
							   }
							   i++;
							} 
							while (AllPivots[i] != 1 && i<100 && i < CurrentBar);
							
							
							CurrentLowPrice2[0] = FinalPrice;
							CurrentLowBar2[0] = FinalBar;
							
						
						
					}
					else
					{
											
						AllPivots[CurrentBar-CurrentLowBar2[1]] = 0;
					
						if (LowLA.ContainsKey(CurrentLowBar2[1]))
							LowLA.Remove(CurrentLowBar2[1]);	
						
						//if (pLevelsEnabled3) RemoveDrawObject(CurrentLowBar2[1].ToString());
					}
					

						AllPivots[CurrentBar-CurrentLowBar2[0]] = -1;
					

					
					if (!LowLA.ContainsKey(CurrentLowBar2[0]))
						LowLA.Add(CurrentLowBar2[0],CurrentLowPrice2[0]);	
						
					barsago = CurrentBars[0] - CurrentLowBar2[0];
					price = CurrentHighPrice2[0];
					
					//if (pLevelsEnabled3) Draw.Ray(this, CurrentLowBar2[0].ToString(), barsago, price, barsago-1, price, true, pSupportName);
					
					
					
				}
				
	
				
				

				foreach (KeyValuePair<int,double> L in HighLA)
				{
					
					if (FinalHigh[0] > L.Value)
					{
						//RemoveDrawObject(L.Key.ToString());
						DeleteLA.Add(L.Key,L.Value);
					}
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					HighLA.Remove(L.Key);
					//if (pHistoricalLevelsEnabled) 
					
					if (!HighLF.ContainsKey(L.Key))
						HighLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();
				
				foreach (KeyValuePair<int,double> L in LowLA)
				{
					
					
					if (FinalLow[0] < L.Value)
					{
						//RemoveDrawObject(L.Key.ToString());
						DeleteLA.Add(L.Key,L.Value);
						
					}
				}	
				
				foreach (KeyValuePair<int,double> L in DeleteLA)
				{
					LowLA.Remove(L.Key);
					//if (pHistoricalLevelsEnabled) 
					if (!LowLF.ContainsKey(L.Key))
					LowLF.Add(L.Key,CurrentBars[0]);
				}
				
				DeleteLA.Clear();				
				
				bool IsNowRealTime = false;
				if (BarsArray[0].Count == CurrentBars[0]+1)
				{
					//Print("IS GOING REAL TIME");	
					IsNowRealTime = true;
				}
				
				//RemoveDrawObjects();
				
				if (IsNowRealTime)
				{
					
				
					//RemoveDrawObject(CurrentBars[0].ToString());
					
					//RemoveDrawObjects();
					
					foreach (KeyValuePair<int,double> L in HighLA)
					{
						
						int barsago = CurrentBars[0] - L.Key;
						double price = L.Value;
						
						//if (pLevelsEnabled3) Draw.Ray(this, L.Key.ToString(), barsago, price, barsago-1, price, true, pResistanceName);
					
					}				
					
					foreach (KeyValuePair<int,double> L in LowLA)
					{
						
						int barsago = CurrentBars[0] - L.Key;
						double price = L.Value;
						
					//	if (pLevelsEnabled3) Draw.Ray(this, L.Key.ToString(), barsago, price, barsago-1, price, true, pSupportName);
					
					}	
				}
				
				
				
				
		
		
				
//				Draw.Ray(NinjaScriptBase owner, string tag, int startBarsAgo, double startY, int endBarsAgo, double endY, Brush brush)
//Draw.Ray(NinjaScriptBase owner, string tag, bool isAutoScale, int startBarsAgo, double startY, int endBarsAgo, double endY, Brush brush, DashStyleHelper dashStyle, int width)
//Draw.Ray(NinjaScriptBase owner, string tag, DateTime startTime, double startY, DateTime endTime, double endY, Brush brush)
//Draw.Ray(NinjaScriptBase owner, string tag, DateTime startTime, double startY, DateTime endTime, double endY, Brush brush, DashStyleHelper dashStyle, int width)
//Draw.Ray(NinjaScriptBase owner, string tag, bool isAutoScale, int startBarsAgo, double startY, int endBarsAgo, double endY, Brush brush, DashStyleHelper dashStyle, int width, bool drawOnPricePanel)
//Draw.Ray(NinjaScriptBase owner, string tag, DateTime startTime, double startY, DateTime endTime, double endY, Brush brush, DashStyleHelper dashStyle, int width, bool drawOnPricePanel)
//Draw.Ray(NinjaScriptBase owner, string tag, int startBarsAgo, double startY, int endBarsAgo, double endY, bool isGlobal, string templateName)
//Draw.Ray(NinjaScriptBase owner, string tag, DateTime startTime, double startY, DateTime endTime, double endY, bool isGlobal, string templateName)
				
				
				return;
			

			
        }		
				
	
	
		private double GetBars(int startbb, int bb, int type)
		{
			double totalv = 0;
			for (int i = startbb; i < startbb+bb; i++)
			{
				if (Direction[i] == type || Direction[i] == 0)	
					totalv = totalv + 1;
			}
			
			
			return totalv;
			
		}
		
		private double GetVolume(int startbb, int bb)
		{
			double totalv = 0;
			for (int i = startbb; i < startbb+bb; i++)
				totalv = totalv + Math.Round(Volume[i] / pDisplayUnits,0);
			
			
			return totalv;
			
		}

		private string HighsStatus()
		{
			double CurrentHigh = GetHighAgo(1);
			double PreviousHigh = GetHighAgo(2);
			double pMinOffset = 0;
			
			if (CurrentHigh > RTTS(PreviousHigh + pMinOffset*TickSize))
				return "HH";
			else if (CurrentHigh < RTTS(PreviousHigh - pMinOffset*TickSize))
				return "LH";	
			else
				return "DT";
			
		}
			
		private string LowsStatus()
		{
			double CurrentLow = GetLowAgo(1);
			double PreviousLow = GetLowAgo(2);
			double pMinOffset = 0;
			
			if (CurrentLow > RTTS(PreviousLow + pMinOffset*TickSize))
				return "HL";
			else if (CurrentLow < RTTS(PreviousLow - pMinOffset*TickSize))
				return "LL";	
			else
				return "DB";
			
		}
		
		private int PriceTrend(string HighStatus, string LowStatus)
		{
			if (HighStatus == "HH" && LowStatus == "HL")
				return 1;
			else if (HighStatus == "LH" && LowStatus == "LL")
				return -1;			
			else
				return 0;			
			
		}
		
		private double GetHighAgo(int pb)
		{
			int x = 0;
			int pivots = 0;
			double finalvol = 0;
			
			do 
			{
				if (AllPivots[x] == 1)
				{
					pivots = pivots + 1;
					finalvol = 	High[x];
				}
					
				x++;
			} while (pivots < pb && x < CurrentBar - 2);
		
		
			
			return finalvol;
			
		}
		
		private double GetLowAgo(int pb)
		{
			int x = 0;
			int pivots = 0;
			double finalvol = 0;
			
			do 
			{
				if (AllPivots[x] == -1)
				{
					pivots = pivots + 1;
					finalvol = 	Low[x];
				}
					
				x++;
			} while (pivots < pb && x < CurrentBar - 2);
		
		
			
			return finalvol;
			
		}
		
		private double GetVolumeAgo(int pb)
		{
			int x = 0;
			int pivots = 0;
			double finalvol = 0;
			
			do 
			{
				if (AllVolume[x] != 0)
				{
					pivots = pivots + 1;
					finalvol = 	AllVolume[x];
				}
					
				x++;
			} while (pivots < pb && x < CurrentBar - 2);
		
		
			
			return finalvol;
			
		}
		
		
		
		
		
		private void RTTS (Series<double> I)
		{
			if (I.IsValidDataPoint(0))
				I[0] = Instrument.MasterInstrument.RoundToTickSize(I[0]);
			
			
		}			
        private double RTTS(double p)
        {
			//if (pRounding)	
            	return Instrument.MasterInstrument.RoundToTickSize(p);
			//else
			//	return p;
        }		
		
		private void SetBackColor (Brush BB)
		{
//			if (pColorAll) 
//				BackBrushesAll[0] = BB;
//			else
//				BackBrushes[0] = BB;			
		}		
		
		
		public override void OnRenderTargetChanged()
		{
			
		  // Explicitly set the Stroke RenderTarget
			
			if (RenderTarget != null)
			{

				
				pHighlightStroke.RenderTarget = RenderTarget;
				
				
				
				pBrush01.RenderTarget = RenderTarget;
				pBrush02.RenderTarget = RenderTarget;
				pBrush03.RenderTarget = RenderTarget;
				pBrush04.RenderTarget = RenderTarget;
				pBrush05.RenderTarget = RenderTarget;
				pBrush06.RenderTarget = RenderTarget;
				pBrush07.RenderTarget = RenderTarget;
				pBrush08.RenderTarget = RenderTarget;
//				pBrush09.RenderTarget = RenderTarget;
//				pBrush10.RenderTarget = RenderTarget;	
//				pBrush11.RenderTarget = RenderTarget;
//				pBrush12.RenderTarget = RenderTarget;
//				pBrush13.RenderTarget = RenderTarget;
//				pBrush14.RenderTarget = RenderTarget;
//				pBrush15.RenderTarget = RenderTarget;
//				pBrush16.RenderTarget = RenderTarget;
//				pBrush17.RenderTarget = RenderTarget;
//				pBrush18.RenderTarget = RenderTarget;
//				pBrush19.RenderTarget = RenderTarget;
//				pBrush20.RenderTarget = RenderTarget;
				
							
		
			ThisStroke.RenderTarget = RenderTarget;
			ThisStrokeH.RenderTarget = RenderTarget;
				
			}
		}
		
		protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
		{

			if (!IsVisible)
				return;
			
			ChartControlProperties myProperties = chartControl.Properties;

			
			 //Set the AllowSelectionDragging property to false
		            myProperties.AllowSelectionDragging = false;
			
			
			// Default plotting in base class. Uncomment if indicators holds at least one plot - so in this case we would expect not to see the SMA plot we have as well in this sample script
			if (pDotsEnabled)
			base.OnRender(chartControl, chartScale);

			//return;
			
			//RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;		

			if (!Permission)
				return;
			
			SharpDX.Direct2D1.AntialiasMode oldAntialiasMode	= RenderTarget.AntialiasMode;
			RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
		

			

					
					VerticalLineHighlightDX = myProperties.ChartText.ToDxBrush(RenderTarget);
					VerticalLineHighlightDX.Opacity = 60/100f;
			
			
			//Stroke ThisStroke = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2);
			//Stroke ThisStrokeH = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2);

			
	
			//return;
			
			//if (BarsInProgress == 1 || ChartControl.FirstBarPainted < 2)
			//	return;
			
			DS = 0;
			
			//int RMB = base.ChartControl.LastBarPainted;
			float x1, x2, y1, y2, y3, x3 = 0;
			
			double PreviousDir = 0;
			double PreviousPrice = 0;
			int PreviousBB = ChartBars.ToIndex;
			double ThisSize = 0;
			double ThisVolume = 0;
			double ThisRatio = 0;
			double ThisBars = 0;
			string FS = string.Empty;
			
			
			
				       
			if (dpiX == 0)
	        {
				PresentationSource source = PresentationSource.FromVisual(this.ChartPanel);
				
				if (source != null)
				{
	            dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
	            dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
				
	             dpiX = 100.0 * source.CompositionTarget.TransformToDevice.M11;
	            dpiY = 100.0 * source.CompositionTarget.TransformToDevice.M22;
				}
				
			}

				
			//Print(dpiX);
			
			FinalXPixel = MP.X / 100 * dpiX;
			FinalYPixel = MP.Y / 100 * dpiY;
			
			//Print(FinalXPixel);
			
           CurrentMousePrice = chartScale.MaxValue - chartScale.MaxMinusMin * (FinalYPixel / chartScale.Height) / dpiY * 100;

			CurrentMousePrice = RTTS(CurrentMousePrice);

			
			
//			SizeF SZ = graphics.MeasureString("", pTextFont2);
			
//			SolidBrush HighTextBrush = new SolidBrush(pBuyColor);
//			SolidBrush LowTextBrush = new SolidBrush(pSellColor);		
//			Pen LinePen = new Pen(pLineColor, pSwingWidth);
			
//			LinePen.DashStyle = pSwingDashStyle;
			
			
//			int j = ChartControl.FirstBarPainted;
//			while ((AllPivots[CurrentBar - j] != 1 && j > 0) || j == ChartControl.FirstBarPainted)
//			{
//				j = j - 1;
				
				
//			}
			
//			int k = Math.Min(CurrentBar, ChartControl.LastBarPainted);

			
//			SmoothingMode OSM = graphics.SmoothingMode;
//			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			
			//ThisStroke = pBrush01;
			//ThisStrokeH = pBrush02;
			

			int pShadowOpacity = 0;

			int pShadowWidth = 3;
			
 			Point StartPoint	= new Point(0, 0);
			Point EndPoint		= new Point(0, 0);
			Point TextPoint		= new Point(0, 0);
					
			TextFormat	textFormat2			= TextFont.ToDirectWriteTextFormat();	
			
			TextLayout textLayout2 = new TextLayout(Globals.DirectWriteFactory, "", textFormat2, 1000, textFormat2.FontSize);
			
		            textFormat2.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
		            textFormat2.ParagraphAlignment = SharpDX.DirectWrite.ParagraphAlignment.Center;
		            textFormat2.WordWrapping = SharpDX.DirectWrite.WordWrapping.NoWrap;
			
            System.Windows.Media.Brush TextBrush = ChartControl.Properties.AxisPen.Brush;
            SharpDX.Direct2D1.Brush TextBrushDX = TextBrush.ToDxBrush(RenderTarget);							
							
			TextBrushDX = pColorStoogesBrush.ToDxBrush(RenderTarget);	
			
			
			SharpDX.Direct2D1.Brush TextUpBrushDX = pColorUpBrush.ToDxBrush(RenderTarget);	
			SharpDX.Direct2D1.Brush TextDownBrushDX = pColorDownBrush.ToDxBrush(RenderTarget);	
			SharpDX.Direct2D1.Brush TextNeutralBrushDX = pColorStoogesBrush.ToDxBrush(RenderTarget);	
			
			SharpDX.Direct2D1.Brush TextFinalBrushDX = pColorDownBrush.ToDxBrush(RenderTarget);	
			
			
//			TextUpBrushDX = myProperties.ChartText.ToDxBrush(RenderTarget);
//			TextDownBrushDX = myProperties.ChartText.ToDxBrush(RenderTarget);
			
			
			double HV1 = 0;
			double HV2 = 0;
			double HV3 = 0;
			double LV1 = 0;
			double LV2 = 0;
			double LV3 = 0;
			
			double HP1 = 0;
			double HP2 = 0;
			double HP3 = 0;
			double LP1 = 0;
			double LP2 = 0;
			double LP3 = 0;
			
			
			
			
			
			
			// draw boxes
			
			int CurrentInsideBar = -1;
			int PreviousInsideBar = -1;
			int FirstBar = 0;
			int LastBar = 0;
			double ThisHigh = 0;
			double ThisLow = 9999999999;
			
			if (pShowSB5)
			if (pShowSB2)
			for (int i = Math.Max(0, ChartBars.FromIndex-200); i <= Math.Min(ChartBars.ToIndex+200,CurrentBars[0]); i++)
			{
				
//				int BB = Math.Max(0,Math.Min(CurrentBar,CurrentBar - i));
				
//				//Print(BB + "   " + CurrentBar);
				
//				if (PreviousDir != 0 && Volume.IsValidDataPointAt(i))
//				ThisVolume = ThisVolume + Math.Round(Volume.GetValueAt(i) / pDisplayUnits,0);
				
				
				CurrentInsideBar = (int) AllInsideBars.GetValueAt(i);
				
				if (CurrentInsideBar != PreviousInsideBar && CurrentInsideBar == 1)
				{
					FirstBar = i;
					
				}
				
				if (CurrentInsideBar == 1)
				{
					ThisHigh = Math.Max(FinalHigh.GetValueAt(i), ThisHigh);
					ThisLow = Math.Min(FinalLow.GetValueAt(i), ThisLow);
				}
				
				if (CurrentInsideBar != PreviousInsideBar && CurrentInsideBar == 0)
				{
					LastBar = i-1;
					
				}
				
				
				if (LastBar != -1)
				{
					

					
					
					int barw = 10;
					barw = (int) chartControl.BarWidth;
					
					x1 = ChartControl.GetXByBarIndex(ChartBars,FirstBar) - barw - 2;
					x2 = ChartControl.GetXByBarIndex(ChartBars,LastBar) + barw + 0;		
					x3 = x2 - x1;
					
					y1 = chartScale.GetYByValue(ThisHigh)-1;
					y2 = chartScale.GetYByValue(ThisLow);
					y3 = y2 - y1;
					
					
					SharpDX.RectangleF			rect2			= new SharpDX.RectangleF(x1, y1, x3, y3);
					
					int eeeee = pExpandRectangle;
					
					RenderTarget.FillRectangle(ExpandRect(rect2,eeeee,eeeee,eeeee,eeeee), pBrush11.ToDxBrush(RenderTarget));
					
					LastBar = -1;
					ThisHigh = 0;
					ThisLow = 9999999999;					
				}
				
				PreviousInsideBar = CurrentInsideBar;
				
//				if (AllInsideBars.GetValueAt(i) != 0)
//				{
					
//					//Print(Time.GetValueAt(i));
					
//					x1 = ChartControl.GetXByBarIndex(ChartBars,i);
//					x2 = ChartControl.GetXByBarIndex(ChartBars,PreviousBB);
					
//					bool ddo = true;
					
//					if (Math.Max(CurrentHighBar2.GetValueAt(CurrentBar), CurrentLowBar2.GetValueAt(CurrentBar)) == i) // don't show currently forming pivot
//						ddo = false;
//					//
					
//					if (ddo)
//					if (PreviousPrice != 0)
//					{
						
						
//						if (AllPivots.GetValueAt(i) == 1)
//						{
							
//							HV3 = HV2;
//							HV2 = HV1;
//							HV1 = FinalVolume.GetValueAt(i);
							
//							HP3 = HP2;
//							HP2 = HP1;
//							HP1 = FinalHigh.GetValueAt(i);							
							
							
//							ThisStroke = pBrush01;
//							//ThisStroke.BrushDX.Opacity = (float) pOpacity01/100;		
							
//							ThisStrokeH.Brush = ThisStroke.Brush;
//							ThisStrokeH.Brush = TextBrush;
//							ThisStrokeH.BrushDX.Opacity = 0.2f;
							

							
//							y1 = chartScale.GetYByValue(FinalHigh.GetValueAt(i));
//							y2 = chartScale.GetYByValue(PreviousPrice);
							
//							StartPoint	= new Point(x1,y1);
//							EndPoint = new Point(x2,y2);

							
							
								
//								SharpDX.RectangleF			rect2			= new SharpDX.RectangleF(newx, newy, RectWidth, RectHeight);

								
//								//Print(HV1 + "  " + HV2 + "  " + HV3);
////								bool ISVOLUMEOK = HV1 < HV2 && HV2 < HV3 && HV3 != 0 && HV2 != 0 && pS3SE && pS3SEText;
////								bool ISPRICEOK = HP1 < HP2 && HP2 < HP3 && HP3 != 0 && HP2 != 0 && pS3SE && pS3SEBox && ISVOLUMEOK;
								
////								//ISPRICEOK = true;
								
////								if (ISPRICEOK)
////								{
								
////									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;
////									RenderTarget.DrawRectangle(rect2, pBrush08.BrushDX, pBrush08.Width, pBrush08.StrokeStyle);
////									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
									
////								}

								
//						}
						
//						//if (ddo)
//						if (AllPivots.GetValueAt(i) == -1)
//						{
							
							
					
//						}						
						
						
						
//					}
				
//					//Print(BB + "   " + CurrentBar);
					
//					PreviousBB = i;
//					PreviousDir = AllPivots.GetValueAt(i);
//					if (PreviousDir == 1)
//						PreviousPrice = FinalHigh.GetValueAt(i);
//					if (PreviousDir == -1)
//						PreviousPrice = FinalLow.GetValueAt(i);
//				}
				
				
			}
			
			
			
			
			
			

			// draw levels
			
						
			RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;
			
			CurrentHoveredLineBar = 0;
			AllLineRects.Clear();
			BlockTradeButtons.Clear();
			
			int iii = 0;
			
			//if (!pLevelsEnabled3)
			if (pLevelsEnabled) 
			foreach(KeyValuePair<int,double> L in LowLA)
			{
				//Print(L.Key);
				
				iii = iii + 1;
				
				if (iii == LowLA.Count && LastFinalSwing.GetValueAt(CurrentBar) == -1)
					continue;
				
				if (BarsToHide.Contains(L.Key))
					continue;

				x1 = ChartControl.GetXByBarIndex(ChartBars,L.Key);
				x2=5000;
				
				y1 = chartScale.GetYByValue(L.Value);
				y2 = y1;
				
				ThisStroke = pBrush03;
				//ThisStroke.BrushDX.Opacity = pOpacity03;
				
				StartPoint	= new Point(x1,y1);
				EndPoint = new Point(x2,y2);
				
				// Hover and Save current hovered line
				
//				HoverRect = new SharpDX.RectangleF(x1,y1-1,x2,0);
//				BlockTradeButtons.Add(HoverRect);

//				if (MouseIn(HoverRect,pLineHoverOffset,pLineHoverOffset))
//				{
//					CurrentHoveredLineBar = L.Key;
					
//					if (!IsInHitTest)
//					RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), VerticalLineHighlightDX, ThisStroke.Width+2, pHighlightStroke.StrokeStyle);
					
//				}
						
				
			
				if (!IsInHitTest)
				RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
				
				
			}	
			
			iii = 0;
			
			if (!pLevelsEnabled3)
			if (pLevelsEnabled) 			
			foreach(KeyValuePair<int,double> H in HighLA)
			{
				
				
				iii = iii + 1;
				
				if (iii == HighLA.Count && LastFinalSwing.GetValueAt(CurrentBar) == 1)
					continue;
				
				
				if (BarsToHide.Contains(H.Key))
					continue;
				
				
				x1 = ChartControl.GetXByBarIndex(ChartBars,H.Key);
				x2=5000;
				
				y1 = chartScale.GetYByValue(H.Value);
				y2 = y1;

				ThisStroke = pBrush04;
				//ThisStroke.BrushDX.Opacity = pOpacity04;
				
				StartPoint	= new Point(x1,y1);
				EndPoint = new Point(x2,y2);
				
				// Hover and Save current hovered line
				
//				HoverRect = new SharpDX.RectangleF(x1,y1-1,x2,0);
//				BlockTradeButtons.Add(HoverRect);

//				if (MouseIn(HoverRect,pLineHoverOffset,pLineHoverOffset))
//				{
//					CurrentHoveredLineBar = H.Key;
					
//					if (!IsInHitTest)
//					RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), VerticalLineHighlightDX, ThisStroke.Width+2, pHighlightStroke.StrokeStyle);
					
//				}
				
				if (!IsInHitTest)
				RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
						
			}	
			
			if (pHistoricalLevelsEnabled)
			foreach(KeyValuePair<int,int> L in LowLF)
			{
				
				if (BarsToHide.Contains(L.Key))
					continue;
				
				x1 = ChartControl.GetXByBarIndex(ChartBars,L.Key);
				x2 = ChartControl.GetXByBarIndex(ChartBars,L.Value);
				
				y1 = chartScale.GetYByValue(Low.GetValueAt(L.Key));
				y2 = y1;

				ThisStroke = pBrush05;
				//ThisStroke.BrushDX.Opacity = pOpacity05;
				
				StartPoint	= new Point(x1,y1);
				EndPoint = new Point(x2,y2);
				
				// Hover and Save current hovered line
				
//				HoverRect = new SharpDX.RectangleF(x1,y1-1,x2-x1,0);
//				BlockTradeButtons.Add(HoverRect);

//				if (MouseIn(HoverRect,pLineHoverOffset,pLineHoverOffset))
//				{
//					CurrentHoveredLineBar = L.Key;
					
//					if (!IsInHitTest)
//					RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), VerticalLineHighlightDX, ThisStroke.Width+2, pHighlightStroke.StrokeStyle);
					
//				}
				
				if (!IsInHitTest)
				RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
						
				
			}
			
			if (pHistoricalLevelsEnabled)			
			foreach(KeyValuePair<int,int> H in HighLF)
			{
				
				if (BarsToHide.Contains(H.Key))
					continue;
				
				x1 = ChartControl.GetXByBarIndex(ChartBars,H.Key);
				x2 = ChartControl.GetXByBarIndex(ChartBars,H.Value);
				
				y1 = chartScale.GetYByValue(High.GetValueAt(H.Key));
				y2 = y1;

				ThisStroke = pBrush06;
				//ThisStroke.BrushDX.Opacity = pOpacity06;
				
				StartPoint	= new Point(x1,y1);
				EndPoint = new Point(x2,y2);
				
				// Hover and Save current hovered line
				
//				HoverRect = new SharpDX.RectangleF(x1,y1-1,x2-x1,0);
//				BlockTradeButtons.Add(HoverRect);

//				if (MouseIn(HoverRect,pLineHoverOffset,pLineHoverOffset))
//				{
//					CurrentHoveredLineBar = H.Key;
					
//					if (!IsInHitTest)
//					RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), VerticalLineHighlightDX, ThisStroke.Width+2, pHighlightStroke.StrokeStyle);
					
//				}
				
				
				if (!IsInHitTest)
				RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
						
				
			}				
				
			
			RenderTarget.AntialiasMode = oldAntialiasMode;
			
			// end levels
			
			
			for (int i = Math.Max(0, ChartBars.FromIndex-200); i <= ChartBars.ToIndex; i++)
			{
				
				int BB = Math.Max(0,Math.Min(CurrentBar,CurrentBar - i));
				
				//Print(BB + "   " + CurrentBar);
				
				if (PreviousDir != 0 && Volume.IsValidDataPointAt(i))
				ThisVolume = ThisVolume + Math.Round(Volume.GetValueAt(i) / pDisplayUnits,0);
				
				
				
				
				
				if (AllPivots.GetValueAt(i) != 0)
				{
					
					//Print(Time.GetValueAt(i));
					
					x1 = ChartControl.GetXByBarIndex(ChartBars,i);
					x2 = ChartControl.GetXByBarIndex(ChartBars,PreviousBB);
					
					bool ddo = true;
					
					if (Math.Max(CurrentHighBar2.GetValueAt(CurrentBar), CurrentLowBar2.GetValueAt(CurrentBar)) == i) // don't show currently forming pivot
						ddo = false;
					//
					
					if (ddo)
					if (PreviousPrice != 0)
					{
						
						
						if (AllPivots.GetValueAt(i) == 1)
						{
							
							HV3 = HV2;
							HV2 = HV1;
							HV1 = AllVolume.GetValueAt(i);
							
							HP3 = HP2;
							HP2 = HP1;
							HP1 = FinalHigh.GetValueAt(i);							
							
							
							ThisStroke = pBrush01;
							//ThisStroke.BrushDX.Opacity = (float) pOpacity01/100;		
							
							ThisStrokeH.Brush = ThisStroke.Brush;
							ThisStrokeH.Brush = TextBrush;
							ThisStrokeH.BrushDX.Opacity = 0.2f;
							

							
							y1 = chartScale.GetYByValue(FinalHigh.GetValueAt(i));
							y2 = chartScale.GetYByValue(PreviousPrice);
							
							StartPoint	= new Point(x1,y1);
							EndPoint = new Point(x2,y2);

							if (!IsInHitTest && pLinesEnabled)
							{
																
								if (pShadowOpacity != 0)
								RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStrokeH.BrushDX, ThisStroke.Width + pShadowWidth*2, ThisStrokeH.StrokeStyle);

								
								RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
							
							}
							
							if (pShowTicks || pShowVolume || pShowRatios || pShowStatus || pShowBars)
							//if (pShowTicks || pShowVolume)
							{
								ThisSize = FinalHigh.GetValueAt(i) - PreviousPrice;	
								ThisRatio = Math.Round(AllRatio.GetValueAt(i),pRatioRound);
								ThisBars = AllCounterTrendBars.GetValueAt(i);
								ThisVolume = AllVolume.GetValueAt(i);
								
								
								FS = string.Empty;

								if (pShowStatus)
								{
									if (FS == string.Empty)
										FS = AllDirection.GetValueAt(i).ToString();
									else
										FS = FS + " / " + AllDirection.GetValueAt(i).ToString();
								}
								
								if (pShowBars)
								{
											if (FS == string.Empty)
										FS = ThisBars.ToString();
									else
										FS = FS + " / " + ThisBars.ToString();	
								}
								
								
								
																
								if (pShowTicks)
								{
									if (FS == string.Empty)
										FS = Math.Round(ThisSize/TickSize,0).ToString();
									else
										FS = FS + " / " + Math.Round(ThisSize/TickSize,0).ToString();
									
														
									
									
								}

								if (pShowVolume)
								{
									if (FS == string.Empty)
										FS = ThisVolume.ToString();
									else
										FS = FS + " / " + ThisVolume.ToString();									
								}
								
								if (pShowRatios)
								{
									if (FS == string.Empty)
										FS = ThisRatio.ToString("n" + pRatioRound);
									else
										FS = FS + " / " + ThisRatio.ToString("n" + pRatioRound);
								}
								
								
								
								
								
//								if (pShowTicks && pShowVolume)
//									FS = Math.Round(ThisSize/TickSize,0).ToString() + " / " + HV1.ToString();								
//								else if (pShowTicks)
//									FS = Math.Round(ThisSize/TickSize,0).ToString();
//								else if (pShowVolume)
//									FS = HV1.ToString();
								
//								FS = "SH";
								
									//FS = ThisVolume.ToString();// + "  " + FinalVolume.GetValueAt(i).ToString();									
					
								textLayout2 = new TextLayout(Globals.DirectWriteFactory, FS, textFormat2, 1000, 1000);

								float boxpadding = textFormat2.FontSize;
								
		           
								float RectWidth = textLayout2.Metrics.Width + (float) TextFont.Size;
								float RectHeight = textLayout2.Metrics.Height  + (float) TextFont.Size / 2f + 1;
								
								
								
								float newy = y1 - RectHeight - 1 - pTextOffset;
								float newx = x1 - RectWidth/2;
							
								TextPoint = new Point(newx, newy);
							
								
								
								
								SharpDX.RectangleF			rect2			= new SharpDX.RectangleF(newx, newy, RectWidth, RectHeight);

								
								//Print(HV1 + "  " + HV2 + "  " + HV3);
								bool ISVOLUMEOK = HV1 < HV2 && HV2 < HV3 && HV3 != 0 && HV2 != 0 && pS3SE && pS3SEText;
								bool ISPRICEOK = HP1 < HP2 && HP2 < HP3 && HP3 != 0 && HP2 != 0 && pS3SE && pS3SEBox && ISVOLUMEOK;
								
								//ISPRICEOK = true;
								
								ISPRICEOK = AllSignals.GetValueAt(i) != 0 && pS3SE;
								
								if (ISPRICEOK)
								{
								
									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;
									RenderTarget.DrawRectangle(rect2, pBrush08.BrushDX, pBrush08.Width, pBrush08.StrokeStyle);
									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
									
								}
								
								
								
//								if (ISVOLUMEOK)
//									RenderTarget.DrawText(FS, textFormat2, rect2, TextBrushDX);
//								else
									
									
								TextFinalBrushDX = TextUpBrushDX;
								
									if (AllDirection.GetValueAt(i) != "HH" && pThisColorMode == "Status")
										TextFinalBrushDX = TextDownBrushDX;
									
									if (AllDirection.GetValueAt(i) == "DT" && pThisColorMode == "Status")
										TextFinalBrushDX = TextNeutralBrushDX;
									
									
									
									RenderTarget.DrawText(FS, textFormat2, rect2, TextFinalBrushDX);
								
								

								
								ThisVolume = 0;
							}
						}
						
						//if (ddo)
						if (AllPivots.GetValueAt(i) == -1)
						{
							
							LV3 = LV2;
							LV2 = LV1;
							LV1 = AllVolume.GetValueAt(i);
							
							LP3 = LP2;
							LP2 = LP1;
							LP1 = FinalLow.GetValueAt(i);							
														
							ThisStroke = pBrush02;
							
							//ThisStroke.BrushDX.Opacity = (float) pOpacity02/100;		
							
							ThisStrokeH.Brush = ThisStroke.Brush;
							ThisStrokeH.Brush = TextBrush;
							ThisStrokeH.BrushDX.Opacity = (float) pShadowOpacity/100;
							
							y1 = chartScale.GetYByValue(FinalLow.GetValueAt(i));
							y2 = chartScale.GetYByValue(PreviousPrice);
							
							StartPoint	= new Point(x1,y1);
							EndPoint = new Point(x2,y2);

							if (!IsInHitTest && pLinesEnabled)
							{
								
								if (pShadowOpacity != 0)
								RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStrokeH.BrushDX, ThisStroke.Width + pShadowWidth*2, ThisStroke.StrokeStyle);
								RenderTarget.DrawLine(StartPoint.ToVector2(), EndPoint.ToVector2(), ThisStroke.BrushDX, ThisStroke.Width, ThisStroke.StrokeStyle);
							}
				
											
							
							//if (!InHitTest && pLinesEnabled) graphics.DrawLine(LinePen,x1,y1,x2,y2);
							
							
							
								if (pShowTicks || pShowVolume || pShowRatios || pShowStatus || pShowBars)
							//if (pShowTicks || pShowVolume)
							{
								ThisSize = PreviousPrice - FinalLow.GetValueAt(i);	
								ThisRatio = Math.Round(AllRatio.GetValueAt(i),pRatioRound);
								ThisBars = AllCounterTrendBars.GetValueAt(i);
								ThisVolume = AllVolume.GetValueAt(i);
								
								
								FS = string.Empty;

								if (pShowStatus)
								{
									if (FS == string.Empty)
										FS = AllDirection.GetValueAt(i).ToString();
									else
										FS = FS + " / " + AllDirection.GetValueAt(i).ToString();
								}
								
								if (pShowBars)
								{
											if (FS == string.Empty)
										FS = ThisBars.ToString();
									else
										FS = FS + " / " + ThisBars.ToString();	
								}
								
								
								
																
								if (pShowTicks)
								{
									if (FS == string.Empty)
										FS = Math.Round(ThisSize/TickSize,0).ToString();
									else
										FS = FS + " / " + Math.Round(ThisSize/TickSize,0).ToString();
									
														
									
									
								}

								if (pShowVolume)
								{
									if (FS == string.Empty)
										FS = ThisVolume.ToString();
									else
										FS = FS + " / " + ThisVolume.ToString();									
								}
								
								if (pShowRatios)
								{
									if (FS == string.Empty)
										FS = ThisRatio.ToString("n" + pRatioRound);
									else
										FS = FS + " / " + ThisRatio.ToString("n" + pRatioRound);
								}
								
								
								
								
								
								
								//FS = "SL";
								
									//FS = ThisVolume.ToString();// + "  " + FinalVolume.GetValueAt(i).ToString();									
					
								textLayout2 = new TextLayout(Globals.DirectWriteFactory, FS, textFormat2, 1000, 1000);

								float boxpadding = textFormat2.FontSize;
								
		           
								float RectWidth = textLayout2.Metrics.Width + (float) TextFont.Size;
								float RectHeight = textLayout2.Metrics.Height  + (float) TextFont.Size / 2f + 1;
								
								
								
								float newy = y1 + pTextOffset;
								float newx = x1 - RectWidth/2;
							
								TextPoint = new Point(newx, newy);
							
								
								
								
								SharpDX.RectangleF			rect2			= new SharpDX.RectangleF(newx, newy, RectWidth, RectHeight);

								
								//Print(HV1 + "  " + HV2 + "  " + HV3);
								
								bool ISVOLUMEOK = LV1 < LV2 && LV2 < LV3 && LV3 != 0 && LV2 != 0 && pS3SE && pS3SEText;
								bool ISPRICEOK = LP1 > LP2 && LP2 > LP3 && LP3 != 0 && LP2 != 0 && pS3SE && pS3SEBox && ISVOLUMEOK;
								
								//ISPRICEOK = true;
								
								ISPRICEOK = AllSignals.GetValueAt(i) != 0 && pS3SE;
								
								if (ISPRICEOK)
								{
								
									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;
									RenderTarget.DrawRectangle(rect2, pBrush07.BrushDX, pBrush07.Width, pBrush07.StrokeStyle);
									RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
									
								}
								
								
								
//								if (ISVOLUMEOK)
//									RenderTarget.DrawText(FS, textFormat2, rect2, TextBrushDX);
//								else
									
								
								
									TextFinalBrushDX = TextDownBrushDX;
								
									if (AllDirection.GetValueAt(i) != "LL" && pThisColorMode == "Status")
										TextFinalBrushDX = TextUpBrushDX;
									
									if (AllDirection.GetValueAt(i) == "DB" && pThisColorMode == "Status")
										TextFinalBrushDX = TextNeutralBrushDX;
									
									
									
									RenderTarget.DrawText(FS, textFormat2, rect2, TextFinalBrushDX);
								
									
									
	
								
								ThisVolume = 0;
							}							
							
							
							
							
							
							
							
							
//							if (pShowTicks || pShowVolume)
//							{
								
//								ThisSize = PreviousPrice - FinalLow.GetValueAt(i);

								
//								if (pShowTicks && pShowVolume)
//									FS = Math.Round(ThisSize/TickSize,0).ToString() + " / " + LV1.ToString();								
//								else if (pShowTicks)
//									FS = Math.Round(ThisSize/TickSize,0).ToString();
//								else if (pShowVolume)
//									FS = LV1.ToString();
								
											
								
//									//FS = ThisVolume.ToString();// + "  " + FinalVolume.GetValueAt(i).ToString();									
										

								
								
								
//								textLayout2 = new TextLayout(Globals.DirectWriteFactory, FS, textFormat2, 1000, textFormat2.FontSize);

//								double newy = y1 + pTextOffset;
//								double newx = x1 - textLayout2.Metrics.Width/2;
							
//								TextPoint = new Point(newx, newy);
							
//								if (LV1 < LV2 && LV2 < LV3 && LV3 != 0 && LV2 != 0 && pS3SE && pS3SEText)
//									RenderTarget.DrawTextLayout(TextPoint.ToVector2(), textLayout2, TextBrushDX);
//								else
//									RenderTarget.DrawTextLayout(TextPoint.ToVector2(), textLayout2, TextDownBrushDX);
							
							
//								//SZ = graphics.MeasureString(FS,pTextFont2);
								
//								//if (!InHitTest) graphics.DrawString(FS,pTextFont2,LowTextBrush,x1-SZ.Width/2,y1+pTextOffset);	
								
//								ThisVolume = 0;
//							}
						}						
						
						
						
					}
				
					//Print(BB + "   " + CurrentBar);
					
					PreviousBB = i;
					PreviousDir = AllPivots.GetValueAt(i);
					if (PreviousDir == 1)
						PreviousPrice = FinalHigh.GetValueAt(i);
					if (PreviousDir == -1)
						PreviousPrice = FinalLow.GetValueAt(i);
				}
			}
			
			
			

//			Pen longBrush = new Pen(BackUpColor,pLineWidth);
//			Pen shortBrush = new Pen(BackDnColor,pLineWidth2);
//			Pen currentBrush = new Pen(Color.Black,2);
			

//			longBrush.DashStyle = pLineDashStyle;
//			shortBrush.DashStyle = pLineDashStyle2;
			
//		//	Print("hey");

			
			
			TextBrushDX.Dispose();
			textFormat2.Dispose();
	
			textLayout2.Dispose();				
			
			TextUpBrushDX.Dispose();
			TextDownBrushDX.Dispose();
			TextFinalBrushDX.Dispose();
			TextNeutralBrushDX.Dispose();
			
			// end of plots for swings
			
			
			
			// chart buttons
			
	 System.Windows.Media.Brush selectBrush = Brushes.Yellow;
            SharpDX.Direct2D1.Brush selectBrushDX = selectBrush.ToDxBrush(RenderTarget);

          
            System.Windows.Media.Brush buttonBrush = ChartControl.Properties.AxisPen.Brush;
            SharpDX.Direct2D1.Brush buttonBrushDX = buttonBrush.ToDxBrush(RenderTarget);

            System.Windows.Media.Brush buttonHBrush = ChartControl.Properties.AxisPen.Brush;
            SharpDX.Direct2D1.Brush buttonHBrushDX = buttonHBrush.ToDxBrush(RenderTarget);

            System.Windows.Media.Brush buttonFHBrush = ChartControl.Properties.AxisPen.Brush;
            SharpDX.Direct2D1.Brush buttonFHBrushDX = buttonFHBrush.ToDxBrush(RenderTarget);

            System.Windows.Media.Brush buttonFOFFBrush = ChartControl.Properties.ChartBackground;
            SharpDX.Direct2D1.Brush buttonFOFFBrushDX = buttonFOFFBrush.ToDxBrush(RenderTarget);

            System.Windows.Media.Brush buttonFONBrush = Brushes.Green;
            SharpDX.Direct2D1.Brush buttonFONBrushDX = areaBrush.ToDxBrush(RenderTarget);
		
			


           // if (ChartControl.Properties.ChartBackground == blackBrush)
            {
               // Print("asf");

                //buttonHBrushDX.Opacity = 0.7f;
                //buttonFHBrushDX.Opacity = 0.4f;
                //buttonFONBrushDX.Opacity = 0.9f;

            }
           // else
            {
                buttonHBrushDX.Opacity = 0.5f;
                buttonFHBrushDX.Opacity = 0.0f;
                //buttonFONBrushDX.Opacity = 0.2f;
            }

            
			
			
			  float CY = (float)chartControl.CanvasRight - 48f;
			
			
			  B2 = new SharpDX.RectangleF(0, space - 4, 10000, pButtonSize + 2);



            // if (MouseIn(B2, 2, 2))


            if (pButtonsEnabled && InMenu)
                foreach (KeyValuePair<double, ButtonZ> thisbutton in AllButtonZ)
                //foreach (string xxx in AllButtons)
                {

                    string xxx = thisbutton.Value.Text;

                    string sd = xxx;



                    // szvv = graphics.MeasureString(sd, ChartControl.Font);

                    // int widdd = (int)szvv.Width + 8;


                    float widdd = 40;
                    widdd = Math.Max(pButtonSize, widdd);

                    if (thisbutton.Value.Width == 1)
                        widdd = pButtonSize;
                    else
                        widdd = thisbutton.Value.Width;


                    



                    SharpDX.DirectWrite.TextFormat ButtonText = new SharpDX.DirectWrite.TextFormat(Core.Globals.DirectWriteFactory, "Arial", SharpDX.DirectWrite.FontWeight.Normal,
                    SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 11.0F);

                    ButtonText = myProperties.LabelFont.ToDirectWriteTextFormat();

                    ButtonText.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
                    ButtonText.ParagraphAlignment = SharpDX.DirectWrite.ParagraphAlignment.Center;
                    ButtonText.WordWrapping = SharpDX.DirectWrite.WordWrapping.NoWrap;

                    TextLayout textLayout1 = new TextLayout(Core.Globals.DirectWriteFactory, thisbutton.Value.Text, ButtonText, 10000, 10000);

                   // Print(textLayout1.Metrics.Width);


                    float FinalH = textLayout1.Metrics.Height;

                    FinalH = Math.Max(pButtonSize, FinalH);

                    float FinalW = Math.Max(FinalH,textLayout1.Metrics.Width);

                    if (thisbutton.Value.Width == 1)
                        FinalW = FinalH;
                    else
                        FinalW = textLayout1.Metrics.Width + 8;


                    CY = CY - FinalW;
                    thisbutton.Value.Rect = new SharpDX.RectangleF(CY, space, FinalW, FinalH);
                    CY = CY - space;

                    //CY = CY - widdd - space;
                   // thisbutton.Value.Rect = new SharpDX.RectangleF(CY, space, widdd, pButtonSize);
                   
                    


                    RenderTarget.FillRectangle(thisbutton.Value.Rect, buttonFOFFBrushDX);
                    if (thisbutton.Value.Switch)
                        RenderTarget.FillRectangle(thisbutton.Value.Rect, buttonFONBrushDX);

                    if (MouseIn(thisbutton.Value.Rect, 2, 2))
                    {
                        if (!thisbutton.Value.Switch)
                            RenderTarget.FillRectangle(thisbutton.Value.Rect, buttonFHBrushDX);

                        RenderTarget.DrawRectangle(thisbutton.Value.Rect, buttonHBrushDX, 3);

                    }

                    RenderTarget.DrawRectangle(thisbutton.Value.Rect, buttonBrushDX, 1);
                    RenderTarget.DrawText(thisbutton.Value.Text, ButtonText, thisbutton.Value.Rect, buttonBrushDX);
                }





            selectBrushDX.Dispose();

			
				
			buttonBrushDX.Dispose();
            buttonHBrushDX.Dispose();
			buttonFHBrushDX.Dispose();
            buttonFOFFBrushDX.Dispose();
			buttonFONBrushDX.Dispose();	
				
				
			

		return;
					
			
			
			
			
					
			// RenderTarget is always full panel, so we need to be mindful which sub ChartPanel we're dealing with
			// always use ChartPanel X, Y, W, H - as ActualWidth, Width, ActualHeight, Height are in WPF units, so they can be drastically different depending on DPI set
			
			Point startPoint	= new Point(ChartPanel.X, ChartPanel.Y);
			Point endPoint		= new Point(ChartPanel.X + ChartPanel.W, ChartPanel.Y + ChartPanel.H);
			Point endPoint2		= new Point(ChartPanel.X + ChartPanel.W, ChartPanel.Y + ChartPanel.H);			
						
			Point startPoint1	= new Point(ChartPanel.X, ChartPanel.Y + ChartPanel.H);
			Point endPoint1		= new Point(ChartPanel.X + ChartPanel.W, ChartPanel.Y);

			Point nextPoint		= new Point(ChartPanel.X + ChartPanel.W, ChartPanel.Y);
			
			double width		= endPoint.X - startPoint.X;
			double height		= endPoint.Y - startPoint.Y;

			TextFormat	textFormat			= TextFont.ToDirectWriteTextFormat();	
			
			
			
//			TextFormat textFormat		= new TextFormat(Core.Globals.DirectWriteFactory, "Calibri", SharpDX.DirectWrite.FontWeight.Normal,
//															SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, fontHeight) 
//															{ TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading, WordWrapping = WordWrapping.NoWrap };
		

		
			
			SharpDX.Direct2D1.Brush			areaBrushDx			= areaBrush.ToDxBrush(RenderTarget);
			SharpDX.Direct2D1.Brush			smallAreaBrushDx	= smallAreaBrush.ToDxBrush(RenderTarget);
			SharpDX.Direct2D1.Brush			textBrushDx			= textBrush.ToDxBrush(RenderTarget);
			SharpDX.Direct2D1.Brush			lineBrushDx			= textBrush.ToDxBrush(RenderTarget);
			
			
			
			
			
			//SharpDX.Direct2D1.AntialiasMode oldAntialiasMode	= RenderTarget.AntialiasMode;
			RenderTarget.AntialiasMode							= SharpDX.Direct2D1.AntialiasMode.Aliased;
		
			
			
			// Set text to chart label color and font
			//textFormat			= chartControl.Properties.LabelFont.ToDirectWriteTextFormat();

//			// Loop through each Plot Values on the chart
			for (int seriesCount = 0; seriesCount < Values.Length; seriesCount++)
			{
				
				if (seriesCount <= 3 && !pShowToday)
					continue;
				
				if (seriesCount > 3 && !pShowYesterday)
					continue;				
		
				double	y					= -1;
				double	startX				= -1;
				double	endX				= -1;
				int		firstBarIdxToPaint	= -1;
				int		firstBarPainted		= ChartBars.FromIndex;
				int		lastBarPainted		= ChartBars.ToIndex;
				Plot	plot				= Plots[seriesCount];

				//blueBrush = BrushSeries[seriesCount];
				
				//lineBrushDx = Plots[0].DashStyleDX;
				
				
				smallAreaBrush	= plot.Brush;
				smallAreaBrushDx	= smallAreaBrush.ToDxBrush(RenderTarget);
				smallAreaBrushDx.Opacity = areaOpacity/100F;
				
				
				
//				for (int i = newSessionBarIdxArr.Count - 1; i >= 0; i--)
//				{
//					int prevSessionBreakIdx = newSessionBarIdxArr[i];
//					if (prevSessionBreakIdx <= lastBarPainted)
//					{
//						firstBarIdxToPaint = prevSessionBreakIdx;
//						break;
//					}
//				}

				
				
				preval = 0;
				
				//Print("===== " + lastBarPainted + "   " + CurrentBars[0]);
				
				lastBarPainted		= ChartBars.ToIndex - 1;
				
				FirstOne = true;
				
				// Loop through visble bars to render plot values	
				
				int startbar = firstBarPainted;
				startbar = 0;
				
				int endbar = lastBarPainted;
				//endbar = CurrentBars[0];
				
				for (int idx = endbar; idx >= 0; idx--)
				//for (int idx = endbar; idx >= Math.Max(startbar, endbar - width); idx--)
				{
					
					//Print(idx);
					
					//if (idx < firstBarIdxToPaint)
					//	break;

					startX		= chartControl.GetXByBarIndex(ChartBars, idx)-1;
				
					endX		= chartControl.GetXByBarIndex(ChartBars, idx+1)-1;
					
					if (idx == lastBarPainted)
					{
						//Print(idx + "  " + endX);
						endX		= endX+20;
						//Print(endX);
					}
						
					preval = val;
					val	= Values[seriesCount].GetValueAt(idx);
					y			= chartScale.GetYByValue(val);
					
					bool c1 = idx == lastBarPainted;
					bool c2 = preval != val && preval != 0;
					c2 = preval != val;
					
					
					if (c1)
						//endX = endX + myProperties.BarMarginRight;
						endX = endX + ChartPanel.W + 0;
						
						
					// Draw pivot lines
					startPoint	= new Point(startX, y);
					endPoint		= new Point(endX, y);

					//RenderTarget.DrawLine(startPoint.ToVector2(), endPoint.ToVector2(), plot.BrushDX , plot.Width, plot.StrokeStyle);
					SharpDX.RectangleF			rect2			= new SharpDX.RectangleF((float)startX,(float)y-pShadowWidth-1,(float)endX-(float)startX,pShadowWidth*2+1);
					//RenderTarget.FillRectangle(rect2, smallAreaBrushDx);	
					
					// line moved so draw labels

					if (c1 || c2)
					{
						
						
						
						startPoint	= new Point(endX, 0);
						endPoint	= new Point(endX, 2000);
						//RenderTarget.DrawLine(startPoint.ToVector2(), endPoint.ToVector2(), lineBrushDx , 1);					
						
						
						
						startPoint	= new Point(chartControl.GetXByBarIndex(ChartBars, idx+1), y);
						endPoint		= new Point(endX, y);
						
						
								
						//if (val == 0)
						
						if (pLabelsEnabled)
						if (val != 0)
						{
						
							TextLayout textLayout = new TextLayout(Globals.DirectWriteFactory, plot.Name, textFormat, 1000, textFormat.FontSize);

							double newy = y-textLayout.Metrics.Height-3;
							
							// text is on a previous line
							endPoint2 = new Point(startPoint.X - textLayout.Metrics.Width - 4 - pRightPX, newy);
							
							if (c1) // text is on right edge
								endPoint2 = new Point(ChartPanel.W - textLayout.Metrics.Width - 4 - pRightPX, newy); 
							
							
							RenderTarget.DrawTextLayout(endPoint2.ToVector2(), textLayout, plot.BrushDX);

							textLayout.Dispose();						
						}
						
				
						
						if (preval != 0)
						if (!FirstOne)
						{
							

							
							nextPoint.Y = prey;
							endPoint.Y = prey;
						
						
							RenderTarget.DrawLine(nextPoint.ToVector2(), endPoint.ToVector2(),  plot.BrushDX , plot.Width, plot.StrokeStyle);
						
											
								float xxxwid = (float) endPoint.X - (float) nextPoint.X;
								
								rect2			= new SharpDX.RectangleF((float)nextPoint.X,(float)endPoint.Y-pShadowWidth-1,xxxwid,pShadowWidth*2+1);
					
								
								
								RenderTarget.FillRectangle(rect2, smallAreaBrushDx);	
						}
						
						
							
						
						
						
						nextPoint = endPoint;
						
						FirstOne = false;
						
					}
					
					
					prey = y;

					
					
				}



				// Draw pivot text
				
				

			}
				
			textFormat.Dispose();
			

			areaBrushDx.Dispose();
			smallAreaBrushDx.Dispose();
			lineBrushDx.Dispose();
			textBrushDx.Dispose();
			
			RenderTarget.AntialiasMode = oldAntialiasMode;
			
			
		
				
		}
	
		
		private SharpDX.RectangleF ExpandRect (SharpDX.RectangleF RR, float left, float right, float top, float bottom)
		{
			
			SharpDX.RectangleF FF = new SharpDX.RectangleF(RR.X-left, RR.Y-top, RR.Width+left+right, RR.Height+top+bottom);
				
			return FF;
			
		}
				
        private void AddButtonZ(string iText, string iName, int iWidth, bool iSwitch)
        {
            ButtonZ Z = new ButtonZ();
            Z.Text = iText;
            Z.Name = iName;
            Z.Width = iWidth;
            Z.Switch = iSwitch;
            Z.Rect = new SharpDX.RectangleF(0, 0, 0, 0);
            Z.Hovered = false;

            AllButtonZ.Add(AllButtonZ.Count + 1, Z);

        }
					
      
        private bool MouseIn(SharpDX.RectangleF RR, int XF, int YF)
        {

			if (FinalXPixel != 0)
            if (FinalXPixel >= RR.Left - XF && FinalXPixel <= RR.Right + XF && FinalYPixel >= RR.Top - YF && FinalYPixel <= RR.Bottom + YF)
                return true;
           
                return false;

        }		
		
		
		internal void OnMouseMove(object sender, MouseEventArgs e)
    	{
            this.MP = e.GetPosition(this.ChartPanel);
			
			FinalXPixel = MP.X / 100 * dpiX;
			FinalYPixel = MP.Y / 100 * dpiY;
			
							
            foreach (KeyValuePair<double, ButtonZ> thisbutton in AllButtonZ)
                {
                    bool hoverednew = MouseIn(thisbutton.Value.Rect, 2, 2);
                    bool hoverednow = thisbutton.Value.Hovered;

                    if (hoverednew && !hoverednow)
                    {
                        thisbutton.Value.Hovered = true;
                        this.ChartControl.InvalidateVisual();
                    }
                    if (!hoverednew && hoverednow)
                    {
                        thisbutton.Value.Hovered = false;
                        this.ChartControl.InvalidateVisual();
                    }

                }

                InMenuP = InMenu;
                InMenu = MouseIn(B2, 8, 8);
           
            if (InMenu != InMenuP)
                this.ChartControl.InvalidateVisual();
			
			
				
		
  				InBlockNow = false;	
				foreach (SharpDX.RectangleF R in BlockTradeButtons)
				{
					if (MouseIn(R,pLineHoverOffset,pLineHoverOffset))
					{
						InBlockNow = true;	
					}
					
				}
				
				//Print(InBlockNow);
				
				if (InBlockNow != InBlockNowP)
				{
					//Print("Refresh Block");
					this.ChartControl.InvalidateVisual();
				}
				
				InBlockNowP = InBlockNow;       	
			
		}
		
		
		
        internal void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
			//Print("MouseDown");
			
         //   this.MP = e.GetPosition(this.ChartPanel);

			//return;
			
			if (CurrentHoveredLineBar != 0)
				
				if (!BarsToHide.Contains(CurrentHoveredLineBar))
				{
					BarsToHide.Add(CurrentHoveredLineBar);
					this.ChartControl.InvalidateVisual();
				}
			
				
				
				// top chart buttons
			
            foreach (KeyValuePair<double, ButtonZ> thisbutton in AllButtonZ)
            {
                bool hoverednew = MouseIn(thisbutton.Value.Rect, 2, 2);
                string buttonn = thisbutton.Value.Text;



//               if (hoverednew && buttonn == "TRADES")
//                {
//                    pSLOffset = Math.Max(0, pSLOffset - 1);
                    
//					TogglePlotExecutions();
					
//					thisbutton.Value.Switch = ChartBars.Properties.PlotExecutions != ChartExecutionStyle.DoNotPlot;
					
//					this.ChartControl.InvalidateVisual();
//					return;

//                }               
              
//                else if (hoverednew && buttonn == "SLM")
//                {
//                    if (pUseSLM)
//                    {
//                        pUseSLM = false;
//                    }
//                    else
//                    {
//                        pUseSLM = true;
//                    }
//                    thisbutton.Value.Switch = pUseSLM;
//                    this.ChartControl.InvalidateVisual();
					
//					return;

//                }

                if (hoverednew && buttonn == "Reset Lines")
                {
                    BarsToHide.Clear();
                    this.ChartControl.InvalidateVisual();
					
					return;

                }  

            }
			
			
			
				
				
				
		}
		
        internal void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
			 	
			
			this.ChartControl.InvalidateVisual();
		}
				
		
        internal void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
			 
		}
		

        internal void OnMouseLeave(object sender, MouseEventArgs e)
        {
            
             
		}		
		
		
		
		
        public override string DisplayName
        {
            get { return ThisName; }
        }			

		
		
		
		
		
		private bool pBackEnabled = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Background", Order = 1)]
        public bool BackEnabled
        {
            get { return pBackEnabled; }
            set { pBackEnabled = value; }
        }

		
		
		private bool pColorAll = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Color All", Description = "", GroupName = "Background", Order = 2)]
        public bool ColorAll
        {
            get { return pColorAll; }
            set { pColorAll = value; }
        }
		
		
	// SESSION 9 COLOR
		
		private System.Windows.Media.Brush	pBrush09 = Brushes.Black;
		[XmlIgnore]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Color", Description = "", GroupName = "Background", Order = 4)]
		public Brush Brush09
		{
			get { return pBrush09; }
			set
			{
				pBrush09 = value;
				if (pBrush09 != null)
				{
					if (pBrush09.IsFrozen)
						pBrush09 = pBrush09.Clone();
					pBrush09.Opacity = pOpacity09 / 100d;
					pBrush09.Freeze();
				}
			}
		}

		[Browsable(false)]
		public string Brush09S
		{
			get { return Serialize.BrushToString(Brush09); }
			set { Brush09 = Serialize.StringToBrush(value); }
		}

		private int	pOpacity09 = 20;
		[Range(0, 100)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Opacity (%)", Description = "", GroupName = "Background", Order = 5)]
		public int Opacity09
		{
			get { return pOpacity09; }
			set
			{
				pOpacity09 = Math.Max(0, Math.Min(100, value));
				if (pBrush09 != null)
				{
					System.Windows.Media.Brush newBrush		= pBrush09.Clone();
					newBrush.Opacity	= pOpacity09 / 100d;
					newBrush.Freeze();
					pBrush09			= newBrush;
				}
			}
		}		
		
		
		
		
		
		
		
		
		
		private bool pShowSB = true;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Significant Bars", Order = 1)]
//        public bool ShowSB
//        {
//            get { return pShowSB; }
//            set { pShowSB = value; }
//        }

		
		private bool pShowSB4 = true;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Background Enabled", Description = "", GroupName = "Significant Bars", Order = 1)]
//        public bool ShowSB4
//        {
//            get { return pShowSB4; }
//            set { pShowSB4 = value; }
//        }			
		
// SESSION 8 COLOR
		
//		private System.Windows.Media.Brush	pBrush08 = Brushes.DarkRed;
//		[XmlIgnore]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Down Color", Description = "", GroupName = "Significant Bars", Order = 6)]
//		public Brush Brush08
//		{
//			get { return pBrush08; }
//			set
//			{
//				pBrush08 = value;
//				if (pBrush08 != null)
//				{
//					if (pBrush08.IsFrozen)
//						pBrush08 = pBrush08.Clone();
//					pBrush08.Opacity = pOpacity08 / 100d;
//					pBrush08.Freeze();
//				}
//			}
//		}

//		[Browsable(false)]
//		public string Brush08S
//		{
//			get { return Serialize.BrushToString(Brush08); }
//			set { Brush08 = Serialize.StringToBrush(value); }
//		}

//		private int	pOpacity08 = 20;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Down Opacity (%)", Description = "", GroupName = "Significant Bars", Order = 7)]
//		public int Opacity08
//		{
//			get { return pOpacity08; }
//			set
//			{
//				pOpacity08 = Math.Max(0, Math.Min(100, value));
//				if (pBrush08 != null)
//				{
//					System.Windows.Media.Brush newBrush		= pBrush08.Clone();
//					newBrush.Opacity	= pOpacity08 / 100d;
//					newBrush.Freeze();
//					pBrush08			= newBrush;
//				}
//			}
//		}		
		
	
		
	
		
		private bool pShowSB5 = false;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Inside Bars", Order = 1)]
//        public bool ShowSB5
//        {
//            get { return pShowSB5; }
//            set { pShowSB5 = value; }
//        }		
		
				

		

		
		private bool pShowSB3 = false;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Background Enabled", Description = "", GroupName = "Inside Bars", Order = 2)]
//        public bool ShowSB3
//        {
//            get { return pShowSB3; }
//            set { pShowSB3 = value; }
//        }			
				
//		private System.Windows.Media.Brush	pBrush10 = Brushes.Black;
//		[XmlIgnore]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Color", Description = "", GroupName = "Inside Bars", Order = 3)]
//		public Brush Brush10
//		{
//			get { return pBrush10; }
//			set
//			{
//				pBrush10 = value;
//				if (pBrush10 != null)
//				{
//					if (pBrush10.IsFrozen)
//						pBrush10 = pBrush10.Clone();
//					pBrush10.Opacity = pOpacity10 / 100d;
//					pBrush10.Freeze();
//				}
//			}
//		}

//		[Browsable(false)]
//		public string Brush10S
//		{
//			get { return Serialize.BrushToString(Brush10); }
//			set { Brush10 = Serialize.StringToBrush(value); }
//		}

//		private int	pOpacity10 = 20;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Opacity (%)", Description = "", GroupName = "Inside Bars", Order = 4)]
//		public int Opacity10
//		{
//			get { return pOpacity10; }
//			set
//			{
//				pOpacity10 = Math.Max(0, Math.Min(100, value));
//				if (pBrush10 != null)
//				{
//					System.Windows.Media.Brush newBrush		= pBrush10.Clone();
//					newBrush.Opacity	= pOpacity10 / 100d;
//					newBrush.Freeze();
//					pBrush10			= newBrush;
//				}
//			}
//		}		
		
		
		private bool pShowSB2 = false;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Box Enabled", Description = "", GroupName = "Inside Bars", Order = 5)]
//        public bool ShowSB2
//        {
//            get { return pShowSB2; }
//            set { pShowSB2 = value; }
//        }		
		
		private System.Windows.Media.Brush	pBrush11 = Brushes.DodgerBlue;
//		[XmlIgnore]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Color", Description = "", GroupName = "Inside Bars", Order = 6)]
//		public Brush Brush11
//		{
//			get { return pBrush11; }
//			set
//			{
//				pBrush11 = value;
//				if (pBrush11 != null)
//				{
//					if (pBrush11.IsFrozen)
//						pBrush11 = pBrush11.Clone();
//					pBrush11.Opacity = pOpacity11 / 110d;
//					pBrush11.Freeze();
//				}
//			}
//		}

//		[Browsable(false)]
//		public string Brush11S
//		{
//			get { return Serialize.BrushToString(Brush11); }
//			set { Brush11 = Serialize.StringToBrush(value); }
//		}

//		private int	pOpacity11 = 20;
//		[Range(0, 110)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Opacity (%)", Description = "", GroupName = "Inside Bars", Order = 7)]
//		public int Opacity11
//		{
//			get { return pOpacity11; }
//			set
//			{
//				pOpacity11 = Math.Max(0, Math.Min(110, value));
//				if (pBrush11 != null)
//				{
//					System.Windows.Media.Brush newBrush		= pBrush11.Clone();
//					newBrush.Opacity	= pOpacity11 / 110d;
//					newBrush.Freeze();
//					pBrush11			= newBrush;
//				}
//			}
//		}		
		
		
	
		private int pExpandRectangle = 5;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Expand (Pixels)", Description="in pixels.", GroupName = "Inside Bars", Order = 50)]
//		public int ExpandRectangle
//		{
//			get { return pExpandRectangle; }
//			set { pExpandRectangle= value; }
//		}			
		
		
		
		private int pStrength = 5;
		[Range(1, 10000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Strength (Bars)", Description="number of bars to the left required to finalize a new swing.", GroupName = "Parameters", Order = 2)]
		public int Strength
		{
			get { return pStrength; }
			set { pStrength= value; }
		}			
		

		
		private double pMinRatio = 3;
		[Range(0, 10000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Minimum Ratio", Description="", GroupName = "Parameters", Order = 3)]
		public double MinRatio
		{
			get { return pMinRatio; }
			set { pMinRatio= value; }
		}			
				
		
		
		private double pZMaxBars = 10;
		[Range(0, 10000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Maximum Bars", Description="the maximum number of countertrend bars in a trigger swing.", GroupName = "Parameters", Order = 4)]
		public double ZMaxBars
		{
			get { return pZMaxBars; }
			set { pZMaxBars= value; }
		}				
		
		
		private string pSwingBase = "Wick";
		
		
		
//		[Display(ResourceType = typeof(Custom.Resource), Name = "ShowClose", GroupName = "NinjaScriptParameters", Order = 0)]
//		public bool ShowClose
//		{ get; set; }

//		[Display(ResourceType = typeof(Custom.Resource), Name = "ShowHigh", GroupName = "NinjaScriptParameters", Order = 1)]
//		public bool ShowHigh
//		{ get; set; }

//		[Display(ResourceType = typeof(Custom.Resource), Name = "ShowLow", GroupName = "NinjaScriptParameters", Order = 2)]
//		public bool ShowLow
//		{ get; set; }

//		[Display(ResourceType = typeof(Custom.Resource), Name = "ShowOpen", GroupName = "NinjaScriptParameters", Order = 3)]
//		public bool ShowOpen
//		{ get; set; }
		
		
		private bool pLabelsEnabled = true;
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Labels Enabled", Description = "", GroupName = "Display", Order = 1)]
//        public bool LabelsEnabled
//        {
//            get { return pLabelsEnabled; }
//            set { pLabelsEnabled = value; }
//        }		

		
		private int pRightPX = 0;
//		[Range(0, 1000)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Label X Offset", Description="in pixels.", GroupName = "Display", Order = 3)]
//		public int RightPX
//		{
//			get { return pRightPX; }
//			set { pRightPX= value; }
//		}	
		
		private int pShadowWidth = 3;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Shadow Width", Description="in pixels.", GroupName = "Display", Order = 5)]
//		public int ShadowWidth
//		{
//			get { return pShadowWidth; }
//			set { pShadowWidth= value; }
//		}	

		
		
		
		private Brush		areaBrush		= Brushes.Blue;
		private Brush		textBrush		= Brushes.White;
		private Brush		smallAreaBrush	= Brushes.Red;
		private	int			areaOpacity		= 20;
		//const	float 		fontHeight		= 30f;
		
		
//		[XmlIgnore]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "NinjaScriptDrawingToolShapesAreaBrush", GroupName = "NinjaScriptGeneral")]
//		public Brush AreaBrush
//		{
//			get { return areaBrush; }
//			set
//			{
//				areaBrush = value;
//				if (areaBrush != null)
//				{
//					if (areaBrush.IsFrozen)
//						areaBrush = areaBrush.Clone();
//					areaBrush.Opacity = areaOpacity / 100d;
//					areaBrush.Freeze();
//				}
//			}
//		}

//		[Browsable(false)]
//		public string AreaBrushSerialize
//		{
//			get { return Serialize.BrushToString(AreaBrush); }
//			set { AreaBrush = Serialize.StringToBrush(value); }
//		}

//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Shadow Opacity (%)", GroupName = "Display", Order = 6)]
//		public int AreaOpacity
//		{
//			get { return areaOpacity; }
//			set
//			{
//				areaOpacity = Math.Max(0, Math.Min(100, value));
//				if (areaBrush != null)
//				{
//					Brush newBrush		= areaBrush.Clone();
//					newBrush.Opacity	= areaOpacity / 100d;
//					newBrush.Freeze();
//					areaBrush			= newBrush;
//				}
//			}
//		}
		
		
		
        private bool pShowToday = true;
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Today Enabled", Description = "", GroupName = "Plots", Order = -20)]
//        public bool ShowToday
//        {
//            get { return pShowToday; }
//            set { pShowToday = value; }
//        }		
		
        private bool pShowYesterday = true;
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Yesterday Enabled", Description = "", GroupName = "Plots", Order = -19)]
//        public bool ShowYesterday
//        {
//            get { return pShowYesterday; }
//            set { pShowYesterday = value; }
//        }				
				
		
	private TimeSpan pCustomETime = new TimeSpan(7,30,0);
		private TimeSpan pCustomTime = new TimeSpan(7,30,0);
//		[Description("Enter the start time of the range.")]
//		[Gui.Design.DisplayNameAttribute("\r\rTime Begin")]
//		[GridCategory("\t\t\t\t\t\t\tParameters")]
//		public string TimeBegin
//		{
//			get { return pStartTime.Hours.ToString("0")+":"+pStartTime.Minutes.ToString("00");}
//			set { if(!TimeSpan.TryParse(value, out pStartTime)) pStartTime=new TimeSpan(0,0,0); }
//		}
		
		private TimeSpan pStartTime = new TimeSpan(9,30,0);
//		[Description("Enter the start time of the range.")]
//		[Gui.Design.DisplayNameAttribute("\r\rTime Begin")]
//		[GridCategory("\t\t\t\t\t\t\tParameters")]
//		public string TimeBegin
//		{
//			get { return pStartTime.Hours.ToString("0")+":"+pStartTime.Minutes.ToString("00");}
//			set { if(!TimeSpan.TryParse(value, out pStartTime)) pStartTime=new TimeSpan(0,0,0); }
//		}

		private TimeSpan pEndTime = new TimeSpan(16,15,0);
//		[Description("Enter the end time of the range.")]
//		[GridCategory("\t\t\t\t\t\t\tParameters")]
//		[Gui.Design.DisplayNameAttribute("Time End")]
//		public string TimeEnd
//		{
//			get { return pEndTime.Hours.ToString("0")+":"+pEndTime.Minutes.ToString("00");}
//			set { if(!TimeSpan.TryParse(value, out pEndTime)) pEndTime=new TimeSpan(0,0,0); }
//		}
		

				
		private int pBoxShiftX = 0;
		private int pBoxShiftY = 0;
		private int pPadding = 1;
		
		
		
        private Stroke pBrush01 = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Up Wave Display", GroupName = "Swings", Order = 3)]
        public Stroke Brush01
        {
            get { return pBrush01; }
            set { pBrush01 = value; }
        }

        private Stroke pBrush02 = new Stroke(Brushes.DarkRed, DashStyleHelper.Solid, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Down Wave Display", GroupName = "Swings", Order = 5)]
        public Stroke Brush02
        {
            get { return pBrush02; }
            set { pBrush02 = value; }
        }
		
        private Stroke pBrush03 = new Stroke(Brushes.DarkGreen, DashStyleHelper.Dash, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Support Display", GroupName = "Levels (Open)", Order = 3)]
        public Stroke Brush03
        {
            get { return pBrush03; }
            set { pBrush03 = value; }
        }

        private Stroke pBrush04 = new Stroke(Brushes.DarkRed, DashStyleHelper.Dash, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Resistance Display", GroupName = "Levels (Open)", Order = 5)]
        public Stroke Brush04
        {
            get { return pBrush04; }
            set { pBrush04 = value; }
        }		
		
        private Stroke pBrush05 = new Stroke(Brushes.DimGray, DashStyleHelper.Dash, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Support Display", GroupName = "Levels (Closed)", Order = 3)]
        public Stroke Brush05
        {
            get { return pBrush05; }
            set { pBrush05 = value; }
        }

        private Stroke pBrush06 = new Stroke(Brushes.DimGray, DashStyleHelper.Dash, 2);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Resistance Display", GroupName = "Levels (Closed)", Order = 5)]
        public Stroke Brush06
        {
            get { return pBrush06; }
            set { pBrush06 = value; }
        }	
		

			
			
				
			private bool pS3SE = true;
			[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Pattern Boxes", Order = 1)]
	        public bool S3SE
	        {
	            get { return pS3SE; }
	            set { pS3SE = value; }
	        }	
			
			
        private Stroke pBrush07 = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 1);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Buy Boxes", GroupName = "Pattern Boxes", Order = 3)]
        public Stroke Brush07
        {
            get { return pBrush07; }
            set { pBrush07 = value; }
        }

        private Stroke pBrush08 = new Stroke(Brushes.Red, DashStyleHelper.Solid, 1);
        [Display(ResourceType = typeof(Custom.Resource), Name = "Sell Boxes", GroupName = "Pattern Boxes", Order = 5)]
        public Stroke Brush08
        {
            get { return pBrush08; }
            set { pBrush08 = value; }
        }	
		
		
//		private int pOpacity01 = 60;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Up Wave Opacity (%)", Description="", GroupName = "Swings", Order = 4)]
//		public int Opacity01
//		{
//			get { return pOpacity01; }
//			set { pOpacity01 = value; }
//		}		
		
//		private int pOpacity02 = 60;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Down Wave Opacity (%)", Description="", GroupName = "Swings", Order = 6)]
//		public int Opacity02
//		{
//			get { return pOpacity02; }
//			set { pOpacity02 = value; }
//		}				
		
//		private int pOpacity03 = 80;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Support Opacity (%)", Description="", GroupName = "Levels (Open)", Order = 4)]
//		public int Opacity03
//		{
//			get { return pOpacity03; }
//			set { pOpacity03 = value; }
//		}		
		
//		private int pOpacity04 = 80;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Resistance Opacity (%)", Description="", GroupName = "Levels (Open)", Order = 6)]
//		public int Opacity04
//		{
//			get { return pOpacity04; }
//			set { pOpacity04 = value; }
//		}	
		
//		private int pOpacity05 = 80;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Support Opacity (%)", Description="", GroupName = "Levels (Closed)", Order = 4)]
//		public int Opacity05
//		{
//			get { return pOpacity05; }
//			set { pOpacity05 = value; }
//		}		
		
//		private int pOpacity06 = 80;
//		[Range(0, 100)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Resistance Opacity (%)", Description="", GroupName = "Levels (Closed)", Order = 6)]
//		public int Opacity06
//		{
//			get { return pOpacity06; }
//			set { pOpacity06 = value; }
//		}	
		


		
		
		
		
		private bool pDotsEnabled = false;		
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Plots", Order = -1)]
//        public bool DotsEnabled
//        {
//            get { return pDotsEnabled; }
//            set { pDotsEnabled = value; }
//        }
		
		//private int pDotSize = 2;
	
		
		private int pOffsetTicks = 1;		
//		[Range(0, 1000000)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Offset (Ticks)", Description="", GroupName = "Plots", Order = 10)]
//		public int OffsetTicks
//		{
//			get { return pOffsetTicks; }
//			set { pOffsetTicks= value; }
//		}
		
		

			
			private bool pS3SEBox = true;
//			[Display(ResourceType = typeof(Custom.Resource), Name = "Boxes Enabled", Description = "", GroupName = "3 Stooges", Order = 4)]
//	        public bool S3SEBox
//	        {
//	            get { return pS3SEBox; }
//	            set { pS3SEBox = value; }
//	        }					
			
		
//        private Stroke pBrush07 = new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 1);
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Buy Boxes", GroupName = "3 Stooges", Order = 5)]
//        public Stroke Brush07
//        {
//            get { return pBrush07; }
//            set { pBrush07 = value; }
//        }

 //       private Stroke pBrush08 = new Stroke(Brushes.Red, DashStyleHelper.Solid, 1);
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Sell Boxes", GroupName = "3 Stooges", Order = 6)]
//        public Stroke Brush08
//        {
//            get { return pBrush08; }
//            set { pBrush08 = value; }
//        }				
			
			
			
			
			
			
		
		private bool pShowStatus = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled (Status)", Description = "", GroupName = "Text", Order = 1)]
        public bool ShowStatus
        {
            get { return pShowStatus; }
            set { pShowStatus = value; }
        }
					
			
		private bool pShowTicks = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled (Ticks)", Description = "", GroupName = "Text", Order = 2)]
        public bool ShowTicks
        {
            get { return pShowTicks; }
            set { pShowTicks = value; }
        }
	
		private bool pShowVolume = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled (Volume)", Description = "", GroupName = "Text", Order = 3)]
        public bool ShowVolume
        {
            get { return pShowVolume; }
            set { pShowVolume = value; }
        }
				
		private bool pShowRatios = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled (Ratio)", Description = "", GroupName = "Text", Order = 4)]
        public bool ShowRatios
        {
            get { return pShowRatios; }
            set { pShowRatios = value; }
        }

		private bool pShowBars = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled (Bars)", Description = "", GroupName = "Text", Order = 5)]
        public bool ShowBars
        {
            get { return pShowBars; }
            set { pShowBars = value; }
        }
		
	
		[Display(ResourceType = typeof(Custom.Resource), Name="Font", Description="", GroupName="Text", Order = 10)]
		public SimpleFont TextFont
		{ get; set; }	
		
			
		private string pThisColorMode = "Status";
		[Description("")]
		[Display(ResourceType = typeof(Custom.Resource), GroupName = "Text", Name = "Color Mode", Order = 11)]
		[RefreshProperties(RefreshProperties.All)]
		[TypeConverter(typeof(ButtonMode3))]
		public string ThisColorMode
		{
			get { return pThisColorMode; }
			set { pThisColorMode = value; }
		}
		
		
		
		
		internal class ButtonMode3 : StringConverter
		{
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
			//true means show a combobox
				return true;
			}
			
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
			//true will limit to list. false will show the list, but allow free-form entry
				return true;
			}
		
			public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new StandardValuesCollection( new String[] {"High / Low", "Status"} );
			}
		}	
		
		
		
		
		
		
			private System.Windows.Media.Brush pColorUpBrush	= Brushes.LimeGreen;
			[XmlIgnore]
			[Display(ResourceType = typeof(Custom.Resource), Name = "Color Up", Description="", GroupName = "Text", Order = 14)]
			public System.Windows.Media.Brush ColorUpBrush
			{
				get { return pColorUpBrush; } set { pColorUpBrush = value; }
			}
			[Browsable(false)]
			public string ColorUpBrushS
			{
				get { return Serialize.BrushToString(pColorUpBrush); } set { pColorUpBrush = Serialize.StringToBrush(value); }
			}	
			
			private System.Windows.Media.Brush pColorDownBrush	= Brushes.Red;
			[XmlIgnore]
			[Display(ResourceType = typeof(Custom.Resource), Name = "Color Down", GroupName = "Text", Order = 15)]
			public System.Windows.Media.Brush ColorDownBrush
			{
				get { return pColorDownBrush; } set { pColorDownBrush = value; }
			}
			[Browsable(false)]
			public string ColorDownBrushS
			{
				get { return Serialize.BrushToString(pColorDownBrush); } set { pColorDownBrush = Serialize.StringToBrush(value); }
			}	
			
		
			private bool pS3SEText = true;
//			[Display(ResourceType = typeof(Custom.Resource), Name = "Text Enabled (Volume)", Description = "", GroupName = "3 Stooges", Order = 2)]
//	        public bool S3SEText
//	        {
//	            get { return pS3SEText; }
//	            set { pS3SEText = value; }
//	        }		
			
			private System.Windows.Media.Brush pColorStoogesBrush	= Brushes.DimGray;
			[XmlIgnore]
			[Display(ResourceType = typeof(Custom.Resource), Name = "Color Neutral", GroupName = "Text", Order = 16)]
			public System.Windows.Media.Brush pColorStoogesBrushSS
			{
				get { return pColorStoogesBrush; } set { pColorStoogesBrush = value; }
			}
			[Browsable(false)]
			public string pColorStoogesBrushS
			{
				get { return Serialize.BrushToString(pColorStoogesBrush); } set { pColorStoogesBrush = Serialize.StringToBrush(value); }
			}	
	
		private int pTextOffset = 10;		
		[Range(0, 1000000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Offset (Pixels)", Description="", GroupName = "Text", Order = 20)]
		public int TextOffset
		{
			get { return pTextOffset; }
			set { pTextOffset= value; }
		}		
		
	
		
		
		private int pRatioRound = 2;		
		[Range(0, 1000000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Ratio Display Digits", Description="", GroupName = "Text", Order = 100)]
		public int RatioRound
		{
			get { return pRatioRound; }
			set { pRatioRound= value; }
		}		
		
		
				
		private int pDisplayUnits = 1;
		[Range(1, 1000000)]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Volume Display Units", Description="", GroupName = "Text", Order = 101)]
		public int DisplayUnits
		{
			get { return pDisplayUnits; }
			set { pDisplayUnits= value; }
		}
		
		
		
	
		
		private int pTicksMove = 12;
//		[Range(1, 1000000)]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Minimum Move (Ticks)", Description="", GroupName = "Parameters", Order = 3)]
//		public int TicksMove
//		{
//			get { return pTicksMove; }
//			set { pTicksMove= value; }
//		}
		
		

		
		private bool pLinesEnabled = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Swings", Order = -1)]
        public bool LinesEnabled
        {
            get { return pLinesEnabled; }
            set { pLinesEnabled = value; }
        }		
		
		private bool pHistoricalLevelsEnabled = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Levels (Closed)", Order = -1)]
        public bool HistoricalLevelsEnabled
        {
            get { return pHistoricalLevelsEnabled; }
            set { pHistoricalLevelsEnabled = value; }
        }
				
		
		private bool pLevelsEnabled = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Levels (Open)", Order = -1)]
        public bool LevelsEnabled
        {
            get { return pLevelsEnabled; }
            set { pLevelsEnabled = value; }
        }
		
		
		private bool pLevelsEnabled3 = false;
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Levels (Open) Global", Order = -1)]
//        public bool LevelsEnabled3
//        {
//            get { return pLevelsEnabled3; }
//            set { pLevelsEnabled3 = value; }
//        }
				
		
		
			
		private bool pMenuEnabled = true;
		[Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Menu", Order = -1)]
        public bool MenuEnabled
        {
            get { return pMenuEnabled; }
            set { pMenuEnabled = value; }
        }
		
		
		
//		private string pSupportName = "";
//		[Description("")]
//		[Display(ResourceType = typeof(Custom.Resource), GroupName = "Levels (Open) Global", Name = "Support Template Name", Description = "",  Order = 31)]

//		public string SupportName
//		{
//			get { return pSupportName; }
//			set { pSupportName = value; }
//		}
		
		
//		private string pResistanceName = "";
//		[Description("")]
//		[Display(ResourceType = typeof(Custom.Resource), GroupName = "Levels (Open) Global", Name = "Resistance Template Name", Description = "",  Order = 32)]
//		public string ResistanceName
//		{
//			get { return pResistanceName; }
//			set { pResistanceName = value; }
//		}
		
		
				
		[Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
       [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public Series<double> DR1
        {
            get { return Values[0]; }
        }
		
		[Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
       [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public Series<double> DR2
        {
            get { return Values[1]; }
        }	
		
        private bool pShowMenu = true;
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", Description = "", GroupName = "Menu", Order = -3)]
//        public bool ShowMenu
//        {
//            get { return pShowMenu; }
//            set { pShowMenu = value; }
//        }	
		
		private Brush pMenuBrush	= Brushes.LightGray;
//		[XmlIgnore]
//		[Display(ResourceType = typeof(Custom.Resource), Name = "Text Color", Description = "", GroupName = "Menu", Order = 20)]
//		public Brush MenuBrush
//		{
//			get { return pMenuBrush; } set { pMenuBrush = value; }
//		}
//		[Browsable(false)]
//		public string pMenuBrushS
//		{
//			get { return Serialize.BrushToString(pMenuBrush); } set { pMenuBrush = Serialize.StringToBrush(value); }
//		}	
		
		
		
		
        private bool pButtonsEnabled = false;
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Enabled", GroupName = "Chart Buttons", Order = 1)]
//        public bool ButtonsEnabled
//        {
//            get { return pButtonsEnabled; }
//            set { pButtonsEnabled = value; }
//        }

		
		
//        [XmlIgnore]
//        [Display(ResourceType = typeof(Custom.Resource), Name = "On Color", GroupName = "Chart Buttons", Order = 2)]
//        public Brush AreaBrush
//        {
//            get { return areaBrush; }
//            set
//            {
//                areaBrush = value;
//                if (areaBrush != null)
//                {
//                    if (areaBrush.IsFrozen)
//                        areaBrush = areaBrush.Clone();
//                    areaBrush.Opacity = areaOpacity / 100d;
//                    areaBrush.Freeze();
//                }
//            }
//        }

//        [Browsable(false)]
//        public string AreaBrushSerialize
//        {
//            get { return Serialize.BrushToString(AreaBrush); }
//            set { AreaBrush = Serialize.StringToBrush(value); }
//        }
		
		

//        [Range(0, 100)]
//        [Display(ResourceType = typeof(Custom.Resource), Name = "On Opacity (%)", GroupName = "Chart Buttons", Order = 3)]
//        public int AreaOpacity
//        {
//            get { return areaOpacity; }
//            set
//            {
//                areaOpacity = Math.Max(0, Math.Min(100, value));
//                if (areaBrush != null)
//                {
//                    Brush newBrush = areaBrush.Clone();
//                    newBrush.Opacity = areaOpacity / 100d;
//                    newBrush.Freeze();
//                    areaBrush = newBrush;
//                }
//            }
//        }

        private int pButtonSize = 20;
//        [Range(1, int.MaxValue), NinjaScriptProperty]
//        [Display(ResourceType = typeof(Custom.Resource), Name = "Size (Pixels)", GroupName = "Chart Buttons", Order = 4)]
//        public int ButtonSize
//        {
//            get { return pButtonSize; }
//            set { pButtonSize = value; }
//        }

		
		
		
//		private ThisSwingBase	pSwingBase	= ThisSwingBase.Wick;
//		[Display(GroupName = "Parameters", Name = "Swing Base", Description="", Order = 1)]
//		public ThisSwingBase SwingBase
//		{
//			get { return pSwingBase; }
//			set { pSwingBase = value; }
//		}		
		
		
		
	}
}

//public enum ThisSwingBase
//{
//	Wick,
//	Body
//}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private GrimmerLegVol[] cacheGrimmerLegVol;
		public GrimmerLegVol GrimmerLegVol()
		{
			return GrimmerLegVol(Input);
		}

		public GrimmerLegVol GrimmerLegVol(ISeries<double> input)
		{
			if (cacheGrimmerLegVol != null)
				for (int idx = 0; idx < cacheGrimmerLegVol.Length; idx++)
					if (cacheGrimmerLegVol[idx] != null &&  cacheGrimmerLegVol[idx].EqualsInput(input))
						return cacheGrimmerLegVol[idx];
			return CacheIndicator<GrimmerLegVol>(new GrimmerLegVol(), input, ref cacheGrimmerLegVol);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.GrimmerLegVol GrimmerLegVol()
		{
			return indicator.GrimmerLegVol(Input);
		}

		public Indicators.GrimmerLegVol GrimmerLegVol(ISeries<double> input )
		{
			return indicator.GrimmerLegVol(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.GrimmerLegVol GrimmerLegVol()
		{
			return indicator.GrimmerLegVol(Input);
		}

		public Indicators.GrimmerLegVol GrimmerLegVol(ISeries<double> input )
		{
			return indicator.GrimmerLegVol(input);
		}
	}
}

#endregion
