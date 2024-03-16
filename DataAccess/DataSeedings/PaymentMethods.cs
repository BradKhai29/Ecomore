namespace DataAccess.DataSeedings
{
    public static class PaymentMethods
    {
        /// <summary>
        ///     Represent for the pending-to-confirm status.
        /// </summary>
        public static class OnlineBanking
        {
            public static readonly Guid Id = new("2d92a50b-6d0a-46f1-9f55-53de6b585600");

            public const string Name = "Online Banking";
        }

        /// <summary>
        ///     Represent for the email-confirmed-success status.
        /// </summary>
        public static class CashOnDelivery
        {
            public static readonly Guid Id = new("cc751bfc-77b9-4a97-85d4-c88e1f3db4de");

            public const string Name = "Cash On Delivery";
        }
    }
}
