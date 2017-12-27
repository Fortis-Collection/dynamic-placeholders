using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DynamicPlaceholders.Pipelines.GetChromeData
{
	public class GetDynamicPlaceholderChromeData : GetPlaceholderChromeData
	{
		public override void Process(GetChromeDataArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			Assert.IsNotNull(args.ChromeData, "Chrome Data");

			if (string.Equals("placeholder", args.ChromeType, StringComparison.OrdinalIgnoreCase))
			{
				var placeholderKey = args.CustomData["placeHolderKey"] as string;
				var regex = new Regex(DynamicPlaceholders.PlaceholderKeyRegex.DynamicKeyRegex);

			    var matches = regex.Matches(placeholderKey);

				if (matches.Count > 0)
				{
				    var match = matches.Cast<Match>().Last();

					var newPlaceholderKey = match.Groups[1].Value;

					args.CustomData["placeHolderKey"] = newPlaceholderKey;

					base.Process(args);

					args.CustomData["placeHolderKey"] = placeholderKey;
				}
				else
				{
					base.Process(args);
				}
			}
		}
	}
}
