#include <Wire.h>
#include "MAX30100_PulseOximeter.h"
#include <SoftwareSerial.h>

#define OKRES_WYSWIETLANIA 500
SoftwareSerial BTSerial(10, 11); // RX, TX
float tetno, saturacja;
uint32_t poprzedniOdczyt = 0;
PulseOximeter max30100;

void onBeatDetected()
{
    Serial.println("beat");
    BTSerial.println("beat");
}

void setup() {
  max30100.begin();
  max30100.setIRLedCurrent(MAX30100_LED_CURR_14_2MA);
  Serial.begin(9600);
  BTSerial.begin(57600);

  max30100.setOnBeatDetectedCallback(onBeatDetected);
}

void loop() {
  max30100.update();
  if (millis() - poprzedniOdczyt > OKRES_WYSWIETLANIA) {
    saturacja = max30100.getSpO2();
    tetno = max30100.getHeartRate();
    
    Serial.println(String(String(tetno)+";"+String(saturacja)));
    BTSerial.println(String(String(tetno)+";"+String(saturacja)));
    
    poprzedniOdczyt = millis();
  }
}
