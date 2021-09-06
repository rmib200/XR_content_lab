/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



public class ingresarda : MonoBehaviour
{
    Thread mThread;
    public string connectionIP="127.0.0.1";
    public int connectionPort= 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos=Vector3.zero;

    bool running;

    public InputField nombreField;
    public InputField edadField;  
    public InputField sexoField;
    public InputField ciField;
    public InputField dptoField;
    public Text busco;

    public string resultado="";


 
   public bool guardar=false;
   public bool buscar=false;


    public void guardarDatos(){
       running=false;
       guardar=true;
    }

    
    public void buscarDatos(){
       running=false;
       buscar=true;
    }




    // Start is called before the first frame update
    void Start()
    {
       ThreadStart ts= new ThreadStart(GetInfo);
       mThread = new Thread(ts);
       mThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
         busco.text = "respuesta++++"+resultado;
        //transform.position = receivedPos; 
    }
     
     void GetInfo()
     {
         localAdd=IPAddress.Parse(connectionIP);
         listener=new TcpListener(IPAddress.Any, connectionPort);
         listener.Start();

         client=listener.AcceptTcpClient();

         running=true;
         while(running!=false){
            print("Esperando....");

         }
         if(running==false && guardar==true){
             ingresarDatos();
         }else if(running==false && buscar==true){
             buscarlosDatos();
         }
         listener.Stop();
     }

    void ingresarDatos(){
            string unombre=nombreField.text;
             string uedad=edadField.text;
             string usexo=sexoField.text;
             string uci=ciField.text;
             string udpto=dptoField.text;
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

      
        if (dataReceived != null)
        {
            //---Using received data---
          //  receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
              print("Conexion: "+dataReceived);


            //codigo para ingresar datos
            string qlcypher = "create (a:Person {nombre:'" + unombre +"', edad:  '" + uedad
            +"', sexo: '" + usexo +"', ci: '" + uci + "', dpto: '" + udpto+ "'})";
 

              //---Sending Data to Host----
             byte[] myWriteBuffer = Encoding.ASCII.GetBytes(qlcypher); //Converting string to byte data
             nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        
            
            byte[] responseBuffer = new byte[client.ReceiveBufferSize];
        
            int read;
            do
            {
              read = nwStream.Read(responseBuffer, 0, client.ReceiveBufferSize);
            } while (read > 0);


            //---receiving Data from the Host----
            // int bytes = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
             string data = Encoding.UTF8.GetString(responseBuffer, 0, read); //Converting byte data to string
             print(data);

        }
    }

     void buscarlosDatos(){
     
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

      
        if (dataReceived != null)
        {
            //---Using received data---
          //  receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
              print("Conexion: "+dataReceived);

 

              //---Sending Data to Host----
             byte[] myWriteBuffer = Encoding.ASCII.GetBytes("MATCH(x) return (x) "); //Converting string to byte data
             nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        
            

            //---receiving Data from the Host----
             int bytes = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
             string data = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
             print(data);
             resultado=data;
            

        }
    }



      void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

      
        if (dataReceived != null)
        {
            //---Using received data---
          //  receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
              print("Conexion: "+dataReceived);


              //---Sending Data to Host----
             byte[] myWriteBuffer = Encoding.ASCII.GetBytes("MATCH(x) WHERE x.name='Barcelona' return (x) LIMIT 3"); //Converting string to byte data
             nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        
            

            //---receiving Data from the Host----
             int bytes = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
             string data = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
             print("sera este el fin del hombre ara;a?????"+ data);

        }
    }


    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    
}
