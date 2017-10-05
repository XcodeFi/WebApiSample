using System.Collections.Generic;
using System;
using System.Linq;
namespace ShCore.ShFlags
{
    [Serializable]
    public class Flag<T> : FlagsBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Flag() { }

        /// <summary>
        /// Flag khởi tạo giá trị mặc định
        /// </summary>
        /// <param name="bitTrues"></param>
        public Flag(params T[] bitTrues) : this()
        {
            for (int i = 0; i < bitTrues.Length; i++)
                this[bitTrues[i]] = true;
        }

        private static Dictionary<T, BitAttribute> dic = null;
        /// <summary>
        /// 
        /// </summary>
        protected static Dictionary<T, BitAttribute> Dic
        {
            get
            {
                if (dic == null)
                {
                    dic = typeof(T).GetFields().Select(f => new { f, At = f.GetCustomAttributes(typeof(BitAttribute), true) }).
                        Where(item => item.At.Length > 0).
                        ToDictionary(item => (T)item.f.GetRawConstantValue(), item => item.At[0] as BitAttribute);
                }
                return dic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static BitAttribute Get(T t)
        {
            BitAttribute ea = null;
            Dic.TryGetValue(t, out ea);
            return ea;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool this[T t]
        {
            get
            {
                var ea = Get(t);
                if (ea == null) return false;
                return this[ea.Bit];
            }
            set
            {
                var ea = Get(t);
                if (ea == null) return;
                this[ea.Bit] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static implicit operator Flag<T>(int i)
        {
            return new Flag<T> { BitValue = i };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static implicit operator long(Flag<T> flag)
        {
            return flag.BitValue;
        }

        /// <summary>
        /// Tạo mới một FlagT
        /// </summary>
        /// <param name="bitTrues"></param>
        /// <returns></returns>
        public static Flag<T> New(params T[] bitTrues)
        {
            return new Flag<T>(bitTrues);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BitAttribute : Attribute
    {
        private int thisBit = 0;
        public BitAttribute(int thisBit)
        {
            this.thisBit = thisBit;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Bit 
        {
            get { return thisBit; }
        }

        public string Name { set; get; }
    }
}
