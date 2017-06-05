// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.EntityFrameworkCore.Metadata
{
    public interface IMutableSequence : ISequence
    {
        new long StartValue { get; set; }
        new int IncrementBy { get; set; }
        new long? MinValue { get; set; }
        new long? MaxValue { get; set; }
        new Type ClrType { get; set; }
        new bool IsCyclic { get; set; }
    }
}
