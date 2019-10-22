using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class Servicio
    {
       
        public int ServicioId{get;set;}

       
        public string Name{get;set;}

       
        public string Description{get;set;}

        
        public string Cost{get;set;}

        public ServiceCategory ServiceCategory{get;set;}

        public int ServiceCategoryId{get;set;}
       
    }
}