from time import sleep
from InterFaces.actuators import IActuator, ACommand
from InterFaces.sensors import ISensor, AReading
from InterFaces.subsystem import ASubsystem
from subsystems.security.actuators.servo import DoorController
from subsystems.security.sensors.doorswitch import DoorSensor
from subsystems.security.sensors.motionsensor import MotionSensor
from subsystems.security.sensors.soundsensor import LoudnessSensor
DOOR_PIN = 24
MOTION_PIN = 12
LOUDNESS_BUS = 0
SERVO_PIN = 16


class SecuritySubSystem(ASubsystem):

    def __init__(self) -> None:
        self._sensors: list[ISensor] = [
        DoorSensor(DOOR_PIN),
        MotionSensor(MOTION_PIN),
        LoudnessSensor(LOUDNESS_BUS)
        ]
        self._actuators: list[IActuator] = [
            DoorController(SERVO_PIN)
        ]
    def get_name(self) -> str:
        return "Security"



"""
if __name__ == "__main__":
    sensors: list[ISensor] = [
        DoorSensor(5),
        MotionSensor(MOTION_PIN),
        LoudnessSensor(LOUDNESS_BUS)
    ]
    servo = DoorController(SERVO_PIN)
    servostate = False
    oncommand = ACommand(DoorController.TARGET, '{"value":"on"}')
    offcommand = ACommand(DoorController.TARGET, '{"value":"off"}')
    while True:
        servostate = not servostate
        readings: list[AReading] = []
        for sensor in sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        print(readings)
        if servostate:
            servo.control_actuator(oncommand.data)
        else:
            servo.control_actuator(offcommand.data)
        sleep(1)
"""