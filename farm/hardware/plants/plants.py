from actuators.fan import FanController
from actuators.led import LedController

from sensors.humidity import HumiditySensor
from sensors.moisture import MoistureSensor
from sensors.temperature import TemperatureSensor
from sensors.water import WaterSensor

from base.sensors import AReading
from base.actuators import ACommand

from time import sleep

FAN_ON_COMMAND = ACommand("fan", {"value": "on"})
FAN_OFF_COMMAND = ACommand("fan", {"value": "off"})
LED_ON_COMMAND = ACommand("led", {"value": "on"})
LED_OFF_COMMAND = ACommand("led", {"value": "off"})

if __name__ == "__main__":
    fan = FanController()
    led = LedController()

    humidity = HumiditySensor()
    temperature = TemperatureSensor()
    moisture = MoistureSensor()
    water = WaterSensor()

    while True:
        # fan control
        if fan._current_state == FAN_ON_COMMAND:
            if fan.validate_command(FAN_OFF_COMMAND):
                fan.control_actuator(FAN_OF_COMMAND.data)
            elif fan.validate_command(FAN_ON_COMMAND):
                fan.control_actuator(FAN_ON_COMMAND.data)

        # led control
        if led._current_state == LED_ON_COMMAND:
            if led.validate_command(LED_OFF_COMMAND):
                led.control_actuator(LED_OF_COMMAND.data)
            elif led.validate_command(LED_ON_COMMAND):
                led.control_actuator(LED_ON_COMMAND.data)

        # print actuator readings
        print(temperature.read_sensor())
        print(humidity.read_sensor())
        print(moisture.read_sensor())
        print(water.read_sensor())

        # sleep
        sleep(1)
