namespace Markdown.ApiSite
{
    using System;
    using System.Linq;

    public enum MemberType
    {
        Unknown,
        Type,
        Field,
        Property,
        Method,
        Event,
        /*
T
type: class, interface, struct, enum, delegate
D
typedef
F
field
P
property (including indexers or other indexed properties)
M
method (including such special methods as constructors, operators, and so forth)
E
event         
        */
    }
}