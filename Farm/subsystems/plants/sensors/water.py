"""This module interfaces with the water sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.adc import ADC
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Water Sensor"
GPIO = 6


class WaterSensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.WATER,
    ):
        self._sensor = ADC(gpio)
        self.gpio = gpio

    def read_sensor(self) -> list[AReading]:
        res = AReading(
            AReading.Type.WATER,
            AReading.Unit.WATER,
            float("{0:.2f}".format(self._sensor.read(self.gpio) / 10)),
        )
        return [res]


if __name__ == "__main__":
    test = ADC(GPIO)
    while True:
        sleep(0.12)
        print(test.read_sensor())
