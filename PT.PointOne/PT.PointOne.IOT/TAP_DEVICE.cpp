#include <ESP8266WiFi.h>

const char* ssid = "wlankonferanse";
const char* password = "Voksen2013";
const char* host = "pointone.azurewebsites.net";//"192.168.1.107"; 
const int tapPin = 4;
const int statusPin = 2;
int retryCount = 0;
const int retryMax = 10;
const int port = 80;

String RequestId;
int TapStatus = 0;

WiFiClient client;

const String PourStatus =
String("GET ") + "/Tap/Pour HTTP/1.1\r\n" +
"Host: " + host + "\r\n" +
"Connection: keep-alive\r\n" +
"Pragma: no-cache\r\n" +
"Cache-Control: no-cache\r\n" +
"Accept: */*\r\n\r\n";

const String POST(String action) {
	String data = String("RequestId=") + RequestId;
int length = data.length();

	return String("POST") + " http://" + host + "/Tap/" + action + " HTTP/1.1\r\n" +
		"Host: " + host + "\r\n" +
		"User-Agent: Arduino/1.0\r\n" +
		"Connection: keep-alive\r\n" +

        String("Content-Length: ") + length + "\r\n" +
		"Content-Type: application/x-www-form-urlencoded; charset=UTF-8\r\n" +
		"\r\n" +
		data + "\n";

}


void setup(void)
{
    Serial.begin(9600);
    Serial.println("System starting!");
    pinMode(tapPin, OUTPUT);
    pinMode(statusPin, OUTPUT);
    ConnectWiFi();
    Serial.println("Started");
}

void ConnectWiFi()
{
    WiFi.begin(ssid, password);
    Serial.println("Connecting to WiFi");
    while (WiFi.status() != WL_CONNECTED)
    {
        yield();
        delay(500);
    }
    Serial.println("Connected to WiFi");
    Serial.print("IP: ");
    Serial.print(WiFi.localIP());
    Serial.print("\r\n");
}


void loop(void)
{
    // Check for new messages
    yield();
    delay(1000);
    BlinkStatus();
    if (TAP_STATUS() == 1)
    {
        PourBeer();
    }
}

void BlinkStatus()
{
    digitalWrite(statusPin, LOW);
    delay(100);
    digitalWrite(statusPin, HIGH);
}

WiFiClient ConnectClient()
{

    if (client.connected())
        return client;

    Serial.println(String("Connecting to ") + host);
    int counter = 0;
    while (!client.connect(host, port))
    {
        yield();
        delay(500);
        counter++;
        if (counter > 30)
        {
            Serial.println("Failed 30 times, reconnecting!");
            ConnectWiFi();
            ConnectClient();
        }
    }
    return client;
}

void PourBeer()
{
    Serial.println("Pouring beer!");
    POSTMESSAGE("Pouring");
    digitalWrite(tapPin, HIGH);
    delay(5000);
    digitalWrite(tapPin, LOW);
    POSTMESSAGE("Poured");
}

int TAP_STATUS()
{
    //Serial.println("Getting status");    
    client = ConnectClient();
    client.print(PourStatus);
    //Serial.println("Message sent, waiting for response");    
    while (!client.available())
    {
        yield(); delay(100);
    }

    while (client.available())
    {
        String line = client.readStringUntil('\r');
        //Serial.print(line);
        if (line.indexOf("TapStatus") != -1)
        {
            //Serial.println(line);
            int tapStatusStart = line.indexOf("TapStatus") + 11;
            int tapStatusEnd = line.indexOf(",\"RequestId");

            String tapStatus = line.substring(tapStatusStart, tapStatusEnd);
            //Serial.println(String("Tap status") + tapStatus); 
            TapStatus = tapStatus.toInt();

            int requestIdStart = line.indexOf("RequestId") + 12;
            int requestIdStop = line.indexOf(",\"Message") - 1;

            RequestId = line.substring(requestIdStart, requestIdStop);
            //Serial.println(RequestId);       

            return TapStatus;
        }
        yield();
    }
    return 0;
}

int POSTMESSAGE(String message)
{
    Serial.println(String("Sending ") + message);
    client = ConnectClient(); // Reconnect if required  
    String msg = POST(message);
    client.print(msg);
    while (!client.available())
    {
        yield(); delay(100);
    }

    while (client.available())
    {
        String line = client.readStringUntil('\r');
        //Serial.println(line);
    }
    return 0;
}