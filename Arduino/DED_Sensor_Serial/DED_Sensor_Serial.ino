void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  static uint32_t started = 0;
  float vol[8];
  String data;
  if (millis() - started >= 1000) {
    data = "0/";
    started = millis();
  } else {
    data = "1/";
  }

  for (int i = 0; i < 8; i++) {
    vol[i] = analogRead(i) * 33000 / 1024;
    data += String(vol[i]) + "/";
  }
  data += "\n";

  Serial.print(data);
}
