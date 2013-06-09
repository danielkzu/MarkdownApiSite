namespace ClariusLabs.Doq
{
    using System;
    using System.Linq;

    /// <summary>
    /// Kind (or kinds for semantically-augmented readings) 
    /// of member node.
    /// </summary>
    [Flags]
    public enum MemberKinds
    {
        Unknown = 0,
        Type = 1,
        Field = 2,
        Property = 4,
        Method = 8,
        Event = 16,
        // Here start the extended member types 
        // available only when you pass in an 
        // assembly for the reading.
        ExtensionMethod = 32,
        NestedType = 64,
        Enum = 128,
        Interface = 256,
        Class = 512,
        Struct = 1024,
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