using DAL;
using ENTITY;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicioOrtodoncista : ServicioPersona<Ortodoncista>
    {
        //RepositorioOrtodoncista RepsOrto;
        DBOrtodoncista RepsOrto;
        public ServicioOrtodoncista()
        {
            //RepsOrto = new RepositorioOrtodoncista(Config.FILENAME_ORTODONCISTA);
            RepsOrto = new DBOrtodoncista();
        }
        public string guardar(Ortodoncista value)
        {
            return RepsOrto.guardarDato(value);
        }
        public List<Ortodoncista> obtenerTodos()
        {
            return RepsOrto.obtenerTodos();
        }

        public Ortodoncista obtenerPorCodigo(string ID)
        {
            return RepsOrto.obtenerPorCodigo(ID);
        }

        public List<Ortodoncista> obtenerPorPrimerNombre(string PrimerNombre) 
        {
            return RepsOrto.obtenerPorNombre(PrimerNombre);
        
        }

        public Ortodoncista iniciarSesion(string id, string Password)
        {
            return RepsOrto.obtenerPorUsuario(id, Password);
        }
    }
}
