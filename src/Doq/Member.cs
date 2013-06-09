namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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
        public abstract MemberKinds Kind { get; }

        /// <summary>
        /// Gets the reflection information for this member, 
        /// if the reading process used an assembly.
        /// </summary>
        public MemberInfo Info { get; set; }

        public override string ToString()
        {
            return this.memberId;
        }
    }
}