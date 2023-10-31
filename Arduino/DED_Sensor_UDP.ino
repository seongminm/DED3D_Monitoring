#include <WiFiNINA.h>
#include <WiFiUdp.h>

char ssid[] = "iptime";  // Wi-Fi 네트워크 이름
char password[] = "";    // Wi-Fi 네트워크 비밀번호

IPAddress ip;
int port;

unsigned int localPort = 4210;
WiFiUDP Udp;

void setup() {
  Serial.begin(9600);
  // while (!Serial) {
  //   ;  // 시리얼 연결 대기
  // }
  delay(500);
  // Wi-Fi 연결
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("......");
    WiFi.begin(ssid, password);
  }
  Serial.print("Connected! IP address: ");
  Serial.println(WiFi.localIP());

  // UDP 클라이언트 초기화
  Udp.begin(localPort);  // 임의의 로컬 포트 번호 설정
}

void loop() {
  static uint32_t started = 0;
  float vol[6];
  if(Serial.available()) {
    Serial.read();
    Serial.println(WiFi.localIP());
  }

  String data;
  if (millis() - started >= 1000) {
    data = "0/";
    started = millis();
  } else {
    data = "1/";
  }

  for (int i = 0; i < 6; i++) {
    vol[i] = analogRead(i) * 3.3 / 1024.0;
    data += String(vol[i]) + "/";
  }
  data += "\n";
  // data = startBit / visible1 / visible2 / visible3/ powder/ sound / laserPower 
  int packetSize = Udp.parsePacket();
  if (packetSize) {
    ip = Udp.remoteIP();
    port = Udp.remotePort();
    Udp.beginPacket(ip, port);
    Udp.print(data);
    Udp.endPacket();
  } else {
    Udp.beginPacket(ip, port);
    Udp.print(data);
    Udp.endPacket();
  }
}
