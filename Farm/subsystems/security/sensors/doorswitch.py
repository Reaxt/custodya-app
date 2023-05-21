from InterFaces.sensors import ISensor, AReading
from time import sleep
from gpiozero import Button

MODEL_NAME = "SEC-100 Magnetic Door Sensor Reed Switch"
class DoorSensor(ISensor):
        """The door sensor"""
        def __init__(self, gpio: int , model: str = MODEL_NAME, type: AReading.ReadingType = AReading.ReadingType.DOOR):
            """Sensor for door, readings return true when closed.
            
            :param int gpio: The GPIO pin channel
            :param str model: The model name of the sensor
            :param AReading.Type: The type of reading emitted by the sensor."""

            self._sensor = Button(gpio)
        def read_sensor(self) -> list[AReading]:
             return [AReading(AReading.ReadingType.DOOR, AReading.Unit.BOOLEAN, self._sensor.is_held)]

if __name__ == "__main__":
    button = Button(5)
    while True:
        print(button.is_held)
        sleep(0.2)
