from subsystems.plants.actuators.fan import FanController
from subsystems.plants.actuators.led import LedController

from subsystems.plants.sensors.humidity import HumiditySensor
from subsystems.plants.sensors.moisture import MoistureSensor
from subsystems.plants.sensors.temperature import TemperatureSensor
from subsystems.plants.sensors.water import WaterSensor

from InterFaces.sensors import AReading, ISensor
from InterFaces.actuators import ACommand, IActuator
from InterFaces.subsystem import ASubsystem

from time import sleep


class PlantsSubSystem(ASubsystem):
    FAN_ON_COMMAND = ACommand("fan", '{"value": "on"}')
    FAN_OFF_COMMAND = ACommand("fan", '{"value": "off"}')
    FAN_GPIO = 5
    FAN_VALUE = {"value": "off"}

    LED_ON_COMMAND = ACommand("led", '{"value": "on"}')
    LED_OFF_COMMAND = ACommand("led", '{"value": "off"}')
    LED_GPIO = 18
    LED_VALUE = {"value": "off"}

    TEMPERATURE_GPIO = 0x38
    TEMPERATURE_MODEL = "Temperature Sensor"
    TEMPERATURE_TYPE = AReading.Type.TEMPERATURE

    HUMIDITY_GPIO = 0x38
    HUMIDITY_MODEL = "Humidity Sensor"
    HUMIDITY_TYPE = AReading.Type.HUMIDITY

    MOISTURE_GPIO = 6
    MOISTURE_MODEL = "Moisture Sensor"
    MOISTURE_TYPE = AReading.Type.HUMIDITY

    WATER_GPIO = 4
    WATER_MODEL = "Water Sensor"
    WATER_TYPE = AReading.Type.WATER

    def __init__(self) -> None:
        self._humidity_sensor = HumiditySensor(
            self.HUMIDITY_GPIO, self.HUMIDITY_MODEL, self.HUMIDITY_TYPE
        )
        self._temperature_sensor = TemperatureSensor(
            self.TEMPERATURE_GPIO, self.TEMPERATURE_MODEL, self.TEMPERATURE_TYPE
        )
        self._moisture_sensor = MoistureSensor(
            self.MOISTURE_GPIO, self.MOISTURE_MODEL, self.MOISTURE_TYPE
        )
        self._water_sensor = WaterSensor(
            self.WATER_GPIO, self.WATER_MODEL, self.WATER_TYPE
        )
        self._sensors: list[ISensor] = [
            self._moisture_sensor,
            self._humidity_sensor,
            self._temperature_sensor,
            self._water_sensor,
        ]
        self._fan_actuator = FanController(PlantsSubSystem.FAN_GPIO, PlantsSubSystem.FAN_VALUE)
        self._led_actuator = LedController(PlantsSubSystem.LED_GPIO, PlantsSubSystem.LED_VALUE)
        self._actuators: list[IActuator] = [
            self._fan_actuator,
            self._led_actuator
            ]
        self._led_actuator.try_command(PlantsSubSystem.LED_OFF_COMMAND)
    def get_name(self):
        return "Plants"


"""
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
        print(humidity.read_sensor()[0].__repr__())
        print(temperature.read_sensor().__repr__())
        print(moisture.read_sensor().__repr__())
        print(water.read_sensor().__repr__())

        # sleep
        sleep(1)
"""
