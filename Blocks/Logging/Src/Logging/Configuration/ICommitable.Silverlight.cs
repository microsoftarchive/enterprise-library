namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Provides a way to apply batched changes made to an update context.
    /// </summary>
    /// <remarks>This service shpuld not be used directly from client code, and it's meant to be used by
    /// the <see cref="LogWriter"/> implementations internally.</remarks>
    internal interface ICommitable
    {
        /// <summary>
        /// Commits the changes.
        /// </summary>
        void Commit();
    }
}