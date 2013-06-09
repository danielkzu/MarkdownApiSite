namespace ClariusLabs.Doq
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Container : Element
    {
        public Container(IEnumerable<Element> elements)
        {
            this.Elements = elements.ToList();
        }

        public IEnumerable<Element> Elements { get; private set; }
    }
}