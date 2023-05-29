using Koko_ClientServer;
using Koko_ClientServer.old;

KokoServer ss = new();
KokoClient sc = new();

List<NetworkComponent> components = new() {
    new TestComponent(),
    new TestComponent2()
};

Console.WriteLine("Choose an option: ");
Console.WriteLine("1. Connect to a server");
Console.WriteLine("2. Start a server");
Console.WriteLine("3. Exit");

string input = Console.ReadLine();

switch (input) {
    case "1":
        Console.WriteLine("Connecting to server...");
        sc.StartClient();
        break;
    case "2":
        Console.WriteLine("Starting server...");
        ss.StartServer(components);
        break;
    case "3":
        Console.WriteLine("Exiting...");
        break;
    default:
        Console.WriteLine("Invalid input");
        Console.WriteLine("Exiting...");
        break;
}

Console.WriteLine("Program ended!");
Console.ReadKey();
