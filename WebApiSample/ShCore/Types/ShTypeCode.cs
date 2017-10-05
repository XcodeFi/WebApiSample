using System;
namespace ShCore.Types
{
    /// <summary>
    /// Type code do Sơn định nghĩa
    /// </summary>
    [Flags]
    public enum ShTypeCode
    {
        [ShTypeCodeOf(typeof(int))]
        Int32,        

        [ShTypeCodeOf(typeof(long))]
        Int64,        

        [ShTypeCodeOf(typeof(string))]
        String,        

        [ShTypeCodeOf(typeof(decimal))]
        Decimal,        

        [ShTypeCodeOf(typeof(bool))]
        Boolean,        

        [ShTypeCodeOf(typeof(DateTime))]
        DateTime,        

        [ShTypeCodeOf(typeof(byte))]
        Byte,        

        [ShTypeCodeOf(typeof(double))]
        Double,        

        [ShTypeCodeOf(typeof(Guid))]
        Guid,        

        [ShTypeCodeOf(typeof(short))]
        Int16,        

        [ShTypeCodeOf(typeof(float))]
        Single,        

        [ShTypeCodeOf(typeof(DBNull))]
        DBNull,

        UnKnown
    }

    /// <summary>
    /// Mở rộng phương thức cho ShTypeCode
    /// </summary>
    public static class ShTypeCodeExtender
    {
        /// <summary>
        /// Kiểm tra xem Type Code có hợp lệ, có nằm trong list Type Code cần check
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="typeCodeChecking"></param>
        /// <returns></returns>
        public static bool IsSet(this ShTypeCode typeCode, ShTypeCode typeCodeChecking)
        {
            return (typeCode & typeCodeChecking) == typeCodeChecking;
        }
    }
}
