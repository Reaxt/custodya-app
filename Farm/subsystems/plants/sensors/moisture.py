"""This module interfaces with the moisture sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.adc import ADC
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Moisture Sensor"
GPIO = 4


class MoistureSensor(ISensor):
    def __init__(
        self,
        gpio: int,
        model: str = MODEL_NAME,
        type: AReading.Type = AReading.Type.MOISTURE,
    ):
        self._sensor = ADC(gpio)
        self.gpio = gpio

    def read_sensor(self) -> list[AReading]:
        res = AReading(
            AReading.Type.MOISTURE,
            AReading.Unit.MOISTURE,
            float("{0:.2f}".format(self._sensor.read_voltage(self.gpio))),
        )
        return [res]


if __name__ == "__main__":
    test = ADC(GPIO)
    while True:
        sleep(0.12)
        print(test.read_sensor())
