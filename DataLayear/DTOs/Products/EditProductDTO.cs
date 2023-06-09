using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Products
{
    public class EditProductDTO : CreateProductDTO
    { 
    public long Id { get; set; }

    public string ImageName { get; set; }
    }
}

public enum EditProductResult
{
    NotFound,
    NotForUser,
    Success
}