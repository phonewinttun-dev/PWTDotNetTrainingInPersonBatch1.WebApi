using System;
using System.Collections.Generic;

namespace PWTDotNetTrainingInPersonBatch1.WebApi.Database.AppDbContextModels;

public partial class TblSaleDetail
{
    public int SaleDetailId { get; set; }

    public int SaleId { get; set; }

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual TblProduct Product { get; set; } = null!;

    public virtual TblSale Sale { get; set; } = null!;
}
