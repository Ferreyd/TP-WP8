using System;
using TP_WP8;
using System.Threading;
using System.Text;
using System.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Foundation;
using Windows.UI.Xaml;
using System.IO;
using TP_WP8.src;
using System.Threading.Tasks;

public class Connexion
{
    private StreamSocket clientSocket;
    private HostName serverHost;
    private string serverPort;
    private bool connected = false;
    private MainPage mainPage;



    public delegate void newLineDelegate(object sender, NewLineEventArgs args);

    public event newLineDelegate newLineEvent;

    public Connexion(MainPage mainPage)
    {
        this.mainPage = mainPage;
        clientSocket = new StreamSocket();
    }

    public async void connect()
    {
        try
        {
            String sendData = "";
            serverHost = new HostName("ovhd2.tokidev.fr");
            serverPort = "4242";

           await clientSocket.ConnectAsync(serverHost, serverPort);

            System.Diagnostics.Debug.WriteLine("Connexion etablie\n ");

            DataWriter writer = new DataWriter(clientSocket.OutputStream);
            // add a newline to the text to send
            
            try
            {
                sendData = "USER nico 0 * : nico\r\n";
                writer.WriteString(sendData);
                await writer.StoreAsync();
              

            }catch(Exception e)
            {
                
                System.Diagnostics.Debug.WriteLine("PREMIERE LIGNE \n " +e.Message);    
            }
            try
            {

                sendData = "NICK nico\r\n";
                writer.WriteString(sendData);
                await writer.StoreAsync();
               

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DEUXIEME LIGNE \n " +e.Message);
            }
            try
            {
                sendData = "JOIN #mbds\r\n";
                writer.WriteString(sendData);
                await writer.StoreAsync();
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TROISIEME LIGNE\n " + e.Message);
            }
           
            
            connected = true;

            // detach the stream and close it
            writer.DetachStream();
            writer.Dispose();
            System.Diagnostics.Debug.WriteLine("Je quitte la méthode CONNECT");



  

        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("CA MARCHE PAS :(((((");
            // If this is an unknown status, 
            // it means that the error is fatal and retry will likely fail.
            if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
            {
                throw;
            }
             // Could retry the connection, but for this simple example
            // just close the socket.

            closing = true;
            // the Close method is mapped to the C# Dispose
            clientSocket.Dispose();
            clientSocket = null;

        }
        System.Diagnostics.Debug.WriteLine("Connected = " + connected);
        
    }

    public async void send(String str)
    {
       // Int32 len = 0; // Gets the UTF-8 string length.
        System.Diagnostics.Debug.WriteLine("Je suis dans la méthode SEND = " + str);

        try
        {
            // add a newline to the text to send
            string sendData = str + Environment.NewLine;
            DataWriter writer = new DataWriter(clientSocket.OutputStream);

            writer.WriteString(sendData);
            await writer.StoreAsync();
      
            // detach the stream and close it
            writer.DetachStream();
           // writer.Dispose();
            System.Diagnostics.Debug.WriteLine("Je quitte la méthode SEND = " + str);

        }
        catch (Exception exception)
        {
            // If this is an unknown status, 
            // it means that the error is fatal and retry will likely fail.
            if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
            {
                throw;
            }

            // Could retry the connection, but for this simple example
            // just close the socket.

            closing = true;
            clientSocket.Dispose();
            clientSocket = null;
            connected = false;

            System.Diagnostics.Debug.WriteLine("J'ai raté la méthode SEND = " + str);

        }

       

    }

    public async void receive()
    {
        String result = "";
        // Now try to receive data from server
        System.Diagnostics.Debug.WriteLine("Je suis dans la méthode RECEIVED");
        try
        {
            DataReader reader = new DataReader(clientSocket.InputStream);
             // set the DataReader to only wait for available data
            reader.InputStreamOptions = InputStreamOptions.Partial;
            // wait for the available data up to 512 bytes
            // count is the number of actually received bytes
           var count = await reader.LoadAsync(512);
   
           // read the data as a string and store it in our container
           if (count > 0)
               result = reader.ReadString(count);

           newLineEvent(this, new NewLineEventArgs(result));
           await mainPage.thread();
      
          
        }
        catch (Exception exception)
        {
            // If this is an unknown status, 
            // it means that the error is fatal and retry will likely fail.
            if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
            {
                throw;
            }

            result = "Receive failed with error: " + exception.Message;
            // Could retry, but for this simple example
            // just close the socket.

            closing = true;
            clientSocket.Dispose();
            clientSocket = null;
            connected = false;

        }
        
    }


    public bool closing { get; set; }
}
