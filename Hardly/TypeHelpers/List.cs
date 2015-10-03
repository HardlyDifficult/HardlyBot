using System;
using System.Collections.Generic;
using System.Linq;

namespace Hardly {
    public class List<ItemType> {
        #region Constructor
        public List(ItemType[] items = null) {
            if(items != null) {
                this.items = new System.Collections.Generic.List<ItemType>(items);
            } else {
                this.items = new System.Collections.Generic.List<ItemType>();
            }
        }

        public List(List<ItemType> existingList) {
            if(existingList != null) {
                this.items = new System.Collections.Generic.List<ItemType>(existingList?.items);
            } else {
                this.items = new System.Collections.Generic.List<ItemType>();
            }
        }

        public List(ItemType existingItem) : this(new[] { existingItem }) {
        }
        #endregion

        #region Data
        System.Collections.Generic.List<ItemType> items;
        public ItemType this[uint i] {
            get {
                return items[(int)i];
            }
            set {
                items[(int)i] = value;
            }
        }
        public ItemType this[int i] {
            get {
                return items[i];
            }
            set {
                items[i] = value;
            }
        }

        public int Count {
            get {
                return items.Count;
            }
        }

        public bool IsEmpty {
            get {
                return items.Count == 0;
            }
        }

        public ItemType First {
            get {
                return Count > 0 ? items[0] : (ItemType)typeof(ItemType).GetDefaultValue();
            }
        }

        public ItemType Last {
            get {
                return Count > 0 ? items[Count - 1] : (ItemType)typeof(ItemType).GetDefaultValue();
            }
        }
        #endregion

        #region Public interface
        public void Add(ItemType item) {
            items.Add(item);
        }

        public void Add(IEnumerable<ItemType> itemsToAdd) {
            foreach(var item in itemsToAdd) {
                items.Add(item);
            }
        }

        public void Append(List<ItemType> itemsToAdd) {
            int itemLength = Count;
            for(int i = 0; i < itemLength; i++) {
                Add(itemsToAdd[i]);
            }
        }

        public void Clear() {
            items.Clear();
        }

        public bool Contains(ItemType item) {
            return items.Contains(item);
        }

        public void DuplicateEntities(uint numberOfTimes) {
            for(int i = 0; i < numberOfTimes; i++) {
                Append(this);
            }
        }

        /// <summary>
        /// This method enables foreach loops.  Never call directly.
        /// </summary>
        public System.Collections.Generic.List<ItemType>.Enumerator GetEnumerator() {
            return items.GetEnumerator();
        }

        public ItemType Pop() {
            if(!IsEmpty) {
                ItemType item = items[0];
                items.RemoveAt(0);
                return item;
            }

            return (ItemType)typeof(ItemType).GetDefaultValue();
        }

        public bool Remove(ItemType item) {
            return items.Remove(item);
        }

        public void Shuffle() {
            items = items.OrderBy(a => System.Guid.NewGuid()).ToList();
        }

        public void Sort() {
            items.Sort();
        }

        public override string ToString() {
            if(!IsEmpty) {
                string message = "";
                bool first = true;
                foreach(var item in items) {
                    if(!first) {
                        message += GetToStringListItemSeparator();
                    }
                    first = false;

                    message += item.ToString();
                }

                return message;
            }

            return null;
        }

        public uint IndexOf(ItemType item) {
            if(Contains(item)) {
                return (uint)items.IndexOf(item);
            } else {
                throw new Exception();
            }
        }
        #endregion

        #region Protected interface
        protected virtual string GetToStringListItemSeparator() {
            return "";
        }
        #endregion
    }
}
