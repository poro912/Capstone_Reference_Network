// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
ServerToClient.Server server = ServerToClient.Server.Instance;
Thread.Sleep(1000);
server.Stop();

while (true)
{
	Thread.Sleep(1000);
	if (!server.Run)
		break;
}