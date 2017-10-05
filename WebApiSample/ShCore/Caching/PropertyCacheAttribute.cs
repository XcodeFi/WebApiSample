using ShCore.Attributes;
namespace ShCore.Caching
{
    /// <summary>
    /// Attribute cho Property của CacheEntry
    /// </summary>
    public abstract class PropertyCacheAttribute : PropertyInfoAttribute
    {   
        /// <summary>
        /// Name dùng để Notify các cache liên quan
        /// </summary>
        public abstract string TargetName
        {
            get;
        }        

        /// <summary>
        /// Value được chứa khi cần gọi phương thức clear cache theo giá trị gì
        /// </summary>
        public object Value { set; get; }
    }
}
