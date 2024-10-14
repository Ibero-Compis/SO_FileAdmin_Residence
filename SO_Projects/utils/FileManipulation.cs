using System.IO;

namespace Lab4_FileManagement.utils;

public class FileManipulation
{
    public static void VerifyFileLocation(string filePath)
    {
        try
        {
            // Verificar si el archivo ya existe
            if (!File.Exists(filePath))
            {
                // Si el archivo no existe, crear los directorios necesarios
                var directory = Path.GetDirectoryName(filePath);

                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Console.WriteLine("Directorios creados: " + directory);
                }

                // Crear el archivo vacío
                File.Create(filePath).Dispose();
                Console.WriteLine("Archivo creado: " + filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al verificar o crear el archivo: " + ex.Message);
        }
    }

    public static void AppendLineToFile(string filePath, string contenido)
    {
        try
        {
            // Abre el archivo en modo de agregar, o crea el archivo si no existe.
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                // Escribe el contenido en una nueva línea.
                sw.WriteLine(contenido);
            }

            Console.WriteLine("Línea agregada exitosamente al archivo.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al escribir en el archivo: " + ex.Message);
        }
    }
}