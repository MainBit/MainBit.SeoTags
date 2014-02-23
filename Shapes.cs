using Orchard;
using Orchard.Core.Containers.Models;
using Orchard.DisplayManagement.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Common
{
    public class Shapes : IShapeTableProvider 
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public Shapes(IWorkContextAccessor workContextAccessor)
		{
			_workContextAccessor = workContextAccessor;
		}

		#region Implementation of IShapeTableProvider

		public void Discover(ShapeTableBuilder builder)
		{
            builder.Describe("Parts_Container_Contained")
				.OnDisplaying(displaying =>
								{
                                    var part = displaying.Shape.ContentPart as ContainerPart;
                                    if (part != null)
									{
                                        if (!String.IsNullOrWhiteSpace(part.ItemContentType))
                                        {
                                            displaying.ShapeMetadata.Alternates.Add("Parts_Container_Contained_" + part.ItemContentType);
                                            displaying.ShapeMetadata.Alternates.Add("Parts_Container_Contained_" + part.ItemContentType + "__" + displaying.ShapeMetadata.DisplayType);
										}
									}
								});
            //это почему-то не работает
            builder.Describe("Style")
                .OnDisplaying(displaying =>
                {
                    var resource = displaying.Shape;
                    resource.url = resource.Url.ToLower();
                });
            builder.Describe("Style__site")
                .OnDisplaying(displaying =>
                {
                    var resource = displaying.Shape;
                    resource.Url = resource.Url.ToLower();
                });
            builder.Describe("Style_ie")
                .OnDisplaying(displaying =>
                {
                    var resource = displaying.Shape;
                    resource.Url = resource.Url.ToLower();
                });

		}

		#endregion
	}
}