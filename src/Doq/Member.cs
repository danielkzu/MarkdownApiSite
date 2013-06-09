namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Member : Container
    {
        private readonly string memberId;

        public Member(string memberId, IEnumerable<Element> elements)
            : base(elements)
        {
            this.memberId = memberId;
            this.Id = memberId.Substring(2);
        }

        public string Id { get; private set; }
        public abstract MemberKind Kind { get; }

        public override string ToString()
        {
            return this.memberId;
        }
    }
}