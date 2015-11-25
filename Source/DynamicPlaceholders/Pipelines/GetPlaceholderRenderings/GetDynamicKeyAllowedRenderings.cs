using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DynamicPlaceholders.Pipelines.GetPlaceholderRenderings
{
	public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
	{
		public new void Process(GetPlaceholderRenderingsArgs args)
		{
			Assert.IsNotNull(args, "args");

			var currentPlaceholderKey = args.PlaceholderKey;
			var temporaryPlaceholderKey = string.Empty;
			var regex = new Regex(DynamicPlaceholders.PlaceholderKeyRegex.DynamicKeyRegex);
			var match = regex.Match(currentPlaceholderKey);

			if (match.Success && match.Groups.Count > 0)
			{
				temporaryPlaceholderKey = match.Groups[1].Value;
			}
			else
			{
				return;
			}

			Item placeholderItem = null;
			if (ID.IsNullOrEmpty(args.DeviceId))
			{
				placeholderItem = Client.Page.GetPlaceholderItem(temporaryPlaceholderKey, args.ContentDatabase, args.LayoutDefinition);
			}
			else
			{
				using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
				{
					placeholderItem = Client.Page.GetPlaceholderItem(temporaryPlaceholderKey, args.ContentDatabase, args.LayoutDefinition);
				}
			}
			List<Item> renderings = null;
			if (placeholderItem != null)
			{
				bool flag;
				args.HasPlaceholderSettings = true;
				renderings = this.GetRenderings(placeholderItem, out flag);
				if (flag)
				{
					args.Options.ShowTree = false;
				}
			}
			if (renderings != null)
			{
				if (args.PlaceholderRenderings == null)
				{
					args.PlaceholderRenderings = new List<Item>();
				}
				args.PlaceholderRenderings.AddRange(renderings);
			}
		}
	}
}
