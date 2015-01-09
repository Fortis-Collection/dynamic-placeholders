using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicPlaceholders
{
	public static class PlaceholderKeyRegex
	{
		public static string DynamicKeyRegex = @"(.+)_[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}";
	}
}
