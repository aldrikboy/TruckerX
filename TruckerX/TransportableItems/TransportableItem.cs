using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.TransportableItems
{
    public enum TransportableItemType
    {
        Normal,
        Special,
        Chemicals,
        Flatbed,
    }

    public class TransportableItem
    {
        public string Name { get; set; }
        public TransportableItemType Type { get; set; } = TransportableItemType.Normal;

        public static TransportableItem Bananas = new TransportableItem("Bananas");
        public static TransportableItem Peanuts = new TransportableItem("Peanuts");

        public TransportableItem(string name)
        {
            Name = name;
        }
    }
}
