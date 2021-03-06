﻿using IMD.Meditoc.CallCenter.Mx.Entities.Ordenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMD.Meditoc.CallCenter.Mx.Entities.Reportes.Ventas
{
    public class EntReporteOrden
    {
        public string uId { get; set; }
        public string sOrderId { get; set; }
        public int iIdEmpresa { get; set; }
        public string sNombre { get; set; }
        public string sCorreo { get; set; }
        public string sFolioEmpresa { get; set; }
        public double nAmount { get; set; }
        public double nAmountDiscount { get; set; }
        public double nAmountTax { get; set; }
        public double nAmountPaid { get; set; }
        public string sPaymentStatus { get; set; }
        public int iIdCupon { get; set; }
        public string sCodigo { get; set; }
        public int iIdOrigen { get; set; }
        public string sOrigen { get; set; }
        public EntCustomerInfo customer_info { get; set; }
        public List<EntReporteProducto> lstProductos { get; set; }
        public EntChargeReporte charges { get; set; }
        public string sRegisterDate { get; set; }
        public DateTime dtRegisterDate { get; set; }
    }
}
