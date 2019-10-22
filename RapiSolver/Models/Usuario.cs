using System.Collections.Generic;

namespace RapiSolver.Models
{
    public class Usuario
    {
        
        public int UsuarioId{get;set;}

       
        public string UserName{get;set;}

        
        public string UserPassword{get;set;}

        public Rol Rol{get;set;}

        public int RolId{get;set;}
    }
}