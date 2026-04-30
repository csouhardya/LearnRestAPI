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
        public const string KafkaSeverKey = "Kafka:BootstrapServers";
        public const string ProduceOrderTopic = "Order";
        public const string ProductOrderMessageKey = "OrderCreated";
        public const string InventoryConsumerGroupId = "InventoryService";
        public const string EmailConsumerGroupId = "EmailService";
        #endregion

        #region EmailKeys
        public const string EmailAddressKey = "Email:Address";
        public const string EmailPasswordKey = "Email:Password";
        #endregion

        #region DefaultPropertyValues
        public const string DefaultCurrency = "INR";
        #endregion
    }
}
