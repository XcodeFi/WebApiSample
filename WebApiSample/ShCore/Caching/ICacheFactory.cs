namespace ShCore.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// 
        /// </summary>
        ICacheProvider Provider { get; }
    }
}
