using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Casa
{
    // Atributos
    public int CasaId { get; set; }
    public int NumeroCasa { get; set; }
    public string Direccion { get; set; }
    public ICollection<Usuario> Habitantes { get; set; }
    public static string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "casas", "casas.txt");

    // Constructor
    public Casa(int numeroCasa, string direccion)
    {
        CasaId = GenerarNuevoId();
        NumeroCasa = numeroCasa;
        Direccion = direccion;
        Habitantes = new List<Usuario>(); // Inicializamos la lista de habitantes vacía
    }

    // Generar un nuevo ID único
    private static int GenerarNuevoId()
    {
        int nuevoId = 1;
        if (File.Exists(rutaArchivo))
        {
            string[] casasData = File.ReadAllLines(rutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < casasData.Length; i += 5) // Assuming each casa entry has 5 lines
            {
                ids.Add(int.Parse(casasData[i]));
            }
            if (ids.Count > 0)
            {
                nuevoId = ids.Max() + 1;
            }
        }
        return nuevoId;
    }

    // Verificar si existe el archivo principal de la clase
    public bool VerificarRutaArchivo()
    {
        return File.Exists(rutaArchivo);
    }

    // Crear el archivo si no existe
    public void CrearArchivoPrincipal()
    {
        if (!VerificarRutaArchivo())
        {
            using (File.Create(rutaArchivo))
            {
                Console.WriteLine("Archivo de casas creado exitosamente.");
            }

            return;
        }

        Console.WriteLine("El archivo de casas ya existe.");
    }

    public void AgregarCasa(Casa casa)
    {
        int casaId = casa.CasaId;
        int numeroCasa = casa.NumeroCasa;
        string direccional = casa.Direccion;
        string habitantesIds = string.Join(",", casa.Habitantes.Select(h => h.UsuarioId));

        try
        {
            string filePath = rutaArchivo;

            FileManipulation.VerifyFileLocation(filePath);

            string casaInfo =
                $"{casaId}\n{numeroCasa}\n{direccional}\n{habitantesIds}\n\n";
            FileManipulation.AppendLineToFile(filePath, casaInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al agregar la casa:  " + e.Message);
            throw;
        }
    }

    public static List<Casa> ObtenerCasas()
    {
        List<Casa> casas = new List<Casa>();
        string filePath = rutaArchivo;

        if (File.Exists(filePath))
        {
            string[] casasData = File.ReadAllLines(filePath);
            for (int i = 0; i < casasData.Length; i += 5) // Assuming each casa entry has 5 lines
            {
                int casaId = int.Parse(casasData[i]);
                int numeroCasa = int.Parse(casasData[i + 1]);
                string direccion = casasData[i + 2];
                string[] habitantesIds = casasData[i + 3].Split(',');
                List<Usuario> habitantes = new List<Usuario>();
                foreach (string habitanteId in habitantesIds)
                {
                    Usuario habitante = Usuario.ObtenerUsuarioPorId(int.Parse(habitanteId));
                    habitantes.Add(habitante);
                }

                Casa casa = new Casa(numeroCasa, direccion)
                {
                    CasaId = casaId,
                    Habitantes = habitantes
                };
                casas.Add(casa);
            }
        }

        return casas;
    }

    public static Casa? ObtenerCasaPorId(int casaId)
    {
        List<Casa> casas = ObtenerCasas();
        return casas.FirstOrDefault(c => c.CasaId == casaId);
    }

    public void MostrarCasas()
    {
        List<Casa> casas = ObtenerCasas();
        foreach (Casa casa in casas)
        {
            Console.WriteLine($"Casa ID: {casa.CasaId}");
            Console.WriteLine($"Número de Casa: {casa.NumeroCasa}");
            Console.WriteLine($"Dirección: {casa.Direccion}");
            Console.WriteLine("Habitantes:");
            foreach (Usuario habitante in casa.Habitantes)
            {
                Console.WriteLine($"- {habitante.Nombre}");
            }

            Console.WriteLine();
        }
    }

    public void EliminarCasa(int casaId)
    {
        string filePath = rutaArchivo;

        if (File.Exists(filePath))
        {
            string[] casasData = File.ReadAllLines(filePath);
            List<string> updatedCasas = new List<string>();

            for (int i = 0; i < casasData.Length; i += 5) // Assuming each casa entry has 5 lines
            {
                int currentCasaId = int.Parse(casasData[i]);
                if (currentCasaId != casaId)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        updatedCasas.Add(casasData[i + j]);
                    }
                }
            }

            File.WriteAllLines(filePath, updatedCasas);
            Console.WriteLine("Casa eliminada exitosamente.");
        }
        else
        {
            Console.WriteLine("No hay casas registradas.");
        }
    }

    public void AgregarHabitante(int casaId, int usuarioId)
    {
        Casa? casa = ObtenerCasaPorId(casaId);
        if (casa != null)
        {
            Usuario? usuario = Usuario.ObtenerUsuarioPorId(usuarioId);
            if (usuario != null)
            {
                casa.Habitantes.Add(usuario);
                EliminarCasa(casaId);
                AgregarCasa(casa);
                Console.WriteLine("Habitante agregado exitosamente.");
            }
            else
            {
                Console.WriteLine("El usuario no existe.");
            }
        }
        else
        {
            Console.WriteLine("La casa no existe.");
        }
    }

    public void EliminarHabitante(int casaId, int usuarioId)
    {
        Casa? casa = ObtenerCasaPorId(casaId);
        if (casa != null)
        {
            Usuario? usuario = Usuario.ObtenerUsuarioPorId(usuarioId);
            if (usuario != null)
            {
                casa.Habitantes.Remove(usuario);
                EliminarCasa(casaId);
                AgregarCasa(casa);
                Console.WriteLine("Habitante eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("El usuario no existe.");
            }
        }
        else
        {
            Console.WriteLine("La casa no existe.");
        }
    }

    public void MostrarHabitantes(int casaId)
    {
        Casa? casa = ObtenerCasaPorId(casaId);
        if (casa != null)
        {
            Console.WriteLine($"Habitantes de la Casa {casa.NumeroCasa}:");
            foreach (Usuario habitante in casa.Habitantes)
            {
                Console.WriteLine($"- {habitante.Nombre}");
            }
        }
        else
        {
            Console.WriteLine("La casa no existe.");
        }
    }

    public void MostrarHabitantesDeTodasLasCasas()
    {
        List<Casa> casas = ObtenerCasas();
        foreach (Casa casa in casas)
        {
            Console.WriteLine($"Habitantes de la Casa {casa.NumeroCasa}:");
            foreach (Usuario habitante in casa.Habitantes)
            {
                Console.WriteLine($"- {habitante.Nombre}");
            }

            Console.WriteLine();
        }
    }
}