"""This module interfaces with the moisture sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.grove_moisture_sensor import GroveMoistureSensor
from base.sensors import ISensor, AReading

MODEL_NAME = "Moisture Sensor"
GPIO = 0x04


class MoistureSensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.MOISTURE,
    ):
        self._sensor = GroveMoistureSensor(gpio)

    def read_sensor(self) -> list[AReading]:
        res = AReading(
            AReading.Type.MOISTURE,
            AReading.Unit.MOISTURE,
            float("{0:.2f}".format(self._sensor.moisture())),
        )
        return [res]


if __name__ == "__main__":
    test = GroveMoistureSensor(GPIO)
    while True:
        sleep(0.12)
        print(test.read_sensor())
