"""This module interfaces with the motion sensor interface"""
from time import sleep
import json
from grove.gpio import GPIO
from grove.grove_mini_pir_motion_sensor import GroveMiniPIRMotionSensor
from gpiozero import DigitalInputDevice
from sensors import ISensor, AReading

MODEL_NAME = "PIR Motion Sensor"

class MotionSensor(ISensor):
    def __init__(self, gpio: int , model: str = MODEL_NAME, type: AReading.Type = AReading.Type.MOTION):
        self._sensor = DigitalInputDevice(12)
    def read_sensor(self) -> list[AReading]:
        res = AReading(AReading.Type.MOTION, AReading.Unit.BOOLEAN, True if (self._sensor.value == 1) else False)
        return [res]
        

if __name__ == "__main__":
    test = MotionSensor(12)
    while True:
        sleep(0.12)
        print(test.read_sensor())

