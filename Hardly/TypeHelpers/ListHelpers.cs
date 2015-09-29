using System;

namespace Hardly {
    public static class ListHelpers {
        public static bool Contains<ItemAType, ItemBType>(this List<Tuple<ItemAType, ItemBType>> list, ItemAType item) {
            foreach(var listItem in list) {
                if(listItem.Item1.Equals(item)) {
                    return true;
                }
            }

            return false;
        }
    }
}
