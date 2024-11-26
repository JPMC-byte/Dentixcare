using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicioConsultorio
    {
        //RepositorioConsultorio reposConsul;
        DBConsultorio reposConsul;

        public ServicioConsultorio()
        {
            //reposConsul = new RepositorioConsultorio(Config.FILENAME_CONSULTORIO);
            reposConsul = new DBConsultorio();
        }
        public Consultorio cargarConsultorio(string ID)
        {
            return reposConsul.obtenerPorCodigo(ID);
        }
        public List<Consultorio> obtenerTodos()
        {
            return reposConsul.obtenerTodos();
        }
    }
}
