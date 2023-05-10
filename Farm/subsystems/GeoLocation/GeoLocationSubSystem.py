from subsystems.GeoLocation.actuators.Buzzer import Buzzer

from subsystems.GeoLocation.sensors.GPS import GPS
from subsystems.GeoLocation.sensors.PitchAndRollSensor import PitchAndRollSensor
from subsystems.GeoLocation.sensors.Vibration import VibrationSensor

from InterFaces.sensors import AReading, ISensor
from InterFaces.actuators import ACommand, IActuator
from InterFaces.subsystem import ASubsystem
from time import sleep




class GeoLocation(ASubsystem):
    def __init__(self) -> None:
        GPSSensor = GPS()
        PitchAndRoll = PitchAndRollSensor()
        Vibration = VibrationSensor()
        self._sensors: list[ISensor] = [GPSSensor,PitchAndRoll,Vibration]
        self._actuators: list[IActuator] = [Buzzer()]
    def get_name(self) -> str:
        return "GeoLocation"