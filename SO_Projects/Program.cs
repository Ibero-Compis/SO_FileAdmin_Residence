using System;
using Lab4_FileManagement;

while (true){
    Console.Clear();
    Console.WriteLine("Main Menu:");
    Console.WriteLine("1. Admin Role");
    Console.WriteLine("2. Vigilant Role");
    Console.WriteLine("3. Exit");
    Console.Write("Select an option: ");
    string mainChoice = Console.ReadLine();

    switch (mainChoice){
        case "1":
            AdminMenu();
            break;
        case "2":
            VigilantMenu();
            break;
        case "3":
            return;
        default:
            Console.WriteLine("Invalid option. Press any key to try again...");
            Console.ReadKey();
            break;
    }
}

static void AdminMenu()
{
    while (true){
        Console.Clear();
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Admin Option 1");
        Console.WriteLine("2. Admin Option 2");
        Console.WriteLine("3. Return to Main Menu");
        Console.Write("Select an option: ");
        string adminChoice = Console.ReadLine();

        switch (adminChoice){
            case "1":
                Console.WriteLine("Admin Option 1 selected. Press any key to return...");
                Console.ReadKey();
                break;
            case "2":
                Console.WriteLine("Admin Option 2 selected. Press any key to return...");
                Console.ReadKey();
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid option. Press any key to try again...");
                Console.ReadKey();
                break;
        }
    }
}

static void VigilantMenu()
{
    while (true){
        Console.Clear();
        Console.WriteLine("Vigilant Menu:");
        Console.WriteLine("1. Vigilant Option 1");
        Console.WriteLine("2. Vigilant Option 2");
        Console.WriteLine("3. Return to Main Menu");
        Console.Write("Select an option: ");
        string vigilantChoice = Console.ReadLine();

        switch (vigilantChoice){
            case "1":
                Console.WriteLine("Vigilant Option 1 selected. Press any key to return...");
                Console.ReadKey();
                break;
            case "2":
                Console.WriteLine("Vigilant Option 2 selected. Press any key to return...");
                Console.ReadKey();
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid option. Press any key to try again...");
                Console.ReadKey();
                break;
        }
    }
}