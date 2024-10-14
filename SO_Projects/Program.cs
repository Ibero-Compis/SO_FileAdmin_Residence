using System;

namespace Lab4_FileManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            AdminMenu();
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Menú de Administración =====");
                Console.WriteLine("1. Agregar Usuario");
                Console.WriteLine("2. Eliminar Usuario");
                Console.WriteLine("3. Buscar Usuario por ID");
                Console.WriteLine("4. Mostrar todos los usuarios");
                Console.WriteLine("5. Volver al Menú Principal");
                Console.WriteLine("==================================");
                Console.Write("Seleccione una opción: ");
                string adminChoice = Console.ReadLine();

                switch (adminChoice)
                {
                    case "1":
                        AgregarUsuario();
                        break;
                    case "2":
                        EliminarUsuario();
                        break;
                    case "3":
                        BuscarUsuarioPorId();
                        break;
                    case "4":
                        MostrarTodosLosUsuarios();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AgregarUsuario()
        {
            Console.Clear();
            Usuario newUser = new Usuario();
            Console.Write("Ingrese Nombre del Usuario: ");
            newUser.Nombre = Console.ReadLine();
            Console.Write("Ingrese Email del Usuario: ");
            newUser.Email = Console.ReadLine();

            int edad;
            while (true)
            {
                Console.Write("Ingrese Edad del Usuario: ");
                if (int.TryParse(Console.ReadLine(), out edad) && edad > 0)
                {
                    newUser.Edad = edad;
                    break;
                }
                else
                {
                    Console.WriteLine("Edad inválida. Debe ser un número mayor que 0.");
                }
            }

            Usuario userManager = new Usuario();
            userManager.AgregarUsuario(newUser);

            Console.WriteLine("Usuario agregado exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EliminarUsuario()
        {
            Console.Clear();
            Usuario userManager = new Usuario();
            userManager.MostrarTodosLosUsuarios();

            int userId;
            while (true)
            {
                Console.Write("Ingrese ID del Usuario a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out userId) && userId > 0)
                {
                    userManager.EliminarUsuario(userId);
                    Console.WriteLine("Usuario eliminado exitosamente.");
                    break;
                }
                else
                {
                    Console.WriteLine("ID inválido. Debe ser un número mayor que 0.");
                }
            }
        }

        static void BuscarUsuarioPorId()
        {
            Console.Clear();
            Usuario userManager = new Usuario();
            userManager.MostrarTodosLosUsuarios();

            int userId;
            while (true)
            {
                Console.Write("Ingrese ID del Usuario a buscar: ");
                if (int.TryParse(Console.ReadLine(), out userId) && userId > 0)
                {
                    Usuario? user = Usuario.BuscarUsuarioPorId(userId);
                    if (user != null)
                    {
                        Console.WriteLine($"ID del Usuario: {user.UsuarioId}");
                        Console.WriteLine($"Nombre: {user.Nombre}");
                        Console.WriteLine($"Email: {user.Email}");
                        Console.WriteLine($"Edad: {user.Edad}");
                        Console.WriteLine($"Rol: {user.Rol.RoleName}");
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("ID inválido. Debe ser un número mayor que 0.");
                }
            }

            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void MostrarTodosLosUsuarios()
        {
            Console.Clear();
            Usuario userManager = new Usuario();
            userManager.MostrarTodosLosUsuarios();
            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }
    }
}