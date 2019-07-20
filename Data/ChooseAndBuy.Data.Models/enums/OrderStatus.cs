namespace ChooseAndBuy.Data.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum OrderStatus
    {
        Pending = 1,
        DeliveryInProgress = 2,
        Delivered = 3,
        Canceled = 4,
        Returned = 5,
    }
}
