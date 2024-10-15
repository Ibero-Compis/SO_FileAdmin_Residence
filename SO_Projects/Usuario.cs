using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Edad { get; set; }
    public Role Rol { get; set; }

    public static string RutaArchivo =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "usuarios.txt");

    public ICollection<Permiso> Permisos { get; set; }

    // Constructor de la clase Usuario
    public Usuario(int usuarioId, string nombre, string email, int edad, Role rol, string password)
    {
        this.UsuarioId = usuarioId;
        this.Password = password;
        this.Nombre = nombre;
        this.Email = email;
        this.Edad = edad;
        this.Rol = rol;
    }

    public Usuario()
    {
        UsuarioId = GenerarNuevoId();
        Nombre = "";
        Password = "";
        Email = "";
        Edad = 0;
        Rol = new Role(1, "Residente");
    }

    private static int GenerarNuevoId()
    {
        int nuevoId = 1;
        if (File.Exists(RutaArchivo))
        {
            string[] usersData = File.ReadAllLines(RutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < usersData.Length; i += 6) // Assuming each user entry has 6 lines
            {
                if (!string.IsNullOrWhiteSpace(usersData[i])) // Check if the line is not empty
                {
                    ids.Add(int.Parse(usersData[i]));
                }
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
        return File.Exists(RutaArchivo);
    }

    public void CrearArchivoUsuario(Usuario usuario)
    {
        string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, usuario.Nombre);
        Directory.CreateDirectory(userFolder);

        string userFilePath = Path.Combine(userFolder, $"{usuario.UsuarioId}_{usuario.Nombre}.txt");
        using (StreamWriter sw = File.CreateText(userFilePath))
        {
            sw.WriteLine(usuario.UsuarioId);
            sw.WriteLine(usuario.Nombre);
            sw.WriteLine(usuario.Email);
            sw.WriteLine(usuario.Edad);
            sw.WriteLine(usuario.Rol.RoleName);
            sw.WriteLine(usuario.Password);
            sw.WriteLine();
        }
    }

    public void AgregarUsuario(Usuario usuario)
    {
        try
        {
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Add the user's ID, name, email, age, role, and password to the file
            using (StreamWriter sw = File.AppendText(Usuario.RutaArchivo))
            {
                sw.WriteLine(usuario.UsuarioId);
                sw.WriteLine(usuario.Nombre);
                sw.WriteLine(); // Add an empty line to separate users
            }

            // Create the user's folder and file
            CrearArchivoUsuario(usuario);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al agregar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? BuscarUsuarioPorId(int usuarioId)
    {
        try
        {
            // Verify if the file exists
            if (!File.Exists(Usuario.RutaArchivo))
            {
                Console.WriteLine("Archivo de usuarios no encontrado.");
                return null;
            }

            // Read all lines and search for the user ID
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();
            for (int i = 0; i < lines.Count; i += 6) // Assuming each user entry has 6 lines
            {
                if (int.TryParse(lines[i], out int currentUserId) && currentUserId == usuarioId)
                {
                    string nombre = lines[i + 1];
                    return BuscarUsuarioPorNombre(nombre);
                }
            }

            Console.WriteLine("Usuario no encontrado.");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al buscar el usuario: " + e.Message);
            throw;
        }
    }

    public void MostrarTodosLosUsuarios()
    {
        try
        {
            // Verify if the file exists
            if (!File.Exists(Usuario.RutaArchivo))
            {
                Console.WriteLine("Archivo de usuarios no encontrado.");
                return;
            }

            // Read all lines and get user names
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();

            // If the file is empty, return
            if (lines.Count == 0)
            {
                Console.WriteLine("No hay usuarios registrados.");
                return;
            }

            for (int i = 0; i < lines.Count; i += 7) // Assuming each user entry has 6 lines + 1 empty line
            {
                string nombre = lines[i + 1];
                Usuario? usuario = BuscarUsuarioPorNombre(nombre);
                if (usuario != null)
                {
                    Console.WriteLine($"ID del Usuario: {usuario.UsuarioId}");
                    Console.WriteLine($"Nombre: {usuario.Nombre}");
                    Console.WriteLine($"Email: {usuario.Email}");
                    Console.WriteLine($"Edad: {usuario.Edad}");
                    Console.WriteLine($"Rol: {usuario.Rol.RoleName}");
                    Console.WriteLine($"Contraseña: {usuario.Password}");
                    Console.WriteLine();
                }
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
        try
        {
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Read all lines and filter out the user to be deleted
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();
            var updatedLines = new List<string>();
            bool userFound = false;
            string userName = string.Empty;

            for (int i = 0; i < lines.Count; i += 7) // Assuming each user entry has 6 lines + 1 empty line
            {
                if (!string.IsNullOrWhiteSpace(lines[i]) && int.TryParse(lines[i], out int currentUserId))
                {
                    if (currentUserId != usuarioId)
                    {
                        updatedLines.Add(lines[i]);
                        updatedLines.Add(lines[i + 1]);
                        updatedLines.Add(lines[i + 2]);
                        updatedLines.Add(lines[i + 3]);
                        updatedLines.Add(lines[i + 4]);
                        updatedLines.Add(lines[i + 5]);
                        updatedLines.Add(lines[i + 6]); // Add the empty line
                    }
                    else
                    {
                        userFound = true;
                        userName = lines[i + 1];
                    }
                }
            }

            if (userFound)
            {
                File.WriteAllLines(Usuario.RutaArchivo, updatedLines);

                // Delete the user's folder
                string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, userName);
                if (Directory.Exists(userFolder))
                {
                    Directory.Delete(userFolder, true);
                    Console.WriteLine("Usuario eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine("Carpeta del usuario no encontrada.");
                }
            }
            else
            {
                Console.WriteLine("Usuario no encontrado.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al eliminar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? BuscarUsuarioPorNombre(string nombreUsuario)
    {
        try
        {
            // Buscar la carpeta con el nombre del usuario
            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreUsuario);
            if (!Directory.Exists(userFolder))
            {
                Console.WriteLine("Usuario no encontrado.");
                return null;
            }

            // Buscar el archivo .txt dentro de la carpeta del usuario
            string[] userFiles = Directory.GetFiles(userFolder, "*.txt");
            if (userFiles.Length == 0)
            {
                Console.WriteLine("Archivo de usuario no encontrado.");
                return null;
            }

            // Leer la información del archivo
            string userFilePath = userFiles[0];
            string[] lines = File.ReadAllLines(userFilePath);

            if (lines.Length < 6)
            {
                Console.WriteLine("Archivo de usuario incompleto.");
                return null;
            }

            int usuarioId = int.Parse(lines[0]);
            string nombre = lines[1];
            string email = lines[2];
            int edad = int.Parse(lines[3]);
            Role rol = new Role(1, lines[4]); // Asumiendo que el RoleName se puede obtener de otra manera
            string password = lines[5];

            return new Usuario(usuarioId, nombre, email, edad, rol, password);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al buscar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? BuscarUsuarioPorEmail(string emailUsuario)
    {
        try
        {
            // Verify if the file exists
            if (!File.Exists(RutaArchivo))
            {
                Console.WriteLine("Archivo de usuarios no encontrado.");
                return null;
            }

            // Read all lines and search for the email
            var lines = File.ReadAllLines(RutaArchivo).ToList();
            for (int i = 0; i < lines.Count; i += 6) // Assuming each user entry has 6 lines
            {
                if (lines[i + 2] == emailUsuario)
                {
                    int usuarioId = int.Parse(lines[i]);
                    string nombre = lines[i + 1];
                    string email = lines[i + 2];
                    int edad = int.Parse(lines[i + 3]);
                    Role rol = new Role(1, lines[i + 4]); // Assuming that the RoleName can be obtained this way
                    string password = lines[i + 5];

                    return new Usuario(usuarioId, nombre, email, edad, rol, password);
                }
            }

            Console.WriteLine("Usuario no encontrado.");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al buscar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? IniciarSesion(string? email, string? password)
    {
        try
        {
            // Get the base directory where user folders are stored
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Get all user folders
            string[] userFolders = Directory.GetDirectories(baseDirectory);

            foreach (string userFolder in userFolders)
            {
                // Get all .txt files in the user folder
                string[] userFiles = Directory.GetFiles(userFolder, "*.txt");

                foreach (string userFile in userFiles)
                {
                    // Read the user file
                    string[] lines = File.ReadAllLines(userFile);

                    if (lines.Length >= 6)
                    {
                        string fileEmail = lines[2];
                        string filePassword = lines[5];

                        // Check if the email and password match
                        if (fileEmail == email && filePassword == password)
                        {
                            int usuarioId = int.Parse(lines[0]);
                            string nombre = lines[1];
                            int edad = int.Parse(lines[3]);
                            Role rol = new Role(1, lines[4]); // Assuming that the RoleName can be obtained this way

                            return new Usuario(usuarioId, nombre, email, edad, rol, password);
                        }
                    }
                }
            }

            Console.WriteLine("Usuario no encontrado.");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al buscar el usuario: " + e.Message);
            throw;
        }
    }
}