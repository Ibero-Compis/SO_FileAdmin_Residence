using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Permiso
{
    public int PermisoId { get; set; }
    private DateTime FechaInicio { get; set; }
    private DateTime FechaFin { get; set; }
    private Casa Casa { get; set; }
    private Usuario Usuario { get; set; }

    // Para mantener la referencia de la tura de los permisos
    public static string RutaArchivo =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "permisos", "permisos.txt");

    // Generar un nuevo ID único
    private static int GenerarNuevoId()
    {
        int nuevoId = 1;
        if (File.Exists(RutaArchivo))
        {
            string[] permisosInformacion = File.ReadAllLines(RutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < permisosInformacion.Length; i += 3) // Assuming each user entry has 2 lines + 1 empty line
            {
                if (!string.IsNullOrWhiteSpace(permisosInformacion[i])) // Check if the line is not empty
                {
                    ids.Add(int.Parse(permisosInformacion[i]));
                }
            }

            if (ids.Count > 0)
            {
                nuevoId = ids.Max() + 1;
            }
        }

        return nuevoId;
    }
    
    public Permiso()
    {
        PermisoId = GenerarNuevoId();
        FechaInicio = DateTime.Now;
        FechaFin = DateTime.Now;
        Casa = new Casa();
        Usuario = new Usuario();
    }

    public Permiso(DateTime FechaInicio, DateTime FechaFin, Casa Casa, Usuario Usuario)
    {
        this.PermisoId = GenerarNuevoId();
        this.FechaInicio = FechaInicio;
        this.FechaFin = FechaFin;
        this.Casa = Casa;
        this.Usuario = Usuario;
    }

    public void AgregarPermiso(Permiso permiso)
    {
        // Obtener casa Id y Usuario Id
        int CasaId = permiso.Casa.CasaId;
        int UsuarioId = permiso.Usuario.UsuarioId;

        try
        {
            // Verificar si el archivo existe
            FileManipulation.VerifyFileLocation(RutaArchivo);

            // Parasear la informacion del permiso
            string permisoInfo =
                $"{permiso.PermisoId}\n{permiso.FechaInicio}\n{permiso.FechaFin}\n{CasaId}\n{UsuarioId}\n"; // Ensure single newline at the end
            FileManipulation.AppendLineToFile(RutaArchivo, permisoInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al agregar el permiso:  " + e.Message);
            throw;
        }
    }

    public void MostrarPermisos()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "db_data", "permisos", "permisos.txt");

        try
        {
            if (File.Exists(filePath))
            {
                string[] permisos = File.ReadAllLines(filePath);
                for (int i = 0; i < permisos.Length; i += 6) // Adjusted to handle the format correctly
                {
                    if (string.IsNullOrWhiteSpace(permisos[i])) // Skip empty lines
                    {
                        continue;
                    }

                    int permisoId = int.Parse(permisos[i]);
                    DateTime fechaInicio = DateTime.Parse(permisos[i + 1]);
                    DateTime fechaFin = DateTime.Parse(permisos[i + 2]);
                    int casaId = int.Parse(permisos[i + 3]);
                    int usuarioId = int.Parse(permisos[i + 4]);

                    Casa? casa = Casa.ObtenerCasaPorId(casaId); // Method to get Casa by ID
                    Usuario usuario = Usuario.BuscarUsuarioPorId(usuarioId); // Method to get Usuario by ID

                    Console.WriteLine($"Permiso ID: {permisoId}");
                    Console.WriteLine($"Fecha Inicio: {fechaInicio}");
                    Console.WriteLine($"Fecha Fin: {fechaFin}");
                    Console.WriteLine($"Casa Numero: {casa.NumeroCasa}");
                    Console.WriteLine($"Usuario Nombre: {usuario.Nombre}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No hay permisos registrados.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al mostrar permisos: " + e.Message);
        }
    }
    
    public void EliminarPermiso(int permisoId)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "db_data", "permisos", "permisos.txt");

        if (File.Exists(filePath))
        {
            string[] permisos = File.ReadAllLines(filePath);
            List<string> updatedPermisos = new List<string>();

            for (int i = 0; i < permisos.Length; i += 6) // Assuming each permission entry has 6 lines
            {
                if (string.IsNullOrWhiteSpace(permisos[i])) // Skip empty lines
                {
                    continue;
                }

                int currentPermisoId;
                if (int.TryParse(permisos[i], out currentPermisoId) && currentPermisoId != permisoId)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (i + j < permisos.Length)
                        {
                            updatedPermisos.Add(permisos[i + j]);
                        }
                    }
                    updatedPermisos.Add(""); // Add a single empty line after each permission entry
                }
            }

            // Ensure the updated permissions list maintains the correct format
            string formattedPermisos = string.Join(Environment.NewLine, updatedPermisos);
            formattedPermisos = formattedPermisos.TrimEnd() + "\n\n"; // Ensure exactly two empty lines at the end
            File.WriteAllText(filePath, formattedPermisos);
            Console.WriteLine($"Permiso con ID {permisoId} ha sido eliminado.");
        }
        else
        {
            Console.WriteLine("No hay permisos registrados.");
        }
    }
}