namespace ShCore.Caching
{
    /// <summary>
    /// PropertyCacheOptional để lập trình thoải mái tạo TargetName
    /// </summary>
    public class PropertyCacheOptionalAttribute : PropertyCacheAttribute
    {
        private string targetName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public override string TargetName
        {
            get 
            {
                return targetName;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetName"></param>
        public PropertyCacheOptionalAttribute(string targetName)
        {
            this.targetName = targetName;
        }
    }
}
