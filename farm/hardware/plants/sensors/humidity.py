"""This module interfaces with the humidity sensor interface"""
from time import sleep
import json
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20
from ..base.sensors import ISensor, AReading

MODEL_NAME = "Humidity Sensor"
GPIO = 38
BUS = 4
SENSOR_POSITION = 1


class HumiditySensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.HUMIDITY,
    ):
        self._sensor = GroveTemperatureHumidityAHT20(GPIO, BUS)

    def read_sensor(self) -> list[AReading]:
        res = AReading(
            AReading.Type.HUMIDITY,
            AReading.Unit.HUMIDITY,
            float("{0:.2f}".format(self._sensor.read()[SENSOR_POSITION])),
        )
        return [res]


if __name__ == "__main__":
    test = TemperatureSensor(GPIO, BUS)
    while True:
        sleep(0.12)
        print(test.read_sensor())
