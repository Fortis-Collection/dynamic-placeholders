using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DynamicPlaceholders.Events
{
	public class DynamicPlaceholder
	{
		public void OnItemSaved(object sender, EventArgs args)
		{
			var item = Event.ExtractParameter(args, 0) as Item;

			if (item != null)
			{
				var device = Context.Device;

				if (device != null)
				{
					var renderingReferences = item.Visualization.GetRenderings(device, false);

					foreach (var renderingReference in renderingReferences)
					{
						var key = renderingReference.Placeholder;
						var regex = new Regex(DynamicPlaceholders.PlaceholderKeyRegex.DynamicKeyRegex);
						var match = regex.Match(renderingReference.Placeholder);

						if (match.Success && match.Groups.Count > 0)
						{
							var parentRenderingId = "{" + key.Substring(key.Length - 36, 36).ToUpper() + "}";

							if (renderingReferences.All(r => r.UniqueId.ToUpper() != parentRenderingId))
							{
								RemovedRenderingReference(item, renderingReference.UniqueId);
							}
						}
					}
				}
			}
		}

		public void RemovedRenderingReference(Item item, string renderingReferenceUid)
		{
			var layoutFieldId = FieldIDs.LayoutField;
			var document = new XmlDocument();

			document.LoadXml(item[layoutFieldId]);

			var node = document.SelectSingleNode(string.Format("//r[@uid='{0}']", renderingReferenceUid));

			if (node != null && node.ParentNode != null)
			{
				node.ParentNode.RemoveChild(node);

				using (new EditContext(item))
				{
					item[layoutFieldId] = document.OuterXml;
				}
			}
		}
	}
}
