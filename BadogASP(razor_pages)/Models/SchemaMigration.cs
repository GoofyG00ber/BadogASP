using System;
using System.Collections.Generic;

namespace BadogASP_razor_pages_.Models;

public partial class SchemaMigration
{
    public long Version { get; set; }

    public DateTime? InsertedAt { get; set; }
}
