using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSVentas.Models.Request
{
    public class VentaRequest
    {
        [Required]
        [Range(1,Double.MaxValue, ErrorMessage = "El IdCliente debe ser mayor a 0")]
        [ExisteCliente(ErrorMessage = "El cliente no existe")]
        public int IdCliente { get; set; }
        public decimal Total { get; set; }

        [MinLength(1,ErrorMessage ="Debe existir al menos 1 concepto")]
        public List<Concepto> Conceptos { get; set; }

        public VentaRequest()
        {
            this.Conceptos = new List<Concepto>();
        }

    }

    public class Concepto
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Importe { get; set; }
        public int IdProducto { get; set; }
        public long IdVenta { get; set; }
    }

    #region Validacion
    public class ExisteClienteAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int idcliente = (int)value;
            using (var db = new VentaRealContext())
            {
                if (db.Clientes.Find(idcliente) == null) return false;
            }
            return true;
        }
    }
    #endregion
}
