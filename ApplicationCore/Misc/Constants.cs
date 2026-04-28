using System.Security.Cryptography;

namespace ApplicationCore.Misc
{
    public static class Constants
    {
        #region CacheKeys
        public const string AllProductCacheKey = "all_products";
        public const string AllOrdersCacheKey = "all_orders";
        #endregion

        #region HashingConstants
        public const int saltSize = 16;
        public const int hashSize = 32;
        public const int iterations = 100000;
        #endregion

        #region DbExceptions
        public const int EmailAlreadyExists = 50001;
        public const int UsernameAlreadyExists = 50002;
        #endregion

        #region KafkaConstants
        public const string KafkaSever = "localhost:9092"; // TODO get it from configuration, env vars
        public const string ProduceOrderTopic = "Order";
        #endregion
    }
}
