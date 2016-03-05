using System;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Represents an entry in either back or forward navigation history.
    /// </summary>
    public class JournalEntry
    {
        /// <summary>
        /// Gets the type of the page that was navigated to.
        /// </summary>
        /// <value>
        /// The type of the page that was navigated to.
        /// </value>
        public Type SourcePageType { get; internal set; }

        /// <summary>
        /// Gets the parameter passed to the page when it was navigated to.
        /// </summary>
        /// <value>
        /// The parameter passed to the page when it was navigated to.
        /// </value>
        public object Parameter { get; internal set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var je = obj as JournalEntry;

            if (je == null)
            {
                return false;
            }

            bool ret = 
                this.SourcePageType.Equals(je.SourcePageType) &&
                ((this.Parameter == null && je.Parameter == null) ||
                 (this.Parameter.Equals(je.Parameter)));

            return ret;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 17;

            if (this.Parameter != null)
            {
                hash = hash * 23 + this.Parameter.GetHashCode();
            }
            else
            {
                hash = hash * 23;
            }

            hash = hash * 23 + this.SourcePageType.GetHashCode();

            return hash;
        }
    }
}