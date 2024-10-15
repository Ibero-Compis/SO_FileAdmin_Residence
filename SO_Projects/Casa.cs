using Lab4_FileManagement.utils;

namespace Lab4_FileManagement;

public class Casa
{
    // Atributos
    public int CasaId { get; set; }
    public int NumeroCasa { get; set; }
    public string Direccion { get; set; }
    public ICollection<Usuario> Habitantes { get; set; }

    public static string RutaArchivo =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "casas", "casas.txt");

    public Casa()
    {
        CasaId = GenerarNuevoId();
        NumeroCasa = 0;
        Direccion = "";
        Habitantes = new List<Usuario>();
    }

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
        if (File.Exists(RutaArchivo)){
            string[] informacionUsuarios = File.ReadAllLines(RutaArchivo);
            List<int> ids = new List<int>();
            for (int i = 0; i < informacionUsuarios.Length; i += 5) // Assuming each user entry has 5 lines
            {
                if (!string.IsNullOrWhiteSpace(informacionUsuarios[i])) // Check if the line is not empty
                {
                    ids.Add(int.Parse(informacionUsuarios[i]));
                }
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

    public void AgregarCasa(Casa casa)
    {
        int casaId = casa.CasaId;
        int numeroCasa = casa.NumeroCasa;
        string direccion = casa.Direccion;
        string habitantesIds = string.Join(",", casa.Habitantes.Select(h => h.UsuarioId));

        try
        {
            FileManipulation.VerifyFileLocation(RutaArchivo);

            string casaInfo = $"{casaId}\n{numeroCasa}\n{direccion}\n{habitantesIds}";
            File.AppendAllText(RutaArchivo, casaInfo + "\n");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al agregar la casa: " + e.Message);
            throw;
        }
    }

    public static List<Casa> ObtenerCasas()
    {
        List<Casa> casas = new List<Casa>();
        string filePath = RutaArchivo;

        if (File.Exists(filePath)){
            string[] casasData = File.ReadAllLines(filePath);
            for (int i = 0; i < casasData.Length; i += 4) // Assuming each casa entry has 3 or 4 lines
            {
                if (!string.IsNullOrWhiteSpace(casasData[i]) &&
                    !string.IsNullOrWhiteSpace(casasData[i + 1]) &&
                    !string.IsNullOrWhiteSpace(casasData[i + 2])){
                    int casaId = int.Parse(casasData[i]);
                    int numeroCasa = int.Parse(casasData[i + 1]);
                    string direccion = casasData[i + 2];
                    List<Usuario> habitantes = new List<Usuario>();

                    if (i + 3 < casasData.Length && !string.IsNullOrWhiteSpace(casasData[i + 3])){
                        string[] habitantesIds = casasData[i + 3].Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (string habitanteId in habitantesIds){
                            if (int.TryParse(habitanteId, out int id)){
                                Usuario habitante = Usuario.BuscarUsuarioPorId(id); // Method to get Usuario by ID
                                if (habitante != null){
                                    habitantes.Add(habitante);
                                }
                            }
                        }
                    }

                    Casa casa = new Casa(numeroCasa, direccion)
                    {
                        CasaId = casaId,
                        Habitantes = habitantes
                    };
                    casas.Add(casa);
                }
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
        foreach (Casa casa in casas){
            Console.WriteLine($"Casa ID: {casa.CasaId}");
            Console.WriteLine($"Número de Casa: {casa.NumeroCasa}");
            Console.WriteLine($"Dirección: {casa.Direccion}");
            Console.WriteLine("Habitantes:");
            foreach (Usuario habitante in casa.Habitantes){
                Console.WriteLine($"- {habitante.Nombre}");
            }

            Console.WriteLine();
        }
    }

    public void EliminarCasa(int casaId)
    {
        string filePath = RutaArchivo;

        if (File.Exists(filePath))
        {
            string[] casasData = File.ReadAllLines(filePath);
            List<string> updatedCasas = new List<string>();

            for (int i = 0; i < casasData.Length; i += 4) // Assuming each casa entry has 3 or 4 lines
            {
                int currentCasaId = int.Parse(casasData[i]);
                if (currentCasaId != casaId)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i + j < casasData.Length && !string.IsNullOrWhiteSpace(casasData[i + j]))
                        {
                            updatedCasas.Add(casasData[i + j]);
                        }
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
            Usuario? usuario = Usuario.BuscarUsuarioPorId(usuarioId);
            if (usuario != null)
            {
                casa.Habitantes.Add(usuario);
                ActualizarCasa(casa);
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
    
    private void ActualizarCasa(Casa casa)
    {
        string filePath = RutaArchivo;
        if (File.Exists(filePath))
        {
            string[] casasData = File.ReadAllLines(filePath);
            List<string> updatedCasas = new List<string>();

            for (int i = 0; i < casasData.Length; i += 4) // Assuming each casa entry has 3 or 4 lines
            {
                int currentCasaId = int.Parse(casasData[i]);
                if (currentCasaId != casa.CasaId)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i + j < casasData.Length)
                        {
                            updatedCasas.Add(casasData[i + j]);
                        }
                    }
                }
                else
                {
                    updatedCasas.Add(casa.CasaId.ToString());
                    updatedCasas.Add(casa.NumeroCasa.ToString());
                    updatedCasas.Add(casa.Direccion);
                    updatedCasas.Add(string.Join(",", casa.Habitantes.Select(h => h.UsuarioId)));
                }
            }

            File.WriteAllLines(filePath, updatedCasas);
        }
    }

    public void EliminarHabitante(int casaId, int usuarioId)
    {
        Casa? casa = ObtenerCasaPorId(casaId);
        if (casa != null){
            Usuario? usuario = Usuario.BuscarUsuarioPorId(usuarioId);
            if (usuario != null){
                casa.Habitantes.Remove(usuario);
                EliminarCasa(casaId);
                AgregarCasa(casa);
                Console.WriteLine("Habitante eliminado exitosamente.");
            }
            else{
                Console.WriteLine("El usuario no existe.");
            }
        }
        else{
            Console.WriteLine("La casa no existe.");
        }
    }

    public void MostrarHabitantes(int casaId)
    {
        Casa? casa = ObtenerCasaPorId(casaId);
        if (casa != null){
            Console.WriteLine($"Habitantes de la Casa {casa.NumeroCasa}:");
            foreach (Usuario habitante in casa.Habitantes){
                Console.WriteLine($"- {habitante.Nombre}");
            }
        }
        else{
            Console.WriteLine("La casa no existe.");
        }
    }

    public void MostrarHabitantesDeTodasLasCasas()
    {
        List<Casa> casas = ObtenerCasas();
        foreach (Casa casa in casas){
            Console.WriteLine($"Habitantes de la Casa {casa.NumeroCasa}:");
            foreach (Usuario habitante in casa.Habitantes){
                Console.WriteLine($"- {habitante.Nombre}");
            }

            Console.WriteLine();
        }
    }
}