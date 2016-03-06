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
	public class GooStochastic : Indicator
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description							= @"Stochastic indicator with threshold color coding";
				Name								= "GooStochastic";
				Calculate							= Calculate.OnBarClose;
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
				UpperRefLine					= 80;
				LowerRefLine					= 20;
				// Default Stochastic values D=K=5 with 3 period smoothing
				PeriodD							= 5;
				PeriodK							= 5;
				PeriodMA						= 3;
				
				AddPlot(Brushes.Orange, "PlotData");
				AddLine(Brushes.Crimson, UpperRefLine, "UpperReference");
				AddLine(Brushes.Green, LowerRefLine, "LowerReference");
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		{
			SolidColorBrush plotColor = Brushes.Gray;
			double perK = this.Stochastics(PeriodD, PeriodK, PeriodMA).K[0];
			double perD = this.Stochastics(PeriodD, PeriodK, PeriodMA).D[0];
			
			PlotData[0] = perK;
			
			if(perK > UpperRefLine) {
				plotColor = Brushes.Crimson;
			} else if(perK < LowerRefLine) {
				plotColor = Brushes.LimeGreen;
			}
			
			PlotBrushes[0][0] = plotColor;
			//BackBrushes[0] = backColor;
			//BackBrushesAll[0] = allBackColor;
		}

		#region Properties
		
		[Range(1, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="PeriodK", Description="Stochastic K Period", Order=1, GroupName="Parameters")]
		public int PeriodK
		{ get; set; }
		
		[Range(1, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="PeriodD", Description="Stochastic D Period", Order=2, GroupName="Parameters")]
		public int PeriodD
		{ get; set; }
		
		[Range(1, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="PeriodMA", Description="Stochastic MA Period (Smoothing)", Order=3, GroupName="Parameters")]
		public int PeriodMA
		{ get; set; }
		
		[Range(1, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="UpperRefLine", Description="Upper Reference Line", Order=4, GroupName="Parameters")]
		public int UpperRefLine
		{ get; set; }

		[Range(1, int.MaxValue)]
		[NinjaScriptProperty]
		[Display(Name="LowerRefLine", Description="Lower Reference Line", Order=5, GroupName="Parameters")]
		public int LowerRefLine
		{ get; set; }

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> PlotData
		{
			get { return Values[0]; }
		}
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private GooStochastic[] cacheGooStochastic;
		public GooStochastic GooStochastic(int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			return GooStochastic(Input, periodK, periodD, periodMA, upperRefLine, lowerRefLine);
		}

		public GooStochastic GooStochastic(ISeries<double> input, int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			if (cacheGooStochastic != null)
				for (int idx = 0; idx < cacheGooStochastic.Length; idx++)
					if (cacheGooStochastic[idx] != null && cacheGooStochastic[idx].PeriodK == periodK && cacheGooStochastic[idx].PeriodD == periodD && cacheGooStochastic[idx].PeriodMA == periodMA && cacheGooStochastic[idx].UpperRefLine == upperRefLine && cacheGooStochastic[idx].LowerRefLine == lowerRefLine && cacheGooStochastic[idx].EqualsInput(input))
						return cacheGooStochastic[idx];
			return CacheIndicator<GooStochastic>(new GooStochastic(){ PeriodK = periodK, PeriodD = periodD, PeriodMA = periodMA, UpperRefLine = upperRefLine, LowerRefLine = lowerRefLine }, input, ref cacheGooStochastic);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.GooStochastic GooStochastic(int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooStochastic(Input, periodK, periodD, periodMA, upperRefLine, lowerRefLine);
		}

		public Indicators.GooStochastic GooStochastic(ISeries<double> input , int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooStochastic(input, periodK, periodD, periodMA, upperRefLine, lowerRefLine);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.GooStochastic GooStochastic(int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooStochastic(Input, periodK, periodD, periodMA, upperRefLine, lowerRefLine);
		}

		public Indicators.GooStochastic GooStochastic(ISeries<double> input , int periodK, int periodD, int periodMA, int upperRefLine, int lowerRefLine)
		{
			return indicator.GooStochastic(input, periodK, periodD, periodMA, upperRefLine, lowerRefLine);
		}
	}
}

#endregion
