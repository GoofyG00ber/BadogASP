﻿using System;
using System.Collections.Generic;

namespace BádogASP.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
