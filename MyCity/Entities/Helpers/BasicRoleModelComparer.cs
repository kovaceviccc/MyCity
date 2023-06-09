using Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Helpers;

internal class BasicRoleModelComparer : IEqualityComparer<BasicRoleModel>
{
    public bool Equals(BasicRoleModel? x, BasicRoleModel? y)
    {
        if (x == null || y == null) return false;

        return x.Id == y.Id;
    }

    public int GetHashCode([DisallowNull] BasicRoleModel obj)
    {
        return obj.Id.GetHashCode();
    }
}
