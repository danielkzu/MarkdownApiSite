namespace ClariusLabs.Demo
{
    using System;

    /// <summary>
    /// Extension class for <see cref="Sample"/>.
    /// </summary>
    public static class SampleExtensions
    {
        /// <summary>
        /// Summary of an extension method for <see cref="Sample"/>.
        /// </summary>
        /// <param name="sample" this="true">The sample to do on.</param>
        public static void Do(this Sample sample)
        {
        }
    }

    /// <summary>
    /// Sample API documentation. This is the summary.
    /// With every line we will have the same 
    /// issue. This should all go to a single 
    /// non-breaking line.
    /// </summary>
    /// <example>
    /// What follows is an example:
    /// <code>
    /// var code = new ThisIsCode();
    /// </code>
    /// And this is an inline <c>c tag</c> within an example.
    /// </example>
    /// <remarks>
    /// This is the remarks section, which can also have <c>c tag</c> code.
    /// <code>
    /// var code = new SomeCodeTagWithinRemarks();
    /// </code>
    /// You can use <see cref="Provider"/> see tag within sections.
    /// <para>
    /// We can have paragraphs anywhere.
    /// </para>
    /// </remarks>
    /// <seealso cref="Provider"/>
    public class Sample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        public Sample()
        {
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id of this sample.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets the value for the given id.
        /// </summary>
        /// <param name="id">The id to get the value for.</param>
        /// <returns><see langword="true"/> if the value <c>true</c> (with c tag); <see langword="false"/> otherwise.</returns>
        public bool GetValue(int id)
        {
            return false;
        }

        /// <summary>
        /// A nested type
        /// </summary>
        public class NestedType
        {
            /// <summary>
            /// Gets or sets the nested type property.
            /// </summary>
            public int NestedTypeProperty { get; set; }
        }
    }
}