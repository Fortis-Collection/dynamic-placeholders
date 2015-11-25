using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.ExecutePageEditorAction;

namespace DynamicPlaceholders.Pipelines.ExecutePageEditorAction
{
	public class DynamicReplaceRendering : ReplaceRendering
	{
		protected override RenderingDefinition DoReplaceRendering(RenderingDefinition sourceRendering, Item targetRenderingItem, DeviceDefinition device)
		{
			Assert.ArgumentNotNull(sourceRendering, "sourceRendering");
			Assert.ArgumentNotNull(targetRenderingItem, "targetRenderingItem");
			Assert.ArgumentNotNull(device, "device");
			RenderingDefinition definition = new RenderingDefinition
			{
				UniqueId = sourceRendering.UniqueId,
				Cachable = sourceRendering.Cachable,
				Conditions = sourceRendering.Conditions,
				Datasource = sourceRendering.Datasource,
				ItemID = targetRenderingItem.ID.ToString(),
				MultiVariateTest = sourceRendering.MultiVariateTest,
				Parameters = sourceRendering.Parameters,
				Placeholder = sourceRendering.Placeholder,
				Rules = sourceRendering.Rules,
				VaryByData = sourceRendering.VaryByData,
				ClearOnIndexUpdate = sourceRendering.ClearOnIndexUpdate,
				VaryByDevice = sourceRendering.VaryByDevice,
				VaryByLogin = sourceRendering.VaryByLogin,
				VaryByParameters = sourceRendering.VaryByParameters,
				VaryByQueryString = sourceRendering.VaryByQueryString,
				VaryByUser = sourceRendering.VaryByUser
			};

			if (device.Renderings != null)
			{
				int index = device.Renderings.IndexOf(sourceRendering);
				device.Renderings.RemoveAt(index);
				device.Renderings.Insert(index, definition);
			}
			return definition;
		}
	}
}
