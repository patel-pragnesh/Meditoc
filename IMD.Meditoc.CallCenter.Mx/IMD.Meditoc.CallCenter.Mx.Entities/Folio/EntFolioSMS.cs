﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMD.Meditoc.CallCenter.Mx.Entities.Folio
{
    public class EntFolioSMS
    {
        public string sFolio { get; set; }
        public string sPassword { get; set; }
        public DateTime? dtFechaVencimiento { get; set; }
        public string sFechaVencimiento { get; set; }
        public bool bReprocesado { get; set; }
    }
}
