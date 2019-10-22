using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class ServiceCategory
    {
         public int ServiceCategoryId{get;set;}

      
        public string CategoryName{get;set;}

        
        public string CategoryDescription{get;set;}

        public List<Servicio> Servicios{get;set;}
    }
}