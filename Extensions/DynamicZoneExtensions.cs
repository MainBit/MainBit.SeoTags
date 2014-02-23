using System.Collections.Generic;

namespace MainBit.Common.Extensions
{
    public static class DynamicZoneExtensions
    {
        public static void RemoveItemFrom(dynamic zone, string itemToDelete)
        {
            var itemsToDelete = new List<object>();

            foreach (var item in zone.Items)
            {
                if (item.Metadata.Type == itemToDelete)
                    itemsToDelete.Add(item);
            }

            foreach (var item in itemsToDelete)
            {
                zone.Items.Remove(item);
            }
        }

        public static void RemoveItemFrom(dynamic zone, string prefix, string type)
        {
            var itemsToDelete = new List<object>();

            foreach (var item in zone.Items)
            {
                if (item.Metadata.Type == type && item.Metadata.Prefix == prefix)
                    itemsToDelete.Add(item);
            }

            foreach (var item in itemsToDelete)
            {
                zone.Items.Remove(item);
            }
        }
    }
}