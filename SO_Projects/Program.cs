using System;

namespace Lab4_FileManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            AdminMenu();
        }
        
        static void IniciarSesion()
        {
            Console.Clear();
            Console.Write("Ingrese Email: ");
            string email = Console.ReadLine();
            Console.Write("Ingrese Contraseña: ");
            string password = Console.ReadLine();

            Usuario? user = Usuario.IniciarSesion(email, password);
            if (user != null)
            {
                switch (user.Rol.RoleName)
                {
                    case "Admin":
                        AdminMenu();
                        break;
                    case "Vigilante":
                        VigilantMenu();
                        break;
                    default:
                        Console.WriteLine("Rol no válido. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Email o contraseña incorrectos. Presione cualquier tecla para intentar de nuevo...");
                Console.ReadKey();
            }
        }
        
        static void PrincipalMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Menú Principal =====");
                Console.WriteLine("1. Iniciar Sesión");
                Console.WriteLine("2. Salir");
                Console.WriteLine("==================================");
                Console.Write("Seleccione una opción: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        IniciarSesion();
                        break;
                    case "2":
                        Console.WriteLine("Gracias por usar el sistema. Hasta luego.");
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        static void VigilantMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Menú de Vigilante =====");
                Console.WriteLine("1. Mostrar Permisos");
                Console.WriteLine("2. Registrar Entrada");
                Console.WriteLine("3. Volver al Menú Principal");
                Console.WriteLine("==================================");
                Console.Write("Seleccione una opción: ");
                string vigilanteChoice = Console.ReadLine();

                switch (vigilanteChoice)
                {
                    case "1":
                        MostrarPermisos();
                        break;
                    case "2":
                        // TODO IMPLEMENTAR REGISTRO DE ENTRADA 
                        return;
                    case "3":
                        PrincipalMenu();
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
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
                Console.WriteLine("5. Gestión de Permisos");
                Console.WriteLine("6. Gestión de Casas");
                Console.WriteLine("7. Volver al Menú Principal");
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
                        GestionPermisosMenu();
                        break;
                    case "6":
                        GestionCasasMenu();
                        break;
                    case "7":
                        PrincipalMenu();
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void GestionPermisosMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Gestión de Permisos =====");
                Console.WriteLine("1. Agregar Permiso");
                Console.WriteLine("2. Mostrar Permisos");
                Console.WriteLine("3. Eliminar Permiso");
                Console.WriteLine("4. Volver al Menú de Administración");
                Console.WriteLine("==================================");
                Console.Write("Seleccione una opción: ");
                string permisoChoice = Console.ReadLine();

                switch (permisoChoice)
                {
                    case "1":
                        AgregarPermiso();
                        break;
                    case "2":
                        MostrarPermisos();
                        break;
                    case "3":
                        EliminarPermiso();
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

        static void GestionCasasMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Gestión de Casas =====");
                Console.WriteLine("1. Agregar Casa");
                Console.WriteLine("2. Mostrar Casas");
                Console.WriteLine("3. Eliminar Casa");
                Console.WriteLine("4. Agregar Habitante");
                Console.WriteLine("5. Eliminar Habitante");
                Console.WriteLine("6. Volver al Menú de Administración");
                Console.WriteLine("==================================");
                Console.Write("Seleccione una opción: ");
                string casaChoice = Console.ReadLine();

                switch (casaChoice)
                {
                    case "1":
                        AgregarCasa();
                        break;
                    case "2":
                        MostrarCasas();
                        break;
                    case "3":
                        EliminarCasa();
                        break;
                    case "4":
                        AgregarHabitante();
                        break;
                    case "5":
                        EliminarHabitante();
                        break;
                    case "6":
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
            while (string.IsNullOrWhiteSpace(newUser.Nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío. Intente de nuevo.");
                newUser.Nombre = Console.ReadLine();
            }

            Console.Write("Ingrese Email del Usuario: ");
            newUser.Email = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(newUser.Email))
            {
                Console.WriteLine("El email no puede estar vacío. Intente de nuevo.");
                newUser.Email = Console.ReadLine();
            }

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
                    else
                    {
                        Console.WriteLine("Usuario no encontrado.");
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

        static void AgregarPermiso()
        {
            Console.Clear();
            DateTime fechaInicio;
            while (true)
            {
                Console.Write("Ingrese Fecha de Inicio (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out fechaInicio))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Fecha inválida. Intente de nuevo.");
                }
            }

            DateTime fechaFin;
            while (true)
            {
                Console.Write("Ingrese Fecha de Fin (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out fechaFin))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Fecha inválida. Intente de nuevo.");
                }
            }

            int casaId;
            while (true)
            {
                Console.Write("Ingrese ID de la Casa: ");
                if (int.TryParse(Console.ReadLine(), out casaId) && casaId > 0)
                {
                    Casa casa = Casa.ObtenerCasaPorId(casaId);
                    if (casa == null)
                    {
                        Console.WriteLine("Casa no encontrada. Por favor, ingrese un ID de Casa válido.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("ID de Casa inválido. Debe ser un número mayor que 0.");
                }
            }

            int usuarioId;
            while (true)
            {
                Console.Write("Ingrese ID del Usuario: ");
                if (int.TryParse(Console.ReadLine(), out usuarioId) && usuarioId > 0)
                {
                    Usuario usuario = Usuario.BuscarUsuarioPorId(usuarioId);
                    if (usuario == null)
                    {
                        Console.WriteLine("Usuario no encontrado. Por favor, ingrese un ID de Usuario válido.");
                    }
                    else
                    {
                        Permiso permiso = new Permiso(fechaInicio, fechaFin, Casa.ObtenerCasaPorId(casaId), usuario);
                        permiso.AgregarPermiso(permiso);
                        Console.WriteLine("Permiso agregado exitosamente. Presione cualquier tecla para volver...");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("ID de Usuario inválido. Debe ser un número mayor que 0.");
                }
            }

            Console.ReadKey();
        }

        static void MostrarPermisos()
        {
            Console.Clear();
            Permiso permisoManager = new Permiso();
            permisoManager.MostrarPermisos();
            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EliminarPermiso()
        {
            Console.Clear();
            int permisoId;
            while (true)
            {
                Console.Write("Ingrese ID del Permiso a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out permisoId) && permisoId > 0)
                {
                    Permiso permisoManager = new Permiso();
                    permisoManager.EliminarPermiso(permisoId);
                    Console.WriteLine("Permiso eliminado exitosamente.");
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

        static void AgregarCasa()
        {
            Console.Clear();
            int numeroCasa;
            while (true)
            {
                Console.Write("Ingrese Número de la Casa: ");
                if (int.TryParse(Console.ReadLine(), out numeroCasa) && numeroCasa > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Número de casa inválido. Debe ser un número mayor que 0.");
                }
            }

            Console.Write("Ingrese Dirección de la Casa: ");
            string direccion = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(direccion))
            {
                Console.WriteLine("La dirección no puede estar vacía. Intente de nuevo.");
                direccion = Console.ReadLine();
            }

            Casa casa = new Casa(numeroCasa, direccion);
            casa.AgregarCasa(casa);

            Console.WriteLine("Casa agregada exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void MostrarCasas()
        {
            Console.Clear();
            Casa casaManager = new Casa();
            casaManager.MostrarCasas();
            Console.WriteLine("Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EliminarCasa()
        {
            Console.Clear();
            // Mostrar todas las casas
            Casa casaManager = new Casa();
            casaManager.MostrarCasas();
            Console.WriteLine();
            
            int casaId;
            while (true)
            {
                Console.Write("Ingrese ID de la Casa a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out casaId) && casaId > 0)
                {
                    casaManager.EliminarCasa(casaId);
                    Console.WriteLine("Casa eliminada exitosamente.");
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

        static void AgregarHabitante()
        {
            Console.Clear();
            
            // Mostrar todas las casas
            Casa casaManager = new Casa();
            casaManager.MostrarCasas();
            Console.WriteLine();
            
            int casaId;
            while (true)
            {
                Console.Write("Ingrese ID de la Casa: ");
                if (int.TryParse(Console.ReadLine(), out casaId) && casaId > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ID de Casa inválido. Debe ser un número mayor que 0.");
                }
            }

            int usuarioId;
            while (true)
            {
                Console.Write("Ingrese ID del Usuario: ");
                if (int.TryParse(Console.ReadLine(), out usuarioId) && usuarioId > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ID de Usuario inválido. Debe ser un número mayor que 0.");
                }
            }

            casaManager.AgregarHabitante(casaId, usuarioId);

            Console.WriteLine("Habitante agregado exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void EliminarHabitante()
        {
            Console.Clear();
            // Mostrar todas las casas
            Casa casaManager = new Casa();
            casaManager.MostrarCasas();
            Console.WriteLine();
            
            int casaId;
            while (true)
            {
                Console.Write("Ingrese ID de la Casa: ");
                if (int.TryParse(Console.ReadLine(), out casaId) && casaId > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ID de Casa inválido. Debe ser un número mayor que 0.");
                }
            }

            int usuarioId;
            while (true)
            {
                Console.Write("Ingrese ID del Usuario: ");
                if (int.TryParse(Console.ReadLine(), out usuarioId) && usuarioId > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ID de Usuario inválido. Debe ser un número mayor que 0.");
                }
            }

            casaManager.EliminarHabitante(casaId, usuarioId);

            Console.WriteLine("Habitante eliminado exitosamente. Presione cualquier tecla para volver...");
            Console.ReadKey();
        }
    }
}