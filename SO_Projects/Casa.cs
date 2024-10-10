namespace Lab4_FileManagement;

public class Casa
{
    // Atributos
    public int CasaId { get; set; }
    public int NumeroCasa { get; set; }
    public string Direccional { get; set; }
    public List<Usuario> Habitantes { get; set; }

    // Constructor
    public Casa(int casaId, int numeroCasa, string direccional)
    {
        CasaId = casaId;
        NumeroCasa = numeroCasa;
        Direccional = direccional;
        Habitantes = new List<Usuario>(); // Inicializamos la lista de habitantes vacía
    }

    // Método para agregar un habitante
    public void AgregarHabitante(Usuario usuario)
    {
        Habitantes.Add(usuario);
        // TODO: Guardar la información de la casa en el archivo
    }

    // Método para eliminar un habitante
    public void EliminarHabitante(Usuario usuario)
    {
        Habitantes.Remove(usuario);
        // TODO: Guardar la información de la casa en el archivo
    }

    // Método para mostrar información de la casa
    public void MostrarInfoCasa()
    {
        Console.WriteLine($"Casa ID: {CasaId}, Numero: {NumeroCasa}, Dirección: {Direccional}");
        Console.WriteLine("Habitantes:");
        foreach (var habitante in Habitantes)
        {
            Console.WriteLine($"- {habitante.Nombre}, Correo: {habitante.Email}");
        }
    }

    // Obtener la ruta completa
    public static string ObtenerRutaArchivo()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "casas", "casas.txt");
    }

    // Verificar si existe el archivo principal de la clase
    public bool VerificarRutaArchivo()
    {
        return File.Exists(ObtenerRutaArchivo());
    }

    // Crear el archivo si no existe
    public void CrearArchivoPrincipal()
    {
        if (!VerificarRutaArchivo())
        {
            using (File.Create(ObtenerRutaArchivo()))
            {
                Console.WriteLine("Archivo de casas creado exitosamente.");
            }

            return;
        }

        Console.WriteLine("El archivo de casas ya existe.");
    }

    // Método que permita guardar la información de la casa en el archivo
    public void GuardarInfoCasa()
    {
        CrearArchivoPrincipal();
        using (StreamWriter sw = new StreamWriter(ObtenerRutaArchivo(), true))
        {
            sw.WriteLine($"Casa ID: {CasaId}, Numero: {NumeroCasa}, Dirección: {Direccional}");
            sw.WriteLine("Habitantes:");
            foreach (var habitante in Habitantes)
            {
                sw.WriteLine($"- {habitante.Nombre}, Correo: {habitante.Email}");
            }

            sw.WriteLine(); // Añadir una línea en blanco para separar las casas
        }
    }

    public static Casa? ObtenerCasaPorId(int id)
    {
        string filePath = ObtenerRutaArchivo();

        // Verificar si el archivo de casas existe
        if (!File.Exists(filePath))
        {
            Console.WriteLine("El archivo de casas no existe.");
            return null;
        }

        // Leer el archivo línea por línea
        using (StreamReader sr = new StreamReader(filePath))
        {
            Casa? casaEncontrada = null;
            List<Usuario> habitantes = new List<Usuario>();
            string? linea;
            bool casaCorrecta = false;

            while ((linea = sr.ReadLine()) != null)
            {
                // Buscar la línea que contiene el CasaID
                if (linea.StartsWith("Casa ID:"))
                {
                    // Extraer el CasaID de la línea
                    int casaIdLeida = int.Parse(linea.Split(",")[0].Split(":")[1].Trim());

                    if (casaIdLeida == id)
                    {
                        casaCorrecta = true;
                        // Obtener los demás datos
                        string[] partes = linea.Split(",");
                        int numeroCasa = int.Parse(partes[1].Split(":")[1].Trim());
                        string direccion = partes[2].Split(":")[1].Trim();

                        casaEncontrada = new Casa(casaIdLeida, numeroCasa, direccion);
                    }
                    else
                    {
                        casaCorrecta = false;
                    }
                }
                // Si la casa correcta fue encontrada, empezar a agregar los habitantes
                else if (casaCorrecta && linea.StartsWith("-"))
                {
                    // Extraer los datos del habitante
                    string[] datosHabitante = linea.Split(",");
                    string nombre = datosHabitante[0].Split("-")[1].Trim();
                    string email = datosHabitante[1].Split(":")[1].Trim();

                    // TODO: Revisar proceso de asignacion de ID
                    Usuario habitante = new Usuario(0, nombre, email, 0, "");

                    casaEncontrada?.AgregarHabitante(habitante);
                }

                // Si encontramos otra casa, salir del ciclo
                if (casaCorrecta && linea == "")
                {
                    break;
                }
            }

            // Retornar la casa encontrada o null si no se encontró
            if (casaEncontrada != null)
            {
                return casaEncontrada;
            }
            else
            {
                Console.WriteLine($"No se encontró una casa con el ID: {id}");
                return null;
            }
        }
    }
}