using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class ServicioPaciente : ServicioPersona<Paciente>
    {
        DBPaciente RepsPaciente;

        public ServicioPaciente()
        {
            //RepsPaciente = new RepositorioPaciente(Config.FILENAME_PACIENTE);
            RepsPaciente = new DBPaciente();
        }
        public string guardar(Paciente value)
        {
            return RepsPaciente.guardarDato(value);
        }

        public List<Paciente> obtenerTodos()
        {
            return RepsPaciente.obtenerTodos();
        }

        public Paciente obtenerPorCodigo(string ID)
        {
            return RepsPaciente.obtenerPorCodigo(ID);
        }

        public List<Paciente> cargarPorCodigo(string ID)
        {
            return RepsPaciente.cargarPorCodigo(ID);
        }

        public Paciente iniciarSesion(string id, string Password)
        {
            return RepsPaciente.obtenerPorUsuario(id, Password);
        }

        public string actualizarContrasena(string cedula, string nuevaContrasena)
        {
            return RepsPaciente.actualizarContrasena(cedula, nuevaContrasena);
        }
    }
}
