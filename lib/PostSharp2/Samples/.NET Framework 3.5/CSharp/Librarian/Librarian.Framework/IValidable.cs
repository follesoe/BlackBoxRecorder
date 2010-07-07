namespace Librarian.Framework
{
    /// <summary>
    /// Exposes a <see cref="Validate"/> method, which should validate the current instance
    /// and throw a <see cref="ValidationException"/> in case of validation error.
    /// </summary>
    public interface IValidable
    {
        /// <summary>
        /// Validate the current instance.
        /// </summary>
        /// <exception cref="ValidationException">The current instance is not valid.</exception>
        void Validate();
    }
}