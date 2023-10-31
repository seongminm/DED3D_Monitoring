void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  static uint32_t started = 0;
  float vol[5];
  String data;
  for(int i = 0; i < 5; i++) {
    vol[i] =  analogRead(i) * 3.3 / 1024.0;
    data += String(vol[i]) + "/";
  }
  data += '\n';
  Serial.print(data);

  if(millis() - started >= 1000) {
    Serial.print("end! \n");
    started = millis();
  }
  
}
