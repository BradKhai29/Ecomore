namespace DataAccess.DataSeedings
{
    public static class OrderStatuses
    {
        /// <summary>
        ///     Represent for the pending-to-process status.
        /// </summary>
        /// <remarks>
        ///     The order with this status is already received by the seller,
        ///     but not processed.
        /// </remarks>
        public static class Pending
        {
            public static readonly Guid Id = new("2d92a50b-6d0a-46f1-9f55-53de6b585600");

            public const string Name = "Pending";
        }

        /// <summary>
        ///     Represent for the in-processing status of the order.
        /// </summary>
        /// <remarks>
        ///     The order with this status is being processed by the seller.
        /// </remarks>
        public static class InProcessing
        {
            public static readonly Guid Id = new("cc751bfc-77b9-4a97-85d4-c88e1f3db4de");

            public const string Name = "In Processing";
        }

        /// <summary>
        ///     Represent for the done status of the order.
        /// </summary>
        /// <remarks>
        ///     The order with this status is processed successfully by the seller,
        ///     and on the way to deliver.
        /// </remarks>
        public static class Done
        {
            public static readonly Guid Id = new("c4cc80c2-c5b1-4852-8f32-f59c6d5b2213");

            public const string Name = "Done";
        }

        /// <summary>
        ///     Represent for the cancelled status of the order.
        /// </summary>
        /// <remarks>
        ///     The order with this status is cancelled by the customer.
        /// </remarks>
        public static class Cancelled
        {
            public static readonly Guid Id = new("3120575b-9f22-4330-9f73-8ac89ba3a15c");

            public const string Name = "Cancelled";
        }
    }
}
