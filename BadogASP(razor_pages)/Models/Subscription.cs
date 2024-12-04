using System;
using System.Collections.Generic;

namespace BadogASP_razor_pages_.Models;

public partial class Subscription
{
    public long Id { get; set; }

    public Guid SubscriptionId { get; set; }

    public string Claims { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
