namespace ClariusLabs.Doq
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class MemberIdMapFixture
    {
        private readonly string assemblyFile = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoSilverlight\bin\ClariusLabs.DemoSilverlight.dll").FullName;

        [Fact]
        public void when_adding_assembly_then_adds_all_ids_from_used_types()
        {
            var map = new MemberIdMap();
            map.Add(Assembly.LoadFrom(assemblyFile));

            map.idToMemberMap.ToList().ForEach(pair => Console.WriteLine("{0} : {1}", pair.Key, pair.Value));
        }
    }
}