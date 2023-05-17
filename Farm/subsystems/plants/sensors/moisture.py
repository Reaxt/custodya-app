"""This module interfaces with the moisture sensor interface"""
import sys

sys.path.append("..")
from time import sleep
import json
from grove.adc import ADC
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Moisture"


class MoistureSensor(ISensor):
    def __init__(
        self,
        gpio: int = 0,
        model: str = MODEL_NAME,
        type: AReading.ReadingType = AReading.ReadingType.MOISTURE,
    ):
        self.sensor = ADC(address=0x04)
        self.channel = gpio

    def read_sensor(self) -> list[AReading]:
        moisture = AReading(
            AReading.ReadingType.MOISTURE,
            AReading.Unit.MOISTURE,
            self.sensor.read(self.channel),
        )

        return [moisture]


if __name__ == "__main__":
    test = Test(4)
    while True:
        sleep(0.12)
        print(test.read_sensor())
