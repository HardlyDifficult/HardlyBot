using System;
using System.Collections.Generic;
using System.Linq;

namespace Hardly {
	public static class ArrayHelpers {
        public static T[] Append<T>(this T[] a, T[] b) {
            if(a != null && b != null) {
                try {
                    int length = a.Length + b.Length;
                    T[] merged = new T[length];
                    int i = 0;
                    foreach(T listItem in a) {
                        merged[i++] = listItem;
                    }
                    foreach(T listItem in b) {
                        merged[i++] = listItem;
                    }

                    Debug.Assert(i == length);

                    return merged;
                } catch(Exception e) {
                    Log.error("Array helper merge failed", e);
                }
            } else if(a != null) {
                return a;
            } else if(b != null) {
                return b;
            }

            Debug.Fail();
            return null;
        }

        public static T[] Append<T>(this T[] a, T b) {
            if(a != null && b != null) {
                try {
                    int length = a.Length + 1;
                    T[] merged = new T[length];
                    int i = 0;
                    foreach(T listItem in a) {
                        merged[i++] = listItem;
                    }
                    merged[i++] = b;

                    Debug.Assert(i == length);

                    return merged;
                } catch(Exception e) {
                    Log.error("Array helper merge failed", e);
                }
            } else if(a != null) {
                return a;
            } else if(b != null) {
                return new[] { b };
            }

            Debug.Fail();
            return null;
        }

        public static bool Contains<T>(this T[] data, T item) {
			foreach(T dataItem in data) {
				if(dataItem == null) {
					if(item == null) {
						return true;
					}
				} else if(dataItem.Equals(item)) {
					return true;
				}
			}

			return false;
		}

		public static T[] DuplicateEntities<T>(this T[] data, uint numberOfTimes) {
			T[] finalData = data;
			if(numberOfTimes > 0) {
				for(int i = 0; i < numberOfTimes; i++) {
					finalData = finalData.Append(data);
				}
			}

			return finalData;
		}

        public static T[] Shuffle<T>(this T[] list) {
            return list.OrderBy(a => Guid.NewGuid()).ToArray();
        }

        public static T[] SubArray<T>(this T[] data, uint index, uint length = 0) {
			if(data != null && data.Length >= 0) {
				try {
					if(length == 0 || index + length > data.Length) {
						length = (uint)data.Length - index;
					}

					T[] result = new T[length];
					Array.Copy(data, index, result, 0, length);
					return result;
				} catch(Exception e) {
					Log.error("Sub-array helper failed", e);
				}
			} else {
				Debug.Fail();
			}

			return null;
		}


		public static T[] ToArray<T>(this IEnumerable<T> list) {
			if(list != null) {
				T[] results = System.Linq.Enumerable.ToArray<T>(list);
				if(results != null && results.Length > 0) {
					return results;
				}
			}

			return null;
		}
	}
}