﻿using System.Linq;

namespace Hardly {
    public class List<ItemType> {
        #region Constructor
        public List(ItemType[] items = null) {
            this.items = new System.Collections.Generic.List<ItemType>(items);
        }

        public List(List<ItemType> existingList) {
            this.items = new System.Collections.Generic.List<ItemType>(existingList?.items);
        }
        #endregion

        #region Data
        System.Collections.Generic.List<ItemType> items;

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
        #endregion

        #region Public interface
        public void Add(ItemType item) {
            items.Add(item);
        }

        public void Append(List<ItemType> itemsToAdd) {
            int itemLength = Count;
            for(int i = 0; i < itemLength; i++) {
                Add(itemsToAdd[i]);
            }
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

        public void Shuffle() {
            items = items.OrderBy(a => System.Guid.NewGuid()).ToList();
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
        #endregion

        #region Protected interface
        protected virtual string GetToStringListItemSeparator() {
            return "";
        }
        #endregion
    }
}