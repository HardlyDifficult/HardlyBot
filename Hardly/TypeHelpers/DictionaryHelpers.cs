using System.Collections.Generic;

namespace Hardly {
    public static class DictionaryHelpers {
        public static bool Remove<KeyType, ListItemType>(this Dictionary<KeyType, List<ListItemType>> dictionary, ListItemType item) {
            foreach(var itemList in dictionary) {
                if(itemList.Value.Remove(item)) {
                    return true;
                }
            }

            return false;
        }
    }
}
