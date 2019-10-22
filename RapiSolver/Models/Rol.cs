using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class Rol
    {
        
        public int RolId{get;set;}

        
        public string RolDescription{get;set;}

        public bool Publish{get;set;}

        public List<Usuario> Usuarios{get;set;}
    }
}