from actuators.fan import FanController
from actuators.led import LedController

from sensors.humidity import HumiditySensor
from sensors.moisture import MoistureSensor
from sensors.temperature import TemperatureSensor
from sensors.water import WaterSensor

from base.sensors import AReading, ISensor
from base.actuators import ACommand, IActuator

from time import sleep

FAN_ON_COMMAND = ACommand("fan", '{"value": "on"}')
FAN_OFF_COMMAND = ACommand("fan", '{"value": "off"}')
FAN_GPIO = 16
FAN_VALUE = {"value": "off"}

LED_ON_COMMAND = ACommand("led", '{"value": "on"}')
LED_OFF_COMMAND = ACommand("led", '{"value": "off"}')
LED_GPIO = 18
LED_VALUE = {"value": "off"}

TEMPERATURE_GPIO = 38
TEMPERATURE_MODEL = "Temperature Sensor"
TEMPERATURE_TYPE = AReading.Type.TEMPERATURE

HUMIDITY_GPIO = 38
HUMIDITY_MODEL = "Humidity Sensor"
HUMIDITY_TYPE = AReading.Type.HUMIDITY

MOISTURE_GPIO = 0x04
MOISTURE_MODEL = "Moisture Sensor"
MOISTURE_TYPE = AReading.Type.MOISTURE

WATER_GPIO = 0x06
WATER_MODEL = "Water Sensor"
WATER_TYPE = AReading.Type.WATER

if __name__ == "__main__":
    fan = FanController(FAN_GPIO, FAN_VALUE)
    led = LedController(LED_GPIO, LED_VALUE)

    humidity = HumiditySensor(HUMIDITY_GPIO, HUMIDITY_MODEL, HUMIDITY_TYPE)
    temperature = TemperatureSensor(
        TEMPERATURE_GPIO, TEMPERATURE_MODEL, TEMPERATURE_TYPE
    )
    moisture = MoistureSensor(MOISTURE_GPIO, MOISTURE_MODEL, MOISTURE_TYPE)
    water = WaterSensor(WATER_GPIO, WATER_MODEL, WATER_TYPE)

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
        print(temperature.read_sensor().__repr__())
        print(humidity.read_sensor().__repr__())
        print(moisture.read_sensor().__repr__())
        print(water.read_sensor().__repr__())

        # sleep
        sleep(1)
