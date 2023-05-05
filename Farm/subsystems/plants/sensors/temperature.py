"""This module interfaces with the humidity sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20
from base.sensors import ISensor, AReading

MODEL_NAME = "Temperature Sensor"
GPIO = 38
BUS = 4


class TemperatureSensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.TEMPERATURE,
    ):
        self._sensor = GroveTemperatureHumidityAHT20(gpio, BUS)

    def read_sensor(self) -> list[AReading]:
        temp, hum = self._sensor.read()
        res = AReading(
            AReading.Type.TEMPERATURE,
            AReading.Unit.CELCIUS,
            float("{0:.2f}".format(temp)),
        )
        return [res]


if __name__ == "__main__":
    test = TemperatureSensor(GPIO, BUS)
    while True:
        sleep(0.12)
        print(test.read_sensor())
