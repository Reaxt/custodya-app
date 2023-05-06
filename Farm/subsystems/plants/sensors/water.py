"""This module interfaces with the water sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.grove_water_sensor import GroveWaterSensor
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Water Sensor"
GPIO = 0x06


class WaterSensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.WATER,
    ):
        self._sensor = GroveWaterSensor(gpio)

    def read_sensor(self) -> list[AReading]:
        res = AReading(
            AReading.Type.WATER,
            AReading.Unit.WATER,
            float("{0:.2f}".format(self._sensor.value() / 10)),
        )
        return [res]


if __name__ == "__main__":
    test = GroveWaterSensor(GPIO)
    while True:
        sleep(0.12)
        print(test.read_sensor())
