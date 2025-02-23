using Server;
using WebSocketCommunication.Server;

WebSocketServer server = new WebSocketServer(8080);
server.AddService<MessagerClientHandler>("messager/");
server.Run();
