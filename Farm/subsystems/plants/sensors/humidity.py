"""This module interfaces with the humidity sensor interface"""
import sys

from time import sleep
import json
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Humidity Sensor"
GPIO = 0x38
BUS = 4


class HumiditySensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.HUMIDITY,
    ):
        self._sensor = GroveTemperatureHumidityAHT20(gpio, BUS)

    def read_sensor(self) -> list[AReading]:
        [temp, hum] = self._sensor.read()
        res = AReading(
            AReading.Type.HUMIDITY,
            AReading.Unit.HUMIDITY,
            float("{0:.2f}".format(hum)),
        )
        return [res]


if __name__ == "__main__":
    test = TemperatureSensor(GPIO, BUS)
    while True:
        sleep(0.12)
        print(test.read_sensor())
