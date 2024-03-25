namespace BusinessLogic.Commons.Enums
{
    /// <summary>
    ///     This enum is used to specified 
    ///     the usage purpose of the given token.
    /// </summary>
    internal enum TokenPurpose
    {
        /// <summary>
        ///     Used for email confirmation purpose only.
        /// </summary>
        EmailConfirmation = 1,

        /// <summary>
        ///     Used for reset password purpose only.
        /// </summary>
        ResetPassword = 2,
    }
}
