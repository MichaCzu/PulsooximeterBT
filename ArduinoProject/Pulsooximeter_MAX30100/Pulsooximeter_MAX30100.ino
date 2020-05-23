#include <Wire.h>
#include "MAX30100_PulseOximeter.h"
#define OKRES_WYSWIETLANIA 200
float tetno;
PulseOximeter max30100;
uint32_t poprzedniOdczyt = 0;

void setup() {
  max30100.begin();
  max30100.setIRLedCurrent(MAX30100_LED_CURR_11MA); 
}

void loop() {
  max30100.update();
  if (millis() - poprzedniOdczyt > OKRES_WYSWIETLANIA) {
    saturacja = max30100.getSpO2();
    tetno = max30100.getHeartRate();

    poprzedniOdczyt = millis();
  }
}
