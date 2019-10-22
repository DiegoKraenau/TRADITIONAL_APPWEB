using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class Supplier: Persona
    {
          public int SupplierId{get;set;}

          public  IEnumerable<ServiceDetails> ServiciosDetails{get;set;}
    }
}