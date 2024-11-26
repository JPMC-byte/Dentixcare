using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public interface ServicioPersona<T>
    {
        string guardar(T value);
        T iniciarSesion(string id, string Password);
        T obtenerPorCodigo(string ID);
        List<T> obtenerTodos();
    }
}
