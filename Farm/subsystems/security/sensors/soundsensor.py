
import math
import sys
import time
from grove.adc import ADC
from InterFaces.sensors import ISensor, AReading

MODEL_NAME = "Grove loudness sensor"

class LoudnessSensor(ISensor):
    """Class representing the loudness sensor"""
    def __init__(self, gpio: int=0 , model: str = MODEL_NAME, type: AReading.Type = AReading.Type.MOTION):
        """Loudness sensor
        
        :param int gpio: The i2c channel
        :param str model: The model name of the sensor
        :param AReading.Type: The type of reading emitted by the sensor."""
        self._adc = ADC(address=0x04)
        self._channel = gpio
    def read_sensor(self) -> list[AReading]:
        return [
        AReading(AReading.Type.LOUDNESS, AReading.Unit.LOUDNESS, self._adc.read(self._channel))
        ]



if __name__ == '__main__':
    sensor = LoudnessSensor()
    while True:
        print(sensor.read_sensor())

