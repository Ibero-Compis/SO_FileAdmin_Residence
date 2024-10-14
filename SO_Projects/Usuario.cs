using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public int Edad { get; set; }
    public Role Rol { get; set; }

    public static string RutaArchivo =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "usuarios.txt");

    public ICollection<Permiso> Permisos { get; set; }

    //Constructor de la clase Usuario
    public Usuario(int usuarioId, string nombre, string email, int edad, Role rol)
    {
        this.UsuarioId = usuarioId;
        this.Nombre = nombre;
        this.Email = email;
        this.Edad = edad;
        this.Rol = rol;
    }

    public Usuario()
    {
        UsuarioId = GenerarNuevoId();
        Nombre = "";
        Email = "";
        Edad = 0;
        Rol = new Role(1, "Residente");
    }

    private static int GenerarNuevoId()
    {
        int nuevoId = 1;
        if (File.Exists(RutaArchivo)){
            string[] usersData = File.ReadAllLines(RutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < usersData.Length; i += 5) // Assuming each casa entry has 5 lines
            {
                ids.Add(int.Parse(usersData[i]));
            }

            if (ids.Count > 0){
                nuevoId = ids.Max() + 1;
            }
        }

        return nuevoId;
    }

    // Verificar si existe el archivo principal de la clase
    public bool VerificarRutaArchivo()
    {
        return File.Exists(RutaArchivo);
    }

    public void CrearArchivoUsuario(Usuario usuario)
    {
        string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, usuario.Nombre);
        Directory.CreateDirectory(userFolder);

        string userFilePath = Path.Combine(userFolder, $"{usuario.UsuarioId}_{usuario.Nombre}.txt");
        using (StreamWriter sw = File.CreateText(userFilePath)){
            sw.WriteLine($"UsuarioId: {usuario.UsuarioId}");
            sw.WriteLine($"Nombre: {usuario.Nombre}");
            sw.WriteLine($"Email: {usuario.Email}");
            sw.WriteLine($"Edad: {usuario.Edad}");
            sw.WriteLine($"Rol: {usuario.Rol.RoleName}");
        }
    }

    public void AgregarUsuario(Usuario usuario)
    {
        try{
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Add the user's name to the file
            File.AppendAllText(Usuario.RutaArchivo, usuario.Nombre + Environment.NewLine);

            // Read all lines, sort them, and write them back
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();
            lines.Sort();
            File.WriteAllLines(Usuario.RutaArchivo, lines);

            // Create the user's folder and file
            CrearArchivoUsuario(usuario);
        }
        catch (Exception e){
            Console.WriteLine("Error al agregar el usuario: " + e.Message);
            throw;
        }
    }
    
    public void MostrarUsuarios()
    {
        try
        {
            // Verify if the file exists
            if (!File.Exists(Usuario.RutaArchivo))
            {
                Console.WriteLine("No hay usuarios registrados.");
                return;
            }

            // Read all lines from the file
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();

            if (lines.Count == 0)
            {
                Console.WriteLine("No hay usuarios registrados.");
                return;
            }

            // Display user information
            for (int i = 0; i < lines.Count; i += 5) // Assuming each user entry has 5 lines
            {
                int usuarioId = int.Parse(lines[i]);
                string nombre = lines[i + 1];
                string email = lines[i + 2];
                int edad = int.Parse(lines[i + 3]);
                string rol = lines[i + 4];

                Console.WriteLine($"ID del Usuario: {usuarioId}");
                Console.WriteLine($"Nombre: {nombre}");
                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"Edad: {edad}");
                Console.WriteLine($"Rol: {rol}");
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al mostrar los usuarios: " + e.Message);
            throw;
        }
    }

    public void EliminarUsuario(int usuarioId)
    {
        try{
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Read all lines and filter out the user to be deleted
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();
            var updatedLines = new List<string>();
            bool userFound = false;

            for (int i = 0; i < lines.Count; i += 5) // Assuming each user entry has 5 lines
            {
                int currentUserId = int.Parse(lines[i]);
                if (currentUserId != usuarioId){
                    for (int j = 0; j < 5; j++){
                        updatedLines.Add(lines[i + j]);
                    }
                }
                else{
                    userFound = true;
                }
            }

            if (userFound){
                File.WriteAllLines(Usuario.RutaArchivo, updatedLines);

                // Delete the user's folder
                string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    BuscarUsuarioPorId(usuarioId).Nombre);
                if (Directory.Exists(userFolder)){
                    Directory.Delete(userFolder, true);
                }

                Console.WriteLine("Usuario eliminado exitosamente.");
            }
            else{
                Console.WriteLine("Usuario no encontrado.");
            }
        }
        catch (Exception e){
            Console.WriteLine("Error al eliminar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? BuscarUsuarioPorId(int usuarioId)
    {
        try{
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Read all lines and search for the user
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();

            for (int i = 0; i < lines.Count; i += 5) // Assuming each user entry has 5 lines
            {
                int currentUserId = int.Parse(lines[i]);
                if (currentUserId == usuarioId){
                    string nombre = lines[i + 1];
                    string email = lines[i + 2];
                    int edad = int.Parse(lines[i + 3]);
                    Role rol = new Role(int.Parse(lines[i + 4]), ""); // Assuming role name is not stored in the file

                    return new Usuario(currentUserId, nombre, email, edad, rol);
                }
            }

            Console.WriteLine("Usuario no encontrado.");
            return null;
        }
        catch (Exception e){
            Console.WriteLine("Error al buscar el usuario: " + e.Message);
            throw;
        }
    }
}