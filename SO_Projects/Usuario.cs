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
    public static readonly string RutaBaseUsuarios =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "Users");
    
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
    
    static Usuario()
    {
        if (!Directory.Exists(RutaBaseUsuarios))
        {
            Directory.CreateDirectory(RutaBaseUsuarios);
        }
    }

    private static int GenerarNuevoId()
    {
        int nuevoId = 1;
        if (File.Exists(RutaArchivo))
        {
            string[] usersData = File.ReadAllLines(RutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < usersData.Length; i += 3) // Assuming each user entry has 2 lines + 1 empty line
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

    public void CrearArchivoUsuario(Usuario usuario)
    {
        string userFolder = Path.Combine(RutaBaseUsuarios, usuario.Nombre);
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
        try{
            // Verify if the file exists
            FileManipulation.VerifyFileLocation(Usuario.RutaArchivo);

            // Add the user's ID, name, email, age, role, and password to the file
            using (StreamWriter sw = File.AppendText(Usuario.RutaArchivo)){
                sw.WriteLine(usuario.UsuarioId);
                sw.WriteLine(usuario.Nombre);
                sw.WriteLine(); // Add an empty line to separate users
            }

            // Create the user's folder and file
            CrearArchivoUsuario(usuario);
        }
        catch (Exception e){
            Console.WriteLine("Error al agregar el usuario: " + e.Message);
            throw;
        }
    }

    public static Usuario? BuscarUsuarioPorId(int usuarioId)
    {
        try
        {
            // Get all user folders
            string[] userFolders = Directory.GetDirectories(RutaBaseUsuarios);

            foreach (string userFolder in userFolders)
            {
                // Get all .txt files in the user folder
                string[] userFiles = Directory.GetFiles(userFolder, "*.txt");

                foreach (string userFile in userFiles)
                {
                    // Extract the file name without extension
                    string fileName = Path.GetFileNameWithoutExtension(userFile);

                    // Split the file name to get the ID and name
                    string[] parts = fileName.Split('_');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int fileUserId) && fileUserId == usuarioId)
                    {
                        // Read the user file
                        string[] lines = File.ReadAllLines(userFile);

                        if (lines.Length >= 6)
                        {
                            string nombre = lines[1];
                            string email = lines[2];
                            int edad = int.Parse(lines[3]);
                            Role rol = new Role(1, lines[4]); // Assuming that the RoleName can be obtained this way
                            string password = lines[5];

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

    public void MostrarTodosLosUsuarios()
    {
        try{
            // Verify if the file exists
            if (!File.Exists(Usuario.RutaArchivo)){
                Console.WriteLine("Archivo de usuarios no encontrado.");
                return;
            }

            // Read all lines from the file
            var lines = File.ReadAllLines(Usuario.RutaArchivo).ToList();

            // If the file is empty, return
            if (lines.Count == 0){
                Console.WriteLine("No hay usuarios registrados.");
                return;
            }

            // Iterate through the lines to get user IDs and names
            for (int i = 0; i < lines.Count; i += 3) // Assuming each user entry has 2 lines + 1 empty line
            {
                if (i + 1 < lines.Count) // Ensure there are enough lines for a complete user entry
                {
                    int usuarioId = int.Parse(lines[i]);

                    // Search for the user by name
                    Usuario? usuario = BuscarUsuarioPorId(usuarioId);
                    if (usuario != null){
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
        }
        catch (Exception e){
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
            string userName = string.Empty;

            for (int i = 0; i < lines.Count; i += 3) // Assuming each user entry has 2 lines + 1 empty line
            {
                if (!string.IsNullOrWhiteSpace(lines[i]) && int.TryParse(lines[i], out int currentUserId)){
                    if (currentUserId != usuarioId){
                        updatedLines.Add(lines[i]);
                        updatedLines.Add(lines[i + 1]);
                        updatedLines.Add(lines[i + 2]); // Add the empty line
                    }
                    else{
                        userFound = true;
                        userName = lines[i + 1];
                    }
                }
            }

            if (userFound){
                File.WriteAllLines(Usuario.RutaArchivo, updatedLines);

                // Delete the user's folder
                string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, userName);
                if (Directory.Exists(userFolder)){
                    Directory.Delete(userFolder, true);
                    Console.WriteLine("Usuario eliminado exitosamente.");
                }
                else{
                    Console.WriteLine("Carpeta del usuario no encontrada.");
                }
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

    public static Usuario? BuscarUsuarioPorEmail(string emailUsuario)
    {
        try
        {
            // Get all user folders
            string[] userFolders = Directory.GetDirectories(RutaBaseUsuarios);

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

                        // Check if the email matches
                        if (fileEmail == emailUsuario)
                        {
                            int usuarioId = int.Parse(lines[0]);
                            string nombre = lines[1];
                            int edad = int.Parse(lines[3]);
                            Role rol = new Role(1, lines[4]); // Assuming that the RoleName can be obtained this way
                            string password = lines[5];

                            return new Usuario(usuarioId, nombre, fileEmail, edad, rol, password);
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

    public static Usuario? IniciarSesion(string? email, string? password)
    {
        try
        {
            // Get all user folders
            string[] userFolders = Directory.GetDirectories(RutaBaseUsuarios);

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