from actuators.Buzzer import Buzzer

from sensors.GPS import GPS
from sensors.PitchAndRollSensor import PitchAndRollSensor
from sensors.Vibration import VibrationSensor

from sensors import AReading, ISensor
from actuators import ACommand, IActuator

from time import sleep




class GeoLocation():
    def __init__(self) -> None:
        GPSSensor = GPS(self.HUMIDITY_GPIO,self.HUMIDITY_MODEL,self.HUMIDITY_TYPE)
        PitchAndRoll = PitchAndRollSensor(self.TEMPERATURE_GPIO, self.TEMPERATURE_MODEL, self.TEMPERATURE_TYPE)
        Vibration = VibrationSensor(self.MOISTURE_GPIO, self.MOISTURE_MODEL, self.MOISTURE_TYPE)
        self.sensors: list[ISensor] = [GPSSensor,PitchAndRoll,Vibration]

    def read_sensors(self):
        readings: list[AReading] = []
        for sensor in self.sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        return readings