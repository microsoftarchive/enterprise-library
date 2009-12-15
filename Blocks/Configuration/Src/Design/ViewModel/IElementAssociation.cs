namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Denotes an associated with another element.
    /// </summary>
    public interface IElementAssociation
    {
        /// <summary>
        /// When implemented, returns the associated <see cref="ElementViewModel"/>
        /// </summary>
        ElementViewModel AssociatedElement { get; }

        /// <summary>
        /// When implemented, returns an element name that may differ from <see cref="ElementViewModel.Name"/>
        /// </summary>
        string ElementName { get; }
    }
}