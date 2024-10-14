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
                Console.WriteLine("Menú de Administración:");
                Console.WriteLine("1. Agregar Usuario");
                Console.WriteLine("2. Eliminar Usuario");
                Console.WriteLine("3. Buscar Usuario por ID");
                Console.WriteLine("4. Volver al Menú Principal");
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
            Console.Write("Ingrese ID del Usuario: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Ingrese Nombre del Usuario: ");
            string userName = Console.ReadLine();
            Console.Write("Ingrese Email del Usuario: ");
            string userEmail = Console.ReadLine();
            Console.Write("Ingrese Edad del Usuario: ");
            int userAge = int.Parse(Console.ReadLine());
            Console.Write("Ingrese Rol del Usuario: ");
            string userRole = Console.ReadLine();

            Role role = new Role(1, userRole); // Asumiendo que el ID del rol es 1 por simplicidad
            Usuario newUser = new Usuario(userId, userName, userEmail, userAge, role);

            // Asumiendo que tienes una instancia de una clase que contiene el método AgregarUsuario
            Usuario userManager = new Usuario();
            userManager.AgregarUsuario(newUser);

            Console.WriteLine("Usuario agregado exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EliminarUsuario()
        {
            Console.Clear();
            Console.Write("Ingrese ID del Usuario a eliminar: ");
            int userId = int.Parse(Console.ReadLine());

            Usuario userManager = new Usuario();
            userManager.EliminarUsuario(userId);

            Console.WriteLine("Usuario eliminado exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void BuscarUsuarioPorId()
        {
            Console.Clear();
            Console.Write("Ingrese ID del Usuario a buscar: ");
            int userId = int.Parse(Console.ReadLine());

            // Asumiendo que tienes una instancia de una clase que contiene el método BuscarUsuarioPorId
            Usuario? user = Usuario.BuscarUsuarioPorId(userId);

            if (user != null)
            {
                Console.WriteLine($"ID del Usuario: {user.UsuarioId}");
                Console.WriteLine($"Nombre: {user.Nombre}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Edad: {user.Edad}");
                Console.WriteLine($"Rol: {user.Rol.RoleName}");
            }
            else
            {
                Console.WriteLine("Usuario no encontrado.");
            }

            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }
    }
}