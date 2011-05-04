#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Cqrs
{
    public enum MessageAttributeTypeContract : uint
    {
        Undefined = 0,
        ContractName = 1,
        ContractDefinition = 2,
        BinaryBody = 3,
        CustomString = 4,
        CustomBinary = 5,
        CustomNumber = 6,
        Topic = 7,
        Sender = 8,
        Identity = 9,
        CreatedUtc = 10,
        StorageReference = 11,
        StorageContainer = 12,
        ErrorText = 13,
        Thread = 14,
        EntityIdentityString = 20
    }
}