using System;
using System.Collections.Generic;
using System.IO;

namespace Lab4_FileManagement;

public class Entrada
{
    public int id { get; set; }
    public string comentario { get; set; }
    public DateTime fecha { get; set; }
    public string terminal { get; set; }
    public int permisoId { get; set; }
    
    public Entrada(string comentario, DateTime fecha, string terminal, int permiso)
    {
        this.id = ObtenerUltimoId() + 1;;
        this.comentario = comentario;
        this.fecha = fecha;
        this.terminal = terminal;
        this.permisoId = permiso;
    }
    
    // id autoincrementable
    public int ObtenerUltimoId()
    {
        if (!File.Exists(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            return 0; // No entries exist yet
        }

        int lastId = 0;
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    lastId = int.Parse(line);
                }

                // Skip the rest of the entry
                sr.ReadLine(); // permisoId
                sr.ReadLine(); // comentario
                sr.ReadLine(); // fecha
                sr.ReadLine(); // terminal
                sr.ReadLine(); // empty line
            }
        }

        return lastId;
    }
    
    // ARCHIVOS
    // Lógica de localización de archivos TODO:ARREGLAR
    private static readonly string BaseDirectory =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_data", "Entrada"); // Relative file location

    //private const string FileName = "tbl_rol.txt"; // File name
    //protected internal static readonly string FilePath = Path.Combine(BaseDirectory, FileName); // Full path

    // metodo para agregar una entrada utilizando archivos
    public void AgregarEntrada(Entrada entrada)
    {
        Console.WriteLine("Agregando entrada...");
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            Directory.CreateDirectory(BaseDirectory);
        }
        
        // validar si ya existe el archivo entradas.txt
        if (!File.Exists(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            // crear el archivo entradas.txt
            using (StreamWriter sw = File.CreateText(Path.Combine(BaseDirectory, "entradas.txt")))
            {
                // escribir la primera entrada
                sw.WriteLine(entrada.id);
                sw.WriteLine(entrada.comentario);
                sw.WriteLine(entrada.fecha);
                sw.WriteLine(entrada.terminal);
                sw.WriteLine(entrada.permisoId);
            }
        }
        else
        {
            // agregar la entrada al archivo entradas.txt
            using (StreamWriter sw = File.AppendText(Path.Combine(BaseDirectory, "entradas.txt")))
            {
                // escribir una linea vacia para separar las entradas
                sw.WriteLine();
                // escribir la nueva entrada
                sw.WriteLine(entrada.id);
                sw.WriteLine(entrada.comentario);
                sw.WriteLine(entrada.fecha);
                sw.WriteLine(entrada.terminal);
                sw.WriteLine(entrada.permisoId);
            }
        }
        
        // crear el archivo con la informacion de la entrada
        //using (StreamWriter sw = File.AppendText(Path.Combine(BaseDirectory, entrada.id.ToString() + ".txt")))
        //{
        //    sw.WriteLine(entrada.id);
        //    sw.WriteLine(entrada.permisoId.ToString());
        //    sw.WriteLine(entrada.comentario);
        //    sw.WriteLine(entrada.fecha);
        //    sw.WriteLine(entrada.terminal);
        //}
    }

    // metodo para obtener una entrada utilizando archivos
    public Entrada ObtenerEntrada(int id)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            // si no existe la carpeta, lanzar error
            throw new Exception("No se ha encontrado la carpeta de entradas");
        }
        
        // leer el archivo de entradas y buscar el bloque q contenga el id recibido
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            string line;
            while ((line = sr.ReadLine()) != null) // primero lee el id
            {
                // leer las demas lineas para asegurarse de q en la proxima vuelta del while se lea el siguiente id
                sr.ReadLine(); // permisoId
                sr.ReadLine(); // comentario
                sr.ReadLine(); // fecha
                sr.ReadLine(); // terminal
                sr.ReadLine(); // linea vacia
                
                if (line == id.ToString())
                {
                    int permisoId = int.Parse(sr.ReadLine()); // TODO: agregar permiso
                    string comentario = sr.ReadLine();
                    DateTime fecha = DateTime.Parse(sr.ReadLine());
                    string terminal = sr.ReadLine();
                    
                    return new Entrada(comentario, fecha, terminal, permisoId);
                }
            }
        }
        
        // si no se encontro la entrada, lanzar error
        throw new Exception("No se ha encontrado la entrada con el id: " + id);
    }
    
    // metodo para obtener todas las entradas utilizando archivos
    public List<Entrada> ObtenerTodasLasEntradas()
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            // si no existe la carpeta, lanzar error
            throw new Exception("No se ha encontrado la carpeta de entradas");
        }
        
        // leer el archivo de entradas y guardar todas las entradas en una lista
        List<Entrada> entradas = new List<Entrada>();
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            string line;
            while ((line = sr.ReadLine()) != null) // primero lee el id
            {
                int id = int.Parse(line);
                int permisoId = int.Parse(sr.ReadLine()); // TODO: agregar permiso
                string comentario = sr.ReadLine();
                DateTime fecha = DateTime.Parse(sr.ReadLine());
                string terminal = sr.ReadLine();
                
                entradas.Add(new Entrada(comentario, fecha, terminal, permisoId));
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
            // si no existe la carpeta, lanzar error
            throw new Exception("No se ha encontrado la carpeta de entradas");
        }
        
        // leer el archivo de entradas y buscar el bloque q contenga el id recibido para eliminarlo
        List<string> lines = new List<string>();
        
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            string line;
            while ((line = sr.ReadLine()) != null) // primero lee el id
            {
                if (line == id.ToString())
                {
                    sr.ReadLine(); // permisoId
                    sr.ReadLine(); // comentario
                    sr.ReadLine(); // fecha
                    sr.ReadLine(); // terminal
                    sr.ReadLine(); // linea vacia
                }
                else
                {
                    lines.Add(line);
                    lines.Add(sr.ReadLine()); // permisoId
                    lines.Add(sr.ReadLine()); // comentario
                    lines.Add(sr.ReadLine()); // fecha
                    lines.Add(sr.ReadLine()); // terminal
                    lines.Add(sr.ReadLine()); // linea vacia
                }
            }
        }
        
        // sobreescribir el archivo de entradas con las lineas restantes
        using (StreamWriter sw = File.CreateText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            foreach (string line in lines)
            {
                sw.WriteLine(line);
            }
        }
    }
    
    // metodo para modificar una entrada utilizando archivos
    public void ModificarEntrada(Entrada entrada)
    {
        // buscar la direccion de la carpeta
        if (!Directory.Exists(BaseDirectory))
        {
            // si no existe la carpeta, lanzar error
            throw new Exception("No se ha encontrado la carpeta de entradas");
        }
        
        // leer el archivo de entradas y buscar el bloque q contenga el id recibido para modificarlo
        List<string> lines = new List<string>();
        
        using (StreamReader sr = File.OpenText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            string line;
            while ((line = sr.ReadLine()) != null) // primero lee el id
            {
                if (line == entrada.id.ToString())
                {
                    sr.ReadLine(); // permisoId
                    sr.ReadLine(); // comentario
                    sr.ReadLine(); // fecha
                    sr.ReadLine(); // terminal
                    sr.ReadLine(); // linea vacia
                    
                    lines.Add(entrada.id.ToString());
                    lines.Add(entrada.permisoId.ToString());
                    lines.Add(entrada.comentario);
                    lines.Add(entrada.fecha.ToString());
                    lines.Add(entrada.terminal);
                    lines.Add(""); // linea vacia
                }
                else
                {
                    lines.Add(line);
                    lines.Add(sr.ReadLine()); // permisoId
                    lines.Add(sr.ReadLine()); // comentario
                    lines.Add(sr.ReadLine()); // fecha
                    lines.Add(sr.ReadLine()); // terminal
                    lines.Add(sr.ReadLine()); // linea vacia
                }
            }
        }
        
        // sobreescribir el archivo de entradas con las lineas restantes
        using (StreamWriter sw = File.CreateText(Path.Combine(BaseDirectory, "entradas.txt")))
        {
            foreach (string line in lines)
            {
                sw.WriteLine(line);
            }
        }
    }

}