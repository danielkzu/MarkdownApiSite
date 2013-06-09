namespace ClariusLabs.Doq
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class ReaderFixture
    {
        private readonly string assemblyFile = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoSilverlight\bin\ClariusLabs.DemoSilverlight.dll").FullName;

        [Fact]
        public void when_reading_extension_method_then_provides_typed_member()
        {
            var typed = Reader.ReadMembers(Assembly.LoadFrom(assemblyFile))
                .OfType<ExtensionMethod>()
                .FirstOrDefault();

            Assert.NotNull(typed);
        }

        [Fact]
        public void when_reading_nested_type_then_provides_typed_member()
        {
            var typed = Reader.ReadMembers(Assembly.LoadFrom(assemblyFile))
                .OfType<NestedType>()
                .FirstOrDefault();

            Assert.NotNull(typed);
        }
    }
}