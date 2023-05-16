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
| Device                                                                                                        | Connection Method | Grove Hat Port | Subsystem   |
| :------------------------------------------------------------------------------------------------------------ | ----------------- | -------------- | ----------- |
| [GPS](https://wiki.seeedstudio.com/Grove-GPS-Air530/)                                                         | UART Serial       | UART           | Geolocation |
| [Accelerometer(Pitch/Roll)](https://wiki.seeedstudio.com/reTerminal-hardware-interfaces-usage/#accelerometer) | On board device   | N/A            | Geolocation |
| [AH20 Temp+Humidity sensor](https://media.digikey.com/pdf/Data%20Sheets/Seeed%20Technology/101990644_Web.pdf) | I2C(0x38)         | D26            | Plants      |
| [Water Level Sensor](https://www.waveshare.com/wiki/Liquid_Level_Sensor)                                      | ADC               | A6             | Plants      |
| [Moisture Sensor](https://wiki.seeedstudio.com/Grove-Capacitive_Moisture_Sensor-Corrosion-Resistant/)         | ADC               | A4             | Plants      |
| [Motion Sensor](https://wiki.seeedstudio.com/Grove-Adjustable_PIR_Motion_Sensor/)                             | GPIO              | D22            | Security    |
| [Loudness Sensor](https://wiki.seeedstudio.com/Grove-Loudness_Sensor/)                                        | ADC               | A0             | Security    |
## Controling Actuators
#### Method
For all actuators, we are using **Device Twins**. Using device twins gives us an easy way to ensure our control is persistent in many situations, making it the best choice. By using Device Twins, we can ensure any commands or changes we make will still apply, even if the device is tempoarily offline for whatever reason. However, if somehow we request a change, and wish to be sure it has been applied, we can check the reported properties from the device, which in our implementation, is a 1:1 of the desired properties.
### Schema
We implemented two methods of control for actuators. Those being **manual control** and **rules**. That being said, generally we follow a structure for each actuator and rule that looks like so:
```json
{
    "actuatorControl":{
        "actuatorName":{
            "controlMethod": "manual | rules",//how we are controlling the actuator.
            "manualState": value, //if we are in manual control mode, this is the only value that matters. It decides what we will set the value to.
            "rules" :
            [
                {
                    "targetReadingType": "reading-name", //The reading type (sensor name) that you want to get the value from
                    "targetValue": value, //could be a object for geolocation, bool, float, etc. Just has to match reading type, be comparable, and parseable.
                    "comparisonType": "> OR < OR ==", //Will compare the reading value to the target value with this. I.E. using > would lead to reading > targetValue
                    "valueOnRule": "value" //If this condition is met, apply this value to the actuator.
                }, 
                {...} //you can have as many rules as youd like per actuator!
            ]
        }
    }
}
```
Using this system, you can ensure any conditionals will apply even if the app is closed.
### Manual Control
Manual control is very simple, from the actuator schema, only three values are important.
```json
"actuatorName": {
    "controlMethod": "manual",
    "manualState": value
}
```
**`"actuatorName"`**: The name of the actuator we are controlling (synonymous with the names found in both telemetry, and the reported properties.) This is the key that holds the object per actuator, so **make sure you are setting the key, not value.**

**`"controlMethod"`**: How we are controlling the actuator. If using manual control, always ensure its set to `"manual"`

**`"manualState"`**: The state we want to enter. Whatever is passed here will be served to the actuator as a command. This can be anything, but for all currently implemented actuators, it should be `true` or `false` 

Examples on manual control for ever actuator can be found __[Here](#manual-control-examples)__

### Rules 
The rules system allows you to set up conditons with results based on certain sensor outputs, it allows for more complex, automatic behavior to occur without an app connection, and at a much faster rate without causing network traffic or ratelimits.
```json
{
    "actuatorControl":{
        "actuatorName":{
            "controlMethod": "rules",
            "manualState": value,
            "rules" :[/*list of rules objects*/]
        }
    }
}
```
**`"actuatorName"`**: The name of the actuator we are controlling (synonymous with the names found in both telemetry, and the reported properties.) This is the key that holds the object per actuator, so **make sure you are setting the key, not value.**

**`"controlMethod"`**: How we are controlling the actuator. If using rules control, always ensure its set to `"rules"`

**`"manualState"`**: When using `"rules"` behavior, the behavior of `"manualState"` is altered slightly. That being, in the reported property, `"manualState"` will reflect the **current state** of the actuator, regardless of how it reached that state.
#### Rule Object
The `"rules"` array consist of as many rules as youd like, following this schema:
```json
{
    "targetReadingType": "ReadingType",
    "targetValue": value,
    "comparisonType": ">"/*OR */"<"/*OR*/"==",
    "valueOnRule": value
},
```
**`"targetReadingType"`**: This is the sensor you wish to target, this is automatic with any sensors available to the system. Currently the implemented sensors that can be used are 

- `"Vibration"` (float)
- `"Motion"` (bool) **ONLY WITH `"=="`** 
- `"Loudness"`(float)
- `"Moisture"`(float)
- `"Humidity"`(float)
- `"Temperature"`(float)
- `"Water"`(float)
- `"Coordinates"`(dict) **UNTESTED**
- `"Heading"` (dict) **UNTESTED**

**`"targetValue"`**: This is the "testing" value. It is the other value that will be used for the comparison. **ENSURE IT MATCHES THE TYPE OF THE SENSOR READING**

**`"comparisonType"`**: The comparison to perform. The result of this comparison between the two values will determine if the rule's value is applied. The comparisons available are

- `">"` = `readingValues > targetValue` 
- `"<"` = `readingValues < targetValue` 
- `"=="` = `readingValues == targetValue` 
  
**`"valueOnRule"`**: The value we will set the actuator to if the rule comparison is true. 
### Rules Control Example
```json
"Led": {
    "controlMethod": "rules",
    "manualState": false,
    "rules": [
        {
            "targetReadingType": "Loudness",
            "targetValue": 100,
            "comparisonType": ">",
            "valueOnRule": true
        },
        {
            "targetReadingType": "Loudness",
            "targetValue": 100,
            "comparisonType": "<",
            "valueOnRule": false
        }
    ]
}
```
This rule setup would lead to the Led being turned on whenever the value of the Loudness sensor is over 100, and turned off whenever it is below 100. You could apply this rule to your IOT Device over the CLI utility with this command
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Led": {
        "controlMethod": "rules",
        "manualState": false,
        "rules": [
            {
                "targetReadingType": "Loudness",
                "targetValue": 100,
                "comparisonType": ">",
                "valueOnRule": true
            },
            {
                "targetReadingType": "Loudness",
                "targetValue": 100,
                "comparisonType": "<",
                "valueOnRule": false
            }
        ]
    }
}}'
```
### Manual Control Examples 
This section contains an azure IoT CLI command for each actuator, to turn them on and off.

#### __**Fan**__
Turn fan on:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Fan": {
        "controlMethod": "manual",
        "manualState": true,
        "rules": []
    }
}}'
```
Turn fan off:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Fan": {
        "controlMethod": "manual",
        "manualState": false,
        "rules": []
    }
}}'
```
#### __**Led**__
Turn lights on:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Led": {
        "controlMethod": "manual",
        "manualState": true,
        "rules": []
    }
}}'
```
Turn lights off:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Led": {
        "controlMethod": "manual",
        "manualState": false,
        "rules": []
    }
}}'
```
#### __**Buzzer**__
Turn buzzer sound on:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Buzzer": {
        "controlMethod": "manual",
        "manualState": true,
        "rules": []
    }
}}'
```
Turn buzzer sound off:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "Buzzer": {
        "controlMethod": "manual",
        "manualState": false,
        "rules": []
    }
}}'
```
#### __**DoorLock**__
Lock the door:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "DoorLock": {
        "controlMethod": "manual",
        "manualState": true,
        "rules": []
    }
}}'
```
Unlock the door:
```bash
az iot hub device-twin update -n {HubName} -d {DeviceName} --desired '
{"actuatorControl": {
    "DoorLock": {
        "controlMethod": "manual",
        "manualState": false,
        "rules": []
    }
}}'
```