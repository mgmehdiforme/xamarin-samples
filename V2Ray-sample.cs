using System;
using System.Net.Sockets;
using System.Text;
using V2Ray.Core;
using V2RayN.Wpf;

namespace YourNamespace
{
    public class V2RayConnection
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public V2RayConnection(string hostname, int port, string username, string password)
        {
            Hostname = hostname;
            Port = port;
            Username = username;
            Password = password;
        }

        public string Connect()
        {
            try
            {
                Router router = new Router();
                Configuration configuration = new Configuration();

                configuration.InboundConfigs = new InboundConfiguration[]
                {
                    new InboundConfiguration
                    {
                        Protocol = "vmess",
                        Port = Port
                    }
                       DetourConfigs = new DetourConfiguration[]
                        {
                            new DetourConfiguration
                            {
                                Type = "none",
                                Settings = new ConfigurationObject()
                            }
                        },
                        Tag = "inbound"
                    }
                };

                configuration.OutboundConfigs = new OutboundConfiguration[]
                {
                    new OutboundConfiguration
                    {
                        Protocol = "vmess",
                        Settings = new ConfigurationObject
                        {
                            {"vnext", new VNext[]{
                                new VNext
                                {
                                    Address = Hostname,
                                    Port = Port,
                                    Users = new User[]{ new User{ Id = Username, AlterId = 64, SecuritySettings = new SecurityConfig{ Type = "aes-128-gcm"} } }
                                }
                            } }
                        },
                        StreamSettings = new StreamConfig
                        {
                            Network = "tcp",
                            SecuritySettings = new SecurityConfig{ Type = "none" },
                            TcpSettings = new TcpConfig{ HeaderSettings = new HeaderConfig{ Type = "http" } }
                        },
                        Mux = new MuxConfig{ Enabled = true },
                        Tag = "outbound"
                    }
                };

                router.Configure(configuration);
                router.Start();

                TcpClient client = new TcpClient(Hostname, Port);
                NetworkStream stream = client.GetStream();
                string request = "GET / HTTP/1.1\r\nHost: " + Hostname + ":" + Port + "\r\nConnection: close\r\n\r\n";
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = new byte[1024];
                int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                router.Close();
                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
