[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/nUJaEIDT)
# <div align='center'>(420-6A6-AB) APP DEV III <br>(420-6P3-AB) Connected Objects <br>Winter 2023</div>

## <div align='center'>Final Project</div>
## <div align='center'>Container-Farms</div>

# Farm Device
## Connection ports
### Actuators
| Device                                                                                                                                         | Connection Method | Grove Hat Port | Subsystem   |
| ---------------------------------------------------------------------------------------------------------------------------------------------- | ----------------- | -------------- | ----------- |
| [Buzzer](https://wiki.seeedstudio.com/reTerminal-hardware-interfaces-usage/#buzzer)                                                            | On board device   | N/A            | Geolocation |
| [Fan](https://abra-electronics.com/thermal-management/fans/dc-fans-5v/5v-cooling-fan-40mm-x-10mm.html)                                         | GPIO Digital      | D5             | Plants      |
| [RGB LED Stick (10 LEDS)](https://wiki.seeedstudio.com/Grove-RGB_LED_Stick-10-WS2813_Mini)                                                     | GPIO              | D18            | Plants      |
| [Servo motor (Door lock)](https://abra-electronics.com/electromechanical/motors/servo-motors/mg90s-metal-gear-micro-servo-rc-micro-servo.html) | GPIO              | D16            | Security    |
### Sensors
| Device | Connection Method | Grove Hat Port | Subsystem |
| :----- | ----------------- | -------------- | --------- |
|[GPS](https://wiki.seeedstudio.com/Grove-GPS-Air530/)| UART Serial| UART| Geolocation |
|[Accelerometer(Pitch/Roll)](https://wiki.seeedstudio.com/reTerminal-hardware-interfaces-usage/#accelerometer) | On board device | N/A | Geolocation
| [AH20 Temp+Humidity sensor](https://media.digikey.com/pdf/Data%20Sheets/Seeed%20Technology/101990644_Web.pdf) | I2C(0x38) | D26 | Plants
[Water Level Sensor](https://www.waveshare.com/wiki/Liquid_Level_Sensor) | ADC | A6 | Plants
[Moisture Sensor](https://wiki.seeedstudio.com/Grove-Capacitive_Moisture_Sensor-Corrosion-Resistant/) | ADC | A4 | Plants
|[Motion Sensor](https://wiki.seeedstudio.com/Grove-Adjustable_PIR_Motion_Sensor/) |  GPIO | D22 | Security
|[Loudness Sensor](https://wiki.seeedstudio.com/Grove-Loudness_Sensor/) | ADC | A0 | Security