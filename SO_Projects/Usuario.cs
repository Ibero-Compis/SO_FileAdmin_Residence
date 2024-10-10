namespace Lab4_FileManagement;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public int Edad { get; set; }
    public string Rol { get; set; } // TODO: string -> Role?

    //Constructor de la clase Usuario
    public Usuario(int usuarioId, string nombre, string email, int edad, string rol)
    {
        this.UsuarioId = usuarioId;
        this.Nombre = nombre;
        this.Email = email;
        this.Edad = edad;
        this.Rol = rol;
    }

    //Método para guardar un usuario en un archivo de texto
    public void GuardarUsuario()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "usuarios.txt");

        // Paso 1: Verificar si ya existe el archivo principal "usuarios.txt"
        if (!File.Exists(filePath))
        {
            // Paso 2: Si no existe, crearlo
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("usuarioId,nombre,email,edad,rol");
            }
        }

        // Paso 3: Si existe, agregar el nuevo usuario al final del archivo
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine($"{UsuarioId},{Nombre},{Email},{Edad},{Rol}");
        }

        // Paso 3.1: Crear una carpeta con el mismo nombre de usuario
        string userFolder = Path.Combine(baseDirectory, Nombre);
        if (!Directory.Exists(userFolder))
        {
            Directory.CreateDirectory(userFolder);
        }

        // Crear un txt con el usuarioId, nombre, email, edad y la lista de los id de los permisos
        string userFilePath = Path.Combine(userFolder, $"{Nombre}.txt");
        using (StreamWriter sw = File.CreateText(userFilePath))
        {
            sw.WriteLine($"usuarioId: {UsuarioId}");
            sw.WriteLine($"nombre: {Nombre}");
            sw.WriteLine($"email: {Email}");
            sw.WriteLine($"edad: {Edad}");
            sw.WriteLine($"rol: {Rol}");
            //lista de los id de los permisos si la tienes
        }

        // Paso 4: Ordenarlo por orden alfabético
        var lines = File.ReadAllLines(filePath).Skip(1).OrderBy(line => line.Split(',')[1]).ToList();
        lines.Insert(0, "usuarioId,nombre,email,edad,rol");
        File.WriteAllLines(filePath, lines);
    }

    // Método para eliminar un usuario
    public void EliminarUsuario(string nombreUsuario)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "usuarios.txt");

        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath).ToList();
            var newLines = lines.Where(line => !line.Contains(nombreUsuario)).ToList();
            File.WriteAllLines(filePath, newLines);

            string userFolder = Path.Combine(baseDirectory, nombreUsuario);
            if (Directory.Exists(userFolder))
            {
                Directory.Delete(userFolder, true);
            }
        }
    }

    // Método para modificar un usuario
    public void ModificarUsuario(int usuarioId, string nuevoNombre, string nuevoEmail, int nuevaEdad, string nuevoRol)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "usuarios.txt");

        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                var data = lines[i].Split(',');
                if (data[0] == usuarioId.ToString())
                {
                    lines[i] = $"{usuarioId},{nuevoNombre},{nuevoEmail},{nuevaEdad},{nuevoRol}";
                    break;
                }
            }

            File.WriteAllLines(filePath, lines);

            string oldUserFolder = Path.Combine(baseDirectory, Nombre);
            string newUserFolder = Path.Combine(baseDirectory, nuevoNombre);
            if (Directory.Exists(oldUserFolder))
            {
                Directory.Move(oldUserFolder, newUserFolder);
            }

            string userFilePath = Path.Combine(newUserFolder, $"{nuevoNombre}.txt");
            using (StreamWriter sw = File.CreateText(userFilePath))
            {
                sw.WriteLine($"usuarioId: {usuarioId}");
                sw.WriteLine($"nombre: {nuevoNombre}");
                sw.WriteLine($"email: {nuevoEmail}");
                sw.WriteLine($"edad: {nuevaEdad}");
                sw.WriteLine($"rol: {nuevoRol}");
                // lista de los id de los permisos si la tienes
            }
        }
    }
}