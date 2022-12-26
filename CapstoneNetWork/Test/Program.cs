// See https://aka.ms/new-console-template for more information
using Protocol;

Console.WriteLine("Hello, World!");

string input = Console.ReadLine();

Console.WriteLine(Converter.Convert(Generater.Generate(input)));


/*
ServerToClient.Server server = ServerToClient.Server.Instance;
Thread.Sleep(1000);
server.Stop();

while (true)
{
	Thread.Sleep(1000);
	if (!server.Run)
		break;
}
*/