using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class Location
    {
   
        public int LocationId{get;set;}

         
        public string Country{get;set;}

        
        public string City{get;set;}

        
        public string State{get;set;}

         
        public string Address{get;set;}

        List<Persona> People{get;set;}
    }
}