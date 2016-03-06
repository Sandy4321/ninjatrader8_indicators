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
		private bool exceedThresh=false;	// Reached a reference threshold, either upper or lower
		#endregion
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description						= @"RSI indicator with adjustable zones";
				Name							= "GooRSI";
				Calculate						= Calculate.OnEachTick;
				IsOverlay						= false;
				DisplayInDataBox				= true;
				DrawOnPricePanel				= true;
				DrawHorizontalGridLines			= true;
				DrawVerticalGridLines			= true;
				PaintPriceMarkers				= true;
				ScaleJustification				= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive		= true;
				LowThreshold					= 32;
				HighThreshold					= 68;
				UpperRefLine					= 60;
				LowerRefLine					= 40;
				ShowRefCross					= true;
				ShowRefRetrace					= true;
				
				// Set up graphic objects for this indicator
				AddPlot(Brushes.Orange, "myRSI");
				AddLine(Brushes.Red, this.HighThreshold, "Short");
				AddLine(Brushes.Green, this.LowThreshold, "Long");
				AddLine(Brushes.White, this.UpperRefLine, "RefUpper");
				AddLine(Brushes.Blue, this.LowerRefLine, "RefLower");
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
			SolidColorBrush allBackColor = Brushes.Transparent;
			Brush dotColor = Brushes.Transparent;

			if(this.CurrentBar < 20)
				return;
			
			// We use straight up RSI to calculate actual values
			myRSI[0] = RSI(14,3)[1];
			
			//Debug.Print("Bar={2},[0]={0}, [1]={1}", MyRSI[0], MyRSI[1], this.CurrentBar);
			if(this.CrossAbove(myRSI, this.HighThreshold, 1)==true)
			{
				dotColor = Brushes.Yellow;
				plotColor = Brushes.Crimson;
				backColor = Brushes.Crimson;
				this.exceedThresh = true;
			} else if(myRSI[0] >= this.HighThreshold) {
				plotColor = Brushes.Crimson;
			} else if (this.CrossBelow(myRSI, this.LowThreshold, 1)==true)
			{
				dotColor = Brushes.Yellow;
				plotColor = Brushes.LimeGreen;
				backColor = Brushes.LimeGreen;
				this.exceedThresh = true;
			} else if(myRSI[0] <= this.LowThreshold) {
				plotColor = Brushes.LimeGreen;
			}
			
			// Display color coded crossings on the indicator pane
			if(this.ShowRefCross == true)
			{
				// Make sure we hit upper/lower threshhold before we retrace
				if(exceedThresh == true)
				{
					if(this.CrossBelow(this.myRSI, this.UpperRefLine, 1) == true)
					{
						dotColor = Brushes.Blue;
						backColor = Brushes.DarkRed;
						// Once we cross, we must breach threshhold to retrigger
						exceedThresh = false;
					}
					else if(this.CrossAbove(this.myRSI, this.LowerRefLine, 1) == true)
					{
						dotColor = Brushes.Blue;
						backColor = Brushes.DarkGreen;
						// Once we cross, we must breach threshhold to retrigger
						this.exceedThresh = false;
					}
				}
			}
			
			PlotBrushes[0][0] = plotColor;
			BackBrushes[0] = backColor;
			BackBrushesAll[0] = allBackColor;
			
			if(dotColor != Brushes.Transparent) {
				Draw.Dot(this, "dot"+this.CurrentBar.ToString(), true, 0, Close[0], dotColor, true);
			}
		}

		#region Properties
		// myRSI data series for indicator values. Not accessible from the indicator property window
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> myRSI
		{
			get { return Values[0];}
		}
		
		[NinjaScriptProperty]
		[Display(Name="Show Ref Cross", Description="Show crosses of reference lines", Order=1, GroupName="Parameters")]
		public bool ShowRefCross
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(Name="Show Ref Retrace", Description="Show crosses back across reference lines", Order=2, GroupName="Parameters")]
		public bool ShowRefRetrace
		{ get; set; }
		
		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="LowThreshold", Description="Lower threshold value", Order=3, GroupName="Parameters")]
		public int LowThreshold
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="HighThreshold", Description="Upper threshold value", Order=4, GroupName="Parameters")]
		public int HighThreshold
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="UpperRefLine", Description="Upper reference line", Order=5, GroupName="Parameters")]
		public int UpperRefLine
		{ get; set; }

		[Range(0, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="LowerRefLine", Description="Lower reference line", Order=6, GroupName="Parameters")]
		public int LowerRefLine
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
		public GooRSI GooRSI(bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			return GooRSI(Input, showRefCross, showRefRetrace, lowThreshold, highThreshold, upperRefLine, lowerRefLine);
		}

		public GooRSI GooRSI(ISeries<double> input, bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			if (cacheGooRSI != null)
				for (int idx = 0; idx < cacheGooRSI.Length; idx++)
					if (cacheGooRSI[idx] != null && cacheGooRSI[idx].ShowRefCross == showRefCross && cacheGooRSI[idx].ShowRefRetrace == showRefRetrace && cacheGooRSI[idx].LowThreshold == lowThreshold && cacheGooRSI[idx].HighThreshold == highThreshold && cacheGooRSI[idx].UpperRefLine == upperRefLine && cacheGooRSI[idx].LowerRefLine == lowerRefLine && cacheGooRSI[idx].EqualsInput(input))
						return cacheGooRSI[idx];
			return CacheIndicator<GooRSI>(new GooRSI(){ ShowRefCross = showRefCross, ShowRefRetrace = showRefRetrace, LowThreshold = lowThreshold, HighThreshold = highThreshold, UpperRefLine = upperRefLine, LowerRefLine = lowerRefLine }, input, ref cacheGooRSI);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.GooRSI GooRSI(bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooRSI(Input, showRefCross, showRefRetrace, lowThreshold, highThreshold, upperRefLine, lowerRefLine);
		}

		public Indicators.GooRSI GooRSI(ISeries<double> input , bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooRSI(input, showRefCross, showRefRetrace, lowThreshold, highThreshold, upperRefLine, lowerRefLine);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.GooRSI GooRSI(bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooRSI(Input, showRefCross, showRefRetrace, lowThreshold, highThreshold, upperRefLine, lowerRefLine);
		}

		public Indicators.GooRSI GooRSI(ISeries<double> input , bool showRefCross, bool showRefRetrace, int lowThreshold, int highThreshold, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooRSI(input, showRefCross, showRefRetrace, lowThreshold, highThreshold, upperRefLine, lowerRefLine);
		}
	}
}

#endregion
