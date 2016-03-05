#region Using declarations
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
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class GooRSI : Indicator
	{
		#region Private variables
		private int lowThreshold = 32; 	// Default Lower Threshold
		private int highThreshold=68; 	// Default Upper Threshold
		private int refLineUpper = 60;	// Upper reference line
		private int refLineLower = 40;	// Lower reference line
		private bool showRefCross=false; // color code reference line crosses
		private bool exceedThresh=false;	// Reached an extreme

		#endregion
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description							= @"RSI indicator with adjustable zones";
				Name								= "GooRSI";
				Calculate							= Calculate.OnEachTick;
				IsOverlay							= false;
				DisplayInDataBox					= true;
				DrawOnPricePanel					= true;
				DrawHorizontalGridLines				= true;
				DrawVerticalGridLines				= true;
				PaintPriceMarkers					= true;
				ScaleJustification					= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive			= true;
				LowThreshold					= 32;
				HighThreshold					= 68;
				UpperRefLine					= 60;
				LowerRefLine					= 40;
				ShowRefCross					= true;
				
				// Set up graphic objects for this indicator
				AddPlot(Brushes.Orange, "myRSI");
				AddLine(Brushes.Red, this.highThreshold, "Short");
				AddLine(Brushes.Green, this.lowThreshold, "Long");
				AddLine(Brushes.White, 60, "RefUpper");
				AddLine(Brushes.White, 40, "RefLower");
				this.DrawOnPricePanel = false;
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnConnectionStatusUpdate(ConnectionStatusEventArgs connectionStatusUpdate)
		{
			
		}

		protected override void OnBarUpdate()
		{
			SolidColorBrush plotColor = Brushes.Gray;
			SolidColorBrush backColor = Brushes.Transparent;
			
			if(this.CurrentBar < 20)
				return;
			
			// We use straight up RSI
			myRSI[0] = RSI(14,3)[1];
			
			//Debug.Print("Bar={2},[0]={0}, [1]={1}", MyRSI[0], MyRSI[1], this.CurrentBar);
			if(this.CrossAbove(myRSI, this.highThreshold, 1)==true)
			{
				plotColor = Brushes.Crimson;
				backColor = Brushes.DarkRed;
			//	this.exceedThresh = true;
			//	this.DrawDot("GooRSI"+this.CurrentBar.ToString(), true, 0, MyRSI[0], Color.Crimson);
			//	this.indicatorCodes.Set((double)Codes_RSI.CrossUp);
			} else if(myRSI[0] >= this.highThreshold) {
				plotColor = Brushes.Crimson;
			}
			else if (this.CrossBelow(myRSI, this.lowThreshold, 1)==true)
			{
				plotColor = Brushes.LimeGreen;
				backColor = Brushes.DarkGreen;
			//	this.exceedThresh = true;
			//	this.DrawDot("GooRSI"+this.CurrentBar.ToString(), true, 0, MyRSI[0], Color.LimeGreen);
			//	this.indicatorCodes.Set((double)Codes_RSI.CrossDown);
			} else if(myRSI[0] <= this.lowThreshold) {
				plotColor = Brushes.LimeGreen;
			}
			
			PlotBrushes[0][0] = plotColor;
			BackBrushesAll[0] = backColor;
		}

		#region Properties
		// myRSI data series for indicator values. Not accessible from the indicator property window
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> myRSI
		{
			get { return Values[0];}
		}
		
		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="LowThreshold", Description="Lower threshold value", Order=1, GroupName="Parameters")]
		public int LowThreshold
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="HighThreshold", Description="Upper threshold value", Order=2, GroupName="Parameters")]
		public int HighThreshold
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="UpperRefLine", Description="Upper reference line", Order=3, GroupName="Parameters")]
		public int UpperRefLine
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="LowerRefLine", Description="Lower reference line", Order=4, GroupName="Parameters")]
		public int LowerRefLine
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="ShowRefCross", Description="Enable display of reference line crosses", Order=5, GroupName="Parameters")]
		public bool ShowRefCross
		{ get; set; }
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private GooRSI[] cacheGooRSI;
		public GooRSI GooRSI(int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			return GooRSI(Input, lowThreshold, highThreshold, upperRefLine, lowerRefLine, showRefCross);
		}

		public GooRSI GooRSI(ISeries<double> input, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			if (cacheGooRSI != null)
				for (int idx = 0; idx < cacheGooRSI.Length; idx++)
					if (cacheGooRSI[idx] != null && cacheGooRSI[idx].LowThreshold == lowThreshold && cacheGooRSI[idx].HighThreshold == highThreshold && cacheGooRSI[idx].UpperRefLine == upperRefLine && cacheGooRSI[idx].LowerRefLine == lowerRefLine && cacheGooRSI[idx].ShowRefCross == showRefCross && cacheGooRSI[idx].EqualsInput(input))
						return cacheGooRSI[idx];
			return CacheIndicator<GooRSI>(new GooRSI(){ LowThreshold = lowThreshold, HighThreshold = highThreshold, UpperRefLine = upperRefLine, LowerRefLine = lowerRefLine, ShowRefCross = showRefCross }, input, ref cacheGooRSI);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.GooRSI GooRSI(int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			return indicator.GooRSI(Input, lowThreshold, highThreshold, upperRefLine, lowerRefLine, showRefCross);
		}

		public Indicators.GooRSI GooRSI(ISeries<double> input , int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			return indicator.GooRSI(input, lowThreshold, highThreshold, upperRefLine, lowerRefLine, showRefCross);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.GooRSI GooRSI(int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			return indicator.GooRSI(Input, lowThreshold, highThreshold, upperRefLine, lowerRefLine, showRefCross);
		}

		public Indicators.GooRSI GooRSI(ISeries<double> input , int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine, bool showRefCross)
		{
			return indicator.GooRSI(input, lowThreshold, highThreshold, upperRefLine, lowerRefLine, showRefCross);
		}
	}
}

#endregion
