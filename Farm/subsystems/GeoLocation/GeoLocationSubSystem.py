from subsystems.GeoLocation.actuators.Buzzer import Buzzer

from subsystems.GeoLocation.sensors.GPS import GPS
from subsystems.GeoLocation.sensors.PitchAndRollSensor import PitchAndRollSensor
from subsystems.GeoLocation.sensors.Vibration import VibrationSensor

from InterFaces.sensors import AReading, ISensor
from InterFaces.actuators import ACommand, IActuator

from time import sleep




class GeoLocation():
    def __init__(self) -> None:
        GPSSensor = GPS()
        PitchAndRoll = PitchAndRollSensor()
        Vibration = VibrationSensor()
        self.sensors: list[ISensor] = [GPSSensor,PitchAndRoll,Vibration]

    def read_sensors(self):
        readings: list[AReading] = []
        for sensor in self.sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        return readings