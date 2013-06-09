namespace ClariusLabs.Doq
{
    using System.Collections.Generic;

    public class Summary : Container
    {
        public Summary(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.VisitSummary(this);
        }
    }
}