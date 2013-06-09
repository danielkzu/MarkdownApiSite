namespace ClariusLabs.Doq
{
    using System;
    using System.Linq;

    public enum MemberKind
    {
        Unknown,
        Type,
        Field,
        Property,
        Method,
        Event,
        // Here start the extended member types 
        // available only when you pass in an 
        // assembly for the reading.
        ExtensionMethod,
        NestedType,
        Enum,
        Interface,
        Class,
        Struct,
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