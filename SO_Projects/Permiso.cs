using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Permiso
{
    private static int ultimoPermisoId = 0;
    public int permisoId { get; set; }
    private DateTime fechaInicio { get; set; }
    private DateTime fechaFin { get; set; }
    private Casa casa { get; set; }
    private Usuario usuario { get; set; }

    public Permiso()
    {
        permisoId = ++ultimoPermisoId;
        fechaInicio = DateTime.Now;
        fechaFin = DateTime.Now;
        
        // TODO: Initialize Casa and Usuario. En teoria, no tendria por qué existir permiso sin referencia
        // casa = new Casa();
        // usuario = new Usuario();
    }

    public Permiso(DateTime fechaInicio, DateTime fechaFin, Casa casa, Usuario usuario)
    {
        this.permisoId = ++ultimoPermisoId;
        this.fechaInicio = fechaInicio;
        this.fechaFin = fechaFin;
        this.casa = casa;
        this.usuario = usuario;
    }

    public void agregarPermiso(Permiso permiso)
    {
        int CasaId = permiso.casa.CasaId; // Assuming Casa has a property Id
        int UsuarioId = permiso.usuario.UsuarioId; // Assuming Usuario has a property Id

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "permisos.txt");

        FileManipulation.VerifyFileLocation(filePath);

        string permisoInfo =
            $"{permiso.permisoId}\n{permiso.fechaInicio}\n{permiso.fechaFin}\n{CasaId}\n{UsuarioId}\n\n";
        FileManipulation.AppendLineToFile(filePath, permisoInfo);
    }

    public void MostrarPermisos()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "permisos.txt");

        if (File.Exists(filePath))
        {
            string[] permisos = File.ReadAllLines(filePath);
            for (int i = 0; i < permisos.Length; i += 6) // Assuming each permission entry has 6 lines
            {
                int permisoId = int.Parse(permisos[i]);
                DateTime fechaInicio = DateTime.Parse(permisos[i + 1]);
                DateTime fechaFin = DateTime.Parse(permisos[i + 2]);
                int casaId = int.Parse(permisos[i + 3]);
                int usuarioId = int.Parse(permisos[i + 4]);

                Casa? casa = Casa.ObtenerCasaPorId(casaId); // Method to get Casa by ID
                //TODO: To be implemented
                //Usuario usuario = ObtenerUsuarioPorId(usuarioId); // Method to get Usuario by ID

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

    public void EliminarPermiso(int permisoId)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "permisos.txt");

        if (File.Exists(filePath))
        {
            string[] permisos = File.ReadAllLines(filePath);
            List<string> updatedPermisos = new List<string>();

            for (int i = 0; i < permisos.Length; i += 6) // Assuming each permission entry has 6 lines
            {
                int currentPermisoId = int.Parse(permisos[i]);
                if (currentPermisoId != permisoId)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        updatedPermisos.Add(permisos[i + j]);
                    }
                }
            }

            File.WriteAllLines(filePath, updatedPermisos);
            Console.WriteLine($"Permiso con ID {permisoId} ha sido eliminado.");
        }
        else
        {
            Console.WriteLine("No hay permisos registrados.");
        }
    }
}