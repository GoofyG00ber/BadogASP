using System;
using System.Collections.Generic;

namespace BádogASP.Models;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public int? OrderId { get; set; }

    public string ShippingMethod { get; set; } = null!;

    public string? TrackingNumber { get; set; }

    public string? ShippingStatus { get; set; }

    public DateOnly? EstimatedDeliveryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }
}
