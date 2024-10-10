namespace Lab4_FileManagement;

public class Entrada
{
    public int id { get; set; }
    public string comentario { get; set; }
    public DateTime fecha { get; set; }
    public string terminal { get; set; }
    public int permisoId { get; set; }
    
    public Entrada(int id, string comentario, DateTime fecha, string terminal, int permiso)
    {
        this.id = id;
        this.comentario = comentario;
        this.fecha = fecha;
        this.terminal = terminal;
        this.permisoId = permiso;
    }
    
    // ARCHIVOS
    // Lógica de localización de archivos TODO:ARREGLAR
    private static readonly string BaseDirectory =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NSdb_files", "Data", "Entrada"); // Relative file location

    //private const string FileName = "tbl_rol.txt"; // File name
    //protected internal static readonly string FilePath = Path.Combine(BaseDirectory, FileName); // Full path

    // metodo para agregar una entrada utilizando archivos
    public void AgregarEntrada(Entrada entrada)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // buscar si ya existe una entrada con el mismo id
        if (File.Exists(Path.Combine(BaseDirectory, entrada.id.ToString() + ".txt")))
        {
            throw new Exception("Ya existe una entrada con el mismo id");
        }
        
        // crear el archivo con la informacion de la entrada
        using (StreamWriter sw = File.AppendText(Path.Combine(BaseDirectory, entrada.id.ToString() + ".txt")))
        {
            sw.WriteLine(entrada.id);
            sw.WriteLine(entrada.permisoId.ToString());
            sw.WriteLine(entrada.comentario);
            sw.WriteLine(entrada.fecha);
            sw.WriteLine(entrada.terminal);
        }
    }

    // metodo para obtener una entrada utilizando archivos
    public Entrada ObtenerEntrada(int id)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // leer el archivo con la informacion de la entrada
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, id.ToString() + ".txt")))
        {
            string line;
            int idEntrada = int.Parse(sr.ReadLine());
            int permisoId = int.Parse(sr.ReadLine()); // TODO: esto deberia tener un objeto de tipo permiso
            string comentario = sr.ReadLine();
            DateTime fecha = DateTime.Parse(sr.ReadLine());
            string terminal = sr.ReadLine();
            
            return new Entrada(idEntrada, comentario, fecha, terminal, permisoId);
        }
    }
    
    // metodo para obtener todas las entradas utilizando archivos
    public List<Entrada> ObtenerTodasLasEntradas()
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // leer todos los archivos con la informacion de las entradas
        List<Entrada> entradas = new List<Entrada>();
        foreach (string file in Directory.EnumerateFiles(BaseDirectory))
        {
            using (StreamReader sr = File.OpenText(file))
            {
                string line;
                int idEntrada = int.Parse(sr.ReadLine());
                int permisoId = int.Parse(sr.ReadLine()); // TODO: agregar permiso
                string comentario = sr.ReadLine();
                DateTime fecha = DateTime.Parse(sr.ReadLine());
                string terminal = sr.ReadLine();
                
                entradas.Add(new Entrada(idEntrada, comentario, fecha, terminal, permisoId));
            }
        }
        
        return entradas;
    }

    // metodo para eliminar una entrada utilizando archivos
    public void EliminarEntrada(int id)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // eliminar el archivo con la informacion de la entrada
        File.Delete(Path.Combine(BaseDirectory, id.ToString() + ".txt"));
    }
    
    // metodo para modificar una entrada utilizando archivos
    public void ModificarEntrada(Entrada entrada)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // modificar el archivo con la informacion de la entrada
        using (StreamWriter sw = File.CreateText(Path.Combine(BaseDirectory, entrada.id.ToString() + ".txt")))
        {
            sw.WriteLine(entrada.id);
            sw.WriteLine(entrada.permisoId.ToString() ); // TODO: agregar permiso
            sw.WriteLine(entrada.comentario);
            sw.WriteLine(entrada.fecha);
            sw.WriteLine(entrada.terminal);
        }
    }

}